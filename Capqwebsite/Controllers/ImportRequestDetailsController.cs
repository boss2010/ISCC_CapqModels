using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Net.Http.Json;
using System.Text.Json;
using ViewModels;

namespace Capqwebsite.Controllers;

public class ImportRequestDetailsController : Controller
{
    private const string ImportApi = "http://10.7.7.250:40/api/Import_CheckRequest_API";
    private const string FinalResultApi = "http://localhost:49310/api/ImCommittee_Final_Result_API";
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _connectionString;

    public ImportRequestDetailsController(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        _httpClientFactory = httpClientFactory;
        _connectionString = configuration.GetConnectionString("DBConnection")
            ?? throw new InvalidOperationException("DBConnection is not configured.");
    }

    [HttpGet]
    public async Task<IActionResult> Index(long checkRequestId, long committeeId, byte committeeTypeId)
    {
        if (HttpContext.Session.GetString("UserSession") != "Authenticated")
            return RedirectToAction("Index", "Login");

        if (checkRequestId <= 0 || committeeId <= 0 || committeeTypeId <= 0)
        {
            TempData["PageError"] = "بيانات طلب الوارد غير مكتملة.";
            return RedirectToAction("Index", "ImportRequests");
        }

        try
        {
            return View(await LoadPageAsync(checkRequestId, committeeId, committeeTypeId));
        }
        catch (Exception ex)
        {
            TempData["PageError"] = $"تعذر تحميل تفاصيل طلب الوارد: {ex.Message}";
            return RedirectToAction("Index", "ImportRequests");
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveInspection(ImportInspectionSaveVm input)
    {
        if (HttpContext.Session.GetString("UserSession") != "Authenticated")
            return RedirectToAction("Index", "Login");

        if (input.CommitteeTypeId != 11)
        {
            TempData["SaveError"] = "حفظ نتيجة الفحص وموقف الحجر متاح للجنة الفحص فقط.";
            return RedirectToAction(nameof(Index), RouteValues(input));
        }

        var access = await GetCommitteeAccessAsync(input.CheckRequestId, input.CommitteeId);
        if (!access.IsAdmin)
        {
            TempData["SaveError"] = "المساعد لا يمكنه تعديل نتيجة الأدمن؛ المتاح له تسجيل الرأي فقط.";
            return RedirectToAction(nameof(Index), RouteValues(input));
        }
        if (access.IsReadOnly)
        {
            TempData["SaveError"] = "تم الانتهاء من هذه اللجنة وأصبحت متاحة للعرض فقط.";
            return RedirectToAction(nameof(Index), RouteValues(input));
        }

        if (input.ApplyToAll)
        {
            foreach (var lot in input.Lots)
            {
                lot.CommitteeResultTypeId = input.SharedCommitteeResultTypeId;
                lot.QuarantineStatusId = input.SharedQuarantineStatusId;
                lot.Notes = input.SharedNotes;
                lot.InfectionItemId = input.SharedInfectionItemId;
            }
        }

        if (input.Lots.Count == 0 || input.Lots.Any(x => x.LotCategoryId <= 0 || x.CommitteeResultTypeId is not (1 or 3)
            || x.QuarantineStatusId <= 0 || string.IsNullOrWhiteSpace(x.Notes)
            || (x.CommitteeResultTypeId == 3 && x.InfectionItemId <= 0)))
        {
            TempData["SaveError"] = "يجب استكمال نتيجة الفحص وموقف الحجر والملاحظات، واختيار نوع الإصابة عند رفض النتيجة.";
            return RedirectToAction(nameof(Index), RouteValues(input));
        }

        if (!short.TryParse(HttpContext.Session.GetString("UserId"), out short userId))
        {
            TempData["SaveError"] = "تعذر تحديد رقم المستخدم. برجاء تسجيل الدخول مرة أخرى.";
            return RedirectToAction("Index", "Login");
        }

        try
        {
            var lotItems = await ResolveLotItemsAsync(input.CheckRequestId, input.Lots.Select(x => x.LotCategoryId));
            if (lotItems.Count != input.Lots.Count)
                throw new InvalidOperationException("أحد اللوطات لا يتبع طلب الوارد الحالي أو بياناته غير مكتملة.");

            await SaveInspectionTransactionAsync(input, userId);
            TempData["SaveSuccess"] = "تم حفظ نتيجة الفحص وموقف الحجر بنجاح.";
        }
        catch (Exception ex)
        {
            TempData["SaveError"] = ex.Message;
        }

        return RedirectToAction(nameof(Index), RouteValues(input));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveAssistantOpinion(AssistantOpinionSaveVm input)
    {
        if (HttpContext.Session.GetString("UserSession") != "Authenticated" ||
            !short.TryParse(HttpContext.Session.GetString("UserId"), out short userId))
            return RedirectToAction("Index", "Login");

        var access = await GetCommitteeAccessAsync(input.CheckRequestId, input.CommitteeId);
        var submittedOpinions = input.Opinions.Where(x => x.ResultId > 0 && x.IsAccepted != null).ToList();
        if (access.IsAdmin || access.IsReadOnly || submittedOpinions.Count == 0)
        {
            TempData["SaveError"] = access.IsReadOnly ? "تم تسجيل رأيك على هذه اللجنة من قبل." : "يجب تحديد موافق أو غير موافق لكل نتيجة.";
            return RedirectToAction(nameof(Index), new { checkRequestId = input.CheckRequestId, committeeId = input.CommitteeId, committeeTypeId = input.CommitteeTypeId });
        }

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var transaction = (SqlTransaction)await connection.BeginTransactionAsync();
        try
        {
            const string sql = """
                INSERT INTO dbo.Im_CommitteeResult_Confirm
                    (ID, Im_CommitteeResult_ID, [Date], EmployeeId, Notes, IsAccepted)
                SELECT NEXT VALUE FOR dbo.Im_CommitteeResult_Confirm_SEQ,
                       result.ID, GETDATE(), @UserId, @Notes, @IsAccepted
                FROM dbo.Im_CommitteeResult result
                WHERE result.ID = @ResultId AND result.Committee_ID = @CommitteeId
                  AND result.CommitteeResultType_ID IS NOT NULL
                  AND NOT EXISTS
                    (SELECT 1 FROM dbo.Im_CommitteeResult_Confirm confirm
                     WHERE confirm.Im_CommitteeResult_ID = result.ID AND confirm.EmployeeId = @UserId)
                """;
            foreach (var opinion in submittedOpinions)
            {
                await using var command = new SqlCommand(sql, connection, transaction);
                command.Parameters.AddWithValue("@UserId", userId);
                command.Parameters.AddWithValue("@Notes", (object?)opinion.Notes?.Trim() ?? DBNull.Value);
                command.Parameters.AddWithValue("@IsAccepted", opinion.IsAccepted!.Value);
                command.Parameters.AddWithValue("@ResultId", opinion.ResultId);
                command.Parameters.AddWithValue("@CommitteeId", input.CommitteeId);
                if (await command.ExecuteNonQueryAsync() != 1)
                    throw new InvalidOperationException("تعذر حفظ الرأي أو تم تسجيله من قبل.");
            }
            await transaction.CommitAsync();
            TempData["SaveSuccess"] = "تم حفظ رأي المساعد بنجاح.";
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            TempData["SaveError"] = ex.Message;
        }
        return RedirectToAction(nameof(Index), new { checkRequestId = input.CheckRequestId, committeeId = input.CommitteeId, committeeTypeId = input.CommitteeTypeId });
    }

    private async Task<ImportRequestDetailsPageVm> LoadPageAsync(long checkRequestId, long committeeId, byte committeeTypeId)
    {
        var client = _httpClientFactory.CreateClient();
        var detailsResponse = await client.GetAsync(
            $"{ImportApi}?CheckRequest_Id={checkRequestId}&Committee_Id={committeeId}&Committee_Type_Id={committeeTypeId}");
        detailsResponse.EnsureSuccessStatusCode();
        var details = await detailsResponse.Content.ReadFromJsonAsync<Ex_CheckRequest_GetData_Android_V2_VM>(
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
            ?? throw new InvalidOperationException("لم يتم إرجاع بيانات للطلب.");

        var statuses = new List<QuarantineStatusVm>();
        if (committeeTypeId == 11)
        {
            using var statusRequest = new HttpRequestMessage(HttpMethod.Get, FinalResultApi);
            statusRequest.Headers.Add("lang", "1");
            var statusResponse = await client.SendAsync(statusRequest);
            statusResponse.EnsureSuccessStatusCode();
            statuses = await statusResponse.Content.ReadFromJsonAsync<List<QuarantineStatusVm>>(
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
            statuses = statuses.Where(x => x.Value > 0).ToList();
        }

        var access = await GetCommitteeAccessAsync(checkRequestId, committeeId);
        var infectionItems = committeeTypeId == 11 && access.IsAdmin
            ? await LoadInfectionItemsAsync()
            : new List<InfectionItemVm>();
        return new ImportRequestDetailsPageVm
        {
            Details = details,
            CheckRequestId = checkRequestId,
            CommitteeId = committeeId,
            CommitteeTypeId = committeeTypeId,
            UserId = HttpContext.Session.GetString("UserId") ?? string.Empty,
            UserName = HttpContext.Session.GetString("UserName") ?? string.Empty,
            QuarantineStatuses = statuses,
            IsReadOnly = access.IsReadOnly,
            IsAdmin = access.IsAdmin,
            InfectionItems = infectionItems,
            AssistantResults = access.IsAdmin ? new() : await LoadAssistantResultsAsync(committeeId, long.Parse(HttpContext.Session.GetString("UserId")!))
        };
    }

    private async Task<List<InfectionItemVm>> LoadInfectionItemsAsync()
    {
        var items = new List<InfectionItemVm>();
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        const string sql = """
            SELECT item.ID, item.Name_Ar, item.Scientific_Name,
                   item.Group_ID, grp.Name_Ar AS GroupName,
                   item.Family_ID, family.Name_Ar AS FamilyName
            FROM dbo.Item item
            LEFT JOIN dbo.[Group] grp ON grp.ID = item.Group_ID
            LEFT JOIN dbo.Family family ON family.ID = item.Family_ID
            WHERE item.User_Deletion_Id IS NULL
              AND NULLIF(LTRIM(RTRIM(item.Name_Ar)), '') IS NOT NULL
            ORDER BY grp.Name_Ar, family.Name_Ar, item.Name_Ar
            """;
        await using var command = new SqlCommand(sql, connection);
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
            items.Add(new InfectionItemVm
            {
                ItemId = reader.GetInt64(0),
                ItemName = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                ScientificName = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
                GroupId = reader.IsDBNull(3) ? 0 : reader.GetInt32(3),
                GroupName = reader.IsDBNull(4) ? "غير محدد" : reader.GetString(4),
                FamilyId = reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                FamilyName = reader.IsDBNull(6) ? "غير محدد" : reader.GetString(6)
            });
        return items;
    }

    private static object RouteValues(ImportInspectionSaveVm input) => new
    {
        checkRequestId = input.CheckRequestId,
        committeeId = input.CommitteeId,
        committeeTypeId = input.CommitteeTypeId
    };

    private async Task<Dictionary<long, LotItemIdentity>> ResolveLotItemsAsync(
        long checkRequestId, IEnumerable<long> lotIds)
    {
        var result = new Dictionary<long, LotItemIdentity>();
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        const string sql = """
            SELECT lot.ID, item.ID AS RequestItemId, item.Item_ShortName_ID
            FROM dbo.Im_CheckRequest_Items_Lot_Category lot
            INNER JOIN dbo.Im_CheckRequest_Items item ON item.ID = lot.Im_CheckRequest_Items_ID
            INNER JOIN dbo.Im_CheckRequset_Shipping_Method shipping
                ON shipping.ID = item.Im_CheckRequset_Shipping_Method_ID
            WHERE lot.ID = @LotId AND shipping.Im_CheckRequest_ID = @CheckRequestId
            """;

        foreach (long lotId in lotIds.Distinct())
        {
            await using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@LotId", lotId);
            command.Parameters.AddWithValue("@CheckRequestId", checkRequestId);
            await using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
                result[lotId] = new LotItemIdentity(reader.GetInt64(1), reader.GetInt64(2));
        }

        return result;
    }

    private async Task<CommitteeAccess> GetCommitteeAccessAsync(long checkRequestId, long committeeId)
    {
        long.TryParse(HttpContext.Session.GetString("UserId"), out long userId);
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        const string sql = """
            SELECT employee.ISAdmin,
              CASE WHEN committee.Is_Cancel IS NOT NULL THEN 1
                   WHEN employee.ISAdmin = 1 AND (committee.Status = 1 OR committee.IsFinishedAll = 1) THEN 1
                   WHEN employee.ISAdmin = 0 AND NOT EXISTS
                     (SELECT 1 FROM dbo.Im_CommitteeResult result
                      WHERE result.Committee_ID = committee.ID AND result.CommitteeResultType_ID IS NOT NULL
                        AND NOT EXISTS (SELECT 1 FROM dbo.Im_CommitteeResult_Confirm confirm
                                        WHERE confirm.Im_CommitteeResult_ID = result.ID AND confirm.EmployeeId = @UserId))
                     AND EXISTS (SELECT 1 FROM dbo.Im_CommitteeResult result WHERE result.Committee_ID = committee.ID AND result.CommitteeResultType_ID IS NOT NULL)
                   THEN 1 ELSE 0 END AS IsReadOnly
            FROM dbo.Im_RequestCommittee committee
            INNER JOIN dbo.CommitteeEmployee employee ON employee.Committee_ID = committee.ID
              AND employee.Employee_Id = @UserId AND employee.OperationType = 74 AND employee.User_Deletion_Id IS NULL
            WHERE committee.ID = @CommitteeId AND committee.ImCheckRequest_ID = @CheckRequestId
            """;
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@CommitteeId", committeeId);
        command.Parameters.AddWithValue("@CheckRequestId", checkRequestId);
        command.Parameters.AddWithValue("@UserId", userId);
        await using var reader = await command.ExecuteReaderAsync();
        return await reader.ReadAsync()
            ? new CommitteeAccess(Convert.ToBoolean(reader[0]), Convert.ToBoolean(reader[1]))
            : new CommitteeAccess(false, true);
    }

    private async Task<List<AssistantResultVm>> LoadAssistantResultsAsync(long committeeId, long userId)
    {
        var results = new List<AssistantResultVm>();
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        const string sql = """
            SELECT result.ID, result.LotData_ID, lot.Lot_Number, type.Name_Ar,
                   result.Notes, result.IS_Total, confirm.IsAccepted, confirm.Notes AS ConfirmNotes,
                   lotStatus.Name_AR AS QuarantineStatus
            FROM dbo.Im_CommitteeResult result
            LEFT JOIN dbo.Im_CheckRequest_Items_Lot_Category lot ON lot.ID = result.LotData_ID
            LEFT JOIN dbo.CommitteeResultType type ON type.ID = result.CommitteeResultType_ID
            LEFT JOIN dbo.Im_CommitteeResult_Confirm confirm ON confirm.Im_CommitteeResult_ID = result.ID AND confirm.EmployeeId = @UserId
            OUTER APPLY (SELECT TOP 1 status.Name_AR FROM dbo.Im_CheckRequest_Items_Lot_Result lr
                         INNER JOIN dbo.Im_CheckRequest_Lot_Result_Status status ON status.ID = lr.IS_Status
                         WHERE lr.Im_CheckRequest_Items_Lot_Category_ID = result.LotData_ID ORDER BY lr.ID DESC) lotStatus
            WHERE result.Committee_ID = @CommitteeId AND result.CommitteeResultType_ID IS NOT NULL
            ORDER BY result.ID
            """;
        await using var command = new SqlCommand(sql, connection);
        command.Parameters.AddWithValue("@CommitteeId", committeeId);
        command.Parameters.AddWithValue("@UserId", userId);
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
            results.Add(new AssistantResultVm
            {
                ResultId = reader.GetInt64(0), LotId = reader.IsDBNull(1) ? 0 : reader.GetInt64(1),
                LotNumber = reader.IsDBNull(2) ? "—" : reader.GetString(2), ResultName = reader.IsDBNull(3) ? "—" : reader.GetString(3),
                AdminNotes = reader.IsDBNull(4) ? string.Empty : reader.GetString(4), IsTotal = !reader.IsDBNull(5) && reader.GetBoolean(5),
                ExistingAccepted = reader.IsDBNull(6) ? null : reader.GetBoolean(6), ExistingNotes = reader.IsDBNull(7) ? string.Empty : reader.GetString(7),
                QuarantineStatus = reader.IsDBNull(8) ? "—" : reader.GetString(8)
            });
        return results;
    }

    private async Task SaveInspectionTransactionAsync(ImportInspectionSaveVm input, short userId)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        await using var transaction = (SqlTransaction)await connection.BeginTransactionAsync();

        try
        {
            const string updateResultSql = """
                UPDATE dbo.Im_CommitteeResult
                SET CommitteeResultType_ID = @ResultTypeId,
                    EmployeeId = @UserId,
                    [Date] = GETDATE(),
                    QuantitySize = @QuantitySize,
                    [Weight] = @Weight,
                    Notes = @Notes,
                    IS_Total_Android = @IsTotal,
                    User_Updation_Id = @UserId,
                    User_Updation_Date = GETDATE()
                WHERE Committee_ID = @CommitteeId
                  AND LotData_ID = @LotId
                  AND User_Deletion_Id IS NULL
                """;

            const string closeOldStatusesSql = """
                UPDATE dbo.Im_CheckRequest_Items_Lot_Result
                SET IS_Status_Committee = 0
                WHERE Im_CheckRequest_Items_Lot_Category_ID = @LotId
                """;

            const string insertStatusSql = """
                INSERT INTO dbo.Im_CheckRequest_Items_Lot_Result
                    (ID, Im_CheckRequest_Items_Lot_Category_ID, Nots,
                     User_Creation_Id, User_Creation_Date, IS_Status)
                VALUES
                    (NEXT VALUE FOR dbo.Im_CheckRequest_Items_Lot_Result_SEQ,
                     @LotId, @Notes, @UserId, GETDATE(), @StatusId)
                """;

            const string clearInfectionsSql = """
                UPDATE infection
                SET User_Deletion_Id = @UserId, User_Deletion_Date = GETDATE()
                FROM dbo.Im_CommitteeResult_Infection infection
                INNER JOIN dbo.Im_CommitteeResult result ON result.ID = infection.Im_CommitteeResult_ID
                WHERE result.Committee_ID = @CommitteeId AND result.LotData_ID = @LotId
                  AND infection.User_Deletion_Id IS NULL
                """;

            const string insertInfectionSql = """
                INSERT INTO dbo.Im_CommitteeResult_Infection
                    (ID, Im_CommitteeResult_ID, Item_ID, User_Creation_Id, User_Creation_Date)
                SELECT NEXT VALUE FOR dbo.Im_CommitteeResult_Infection_SEQ,
                       result.ID, @InfectionItemId, @UserId, GETDATE()
                FROM dbo.Im_CommitteeResult result
                WHERE result.Committee_ID = @CommitteeId AND result.LotData_ID = @LotId
                  AND result.User_Deletion_Id IS NULL
                """;

            foreach (var lot in input.Lots)
            {
                await using var updateResult = new SqlCommand(updateResultSql, connection, transaction);
                updateResult.Parameters.AddWithValue("@ResultTypeId", lot.CommitteeResultTypeId);
                updateResult.Parameters.AddWithValue("@UserId", userId);
                updateResult.Parameters.AddWithValue("@QuantitySize", (object?)lot.QuantitySize ?? DBNull.Value);
                updateResult.Parameters.AddWithValue("@Weight", (object?)lot.Weight ?? DBNull.Value);
                updateResult.Parameters.AddWithValue("@Notes", lot.Notes.Trim());
                updateResult.Parameters.AddWithValue("@IsTotal", lot.IsTotal);
                updateResult.Parameters.AddWithValue("@CommitteeId", input.CommitteeId);
                updateResult.Parameters.AddWithValue("@LotId", lot.LotCategoryId);
                int updatedResults = await updateResult.ExecuteNonQueryAsync();
                if (updatedResults != 1)
                    throw new InvalidOperationException($"لم يتم العثور على سجل نتيجة الفحص للوط {lot.LotCategoryId} داخل اللجنة {input.CommitteeId}.");

                await using var closeStatuses = new SqlCommand(closeOldStatusesSql, connection, transaction);
                closeStatuses.Parameters.AddWithValue("@LotId", lot.LotCategoryId);
                await closeStatuses.ExecuteNonQueryAsync();

                await using var insertStatus = new SqlCommand(insertStatusSql, connection, transaction);
                insertStatus.Parameters.AddWithValue("@LotId", lot.LotCategoryId);
                insertStatus.Parameters.AddWithValue("@Notes", lot.Notes.Trim());
                insertStatus.Parameters.AddWithValue("@UserId", userId);
                insertStatus.Parameters.AddWithValue("@StatusId", lot.QuarantineStatusId);
                if (await insertStatus.ExecuteNonQueryAsync() != 1)
                    throw new InvalidOperationException($"تعذر إدخال موقف الحجر للوط {lot.LotCategoryId}.");

                await using var clearInfections = new SqlCommand(clearInfectionsSql, connection, transaction);
                clearInfections.Parameters.AddWithValue("@UserId", userId);
                clearInfections.Parameters.AddWithValue("@CommitteeId", input.CommitteeId);
                clearInfections.Parameters.AddWithValue("@LotId", lot.LotCategoryId);
                await clearInfections.ExecuteNonQueryAsync();

                if (lot.CommitteeResultTypeId == 3)
                {
                    if (lot.InfectionItemId <= 0)
                        throw new InvalidOperationException($"يجب اختيار نوع الإصابة للوط {lot.LotCategoryId} لأن النتيجة مرفوضة.");
                    await using var insertInfection = new SqlCommand(insertInfectionSql, connection, transaction);
                    insertInfection.Parameters.AddWithValue("@InfectionItemId", lot.InfectionItemId);
                    insertInfection.Parameters.AddWithValue("@UserId", userId);
                    insertInfection.Parameters.AddWithValue("@CommitteeId", input.CommitteeId);
                    insertInfection.Parameters.AddWithValue("@LotId", lot.LotCategoryId);
                    if (await insertInfection.ExecuteNonQueryAsync() != 1)
                        throw new InvalidOperationException($"تعذر حفظ نوع الإصابة للوط {lot.LotCategoryId}.");
                }
            }

            const string updateCommitteeSql = """
                UPDATE dbo.Im_RequestCommittee
                SET Status = 1,
                    IsFinishedAll = @IsFinishedAll,
                    User_Updation_Id = @UserId,
                    User_Updation_Date = GETDATE()
                WHERE ID = @CommitteeId
                  AND ImCheckRequest_ID = @CheckRequestId
                  AND User_Deletion_Id IS NULL
                """;
            await using var updateCommittee = new SqlCommand(updateCommitteeSql, connection, transaction);
            updateCommittee.Parameters.AddWithValue("@IsFinishedAll", input.IsFinishedAll);
            updateCommittee.Parameters.AddWithValue("@UserId", userId);
            updateCommittee.Parameters.AddWithValue("@CommitteeId", input.CommitteeId);
            updateCommittee.Parameters.AddWithValue("@CheckRequestId", input.CheckRequestId);
            if (await updateCommittee.ExecuteNonQueryAsync() != 1)
                throw new InvalidOperationException("لم يتم العثور على اللجنة التابعة للطلب الحالي.");

            await transaction.CommitAsync();
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}

public record LotItemIdentity(long RequestItemId, long ItemShortNameId);
public record CommitteeAccess(bool IsAdmin, bool IsReadOnly);

public class ImportRequestDetailsPageVm
{
    public Ex_CheckRequest_GetData_Android_V2_VM Details { get; set; } = new();
    public long CheckRequestId { get; set; }
    public long CommitteeId { get; set; }
    public byte CommitteeTypeId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public List<QuarantineStatusVm> QuarantineStatuses { get; set; } = new();
    public List<InfectionItemVm> InfectionItems { get; set; } = new();
    public bool IsReadOnly { get; set; }
    public bool IsAdmin { get; set; }
    public List<AssistantResultVm> AssistantResults { get; set; } = new();
}

public class InfectionItemVm
{
    public long ItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string ScientificName { get; set; } = string.Empty;
    public int GroupId { get; set; }
    public string GroupName { get; set; } = string.Empty;
    public int FamilyId { get; set; }
    public string FamilyName { get; set; } = string.Empty;
}

public class AssistantResultVm
{
    public long ResultId { get; set; }
    public long LotId { get; set; }
    public string LotNumber { get; set; } = string.Empty;
    public string ResultName { get; set; } = string.Empty;
    public string AdminNotes { get; set; } = string.Empty;
    public string QuarantineStatus { get; set; } = string.Empty;
    public bool IsTotal { get; set; }
    public bool? ExistingAccepted { get; set; }
    public string ExistingNotes { get; set; } = string.Empty;
}

public class AssistantOpinionSaveVm
{
    public long CheckRequestId { get; set; }
    public long CommitteeId { get; set; }
    public byte CommitteeTypeId { get; set; }
    public List<AssistantLotOpinionVm> Opinions { get; set; } = new();
}

public class AssistantLotOpinionVm
{
    public long ResultId { get; set; }
    public bool? IsAccepted { get; set; }
    public string? Notes { get; set; }
}

public class QuarantineStatusVm
{
    public int Value { get; set; }
    public bool? Value2 { get; set; }
    public string DisplayText { get; set; } = string.Empty;
}

public class ImportInspectionSaveVm
{
    public long CheckRequestId { get; set; }
    public long CommitteeId { get; set; }
    public byte CommitteeTypeId { get; set; }
    public bool IsFinishedAll { get; set; }
    public bool ApplyToAll { get; set; }
    public byte SharedCommitteeResultTypeId { get; set; }
    public int SharedQuarantineStatusId { get; set; }
    public string SharedNotes { get; set; } = string.Empty;
    public long SharedInfectionItemId { get; set; }
    public List<ImportInspectionLotVm> Lots { get; set; } = new();
}

public class ImportInspectionLotVm
{
    public long LotCategoryId { get; set; }
    public bool IsTotal { get; set; }
    public decimal? Weight { get; set; }
    public decimal? QuantitySize { get; set; }
    public byte CommitteeResultTypeId { get; set; }
    public int QuarantineStatusId { get; set; }
    public string Notes { get; set; } = string.Empty;
    public long InfectionItemId { get; set; }
}
