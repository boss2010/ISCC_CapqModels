using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using EF.Imports;
using Microsoft.EntityFrameworkCore;
using PlantMobile.Data;
using static ViewModels.ExportImportActivity_API;

namespace PlantMobile.Controllers;

public class ImportRequestsController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly ILogger<ImportRequestsController> _logger;
    private readonly IDbContextFactory<ImportDbContext> _dbFactory;

    public ImportRequestsController(
        IConfiguration configuration,
        ILogger<ImportRequestsController> logger,
        IDbContextFactory<ImportDbContext>? dbFactory = null)
    {
        _configuration = configuration;
        _logger = logger;
        _dbFactory = dbFactory ?? ImportDatabase.CreateFactory(configuration);
    }

    [HttpGet]
    public async Task<IActionResult> QuarantineLots(long checkRequestId)
    {
        if (HttpContext.Session.GetString("UserSession") != "Authenticated" ||
            !long.TryParse(HttpContext.Session.GetString("UserId"), out long userId))
            return Unauthorized("انتهت جلسة الدخول.");

        var model = new RequestQuarantineVm { CheckRequestId = checkRequestId };
        await using var connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        await connection.OpenAsync();
        const string accessSql = """
            SELECT TOP 1 request.CheckRequest_Number
            FROM dbo.Im_CheckRequest request
            JOIN dbo.Im_RequestCommittee committee ON committee.ImCheckRequest_ID=request.ID AND committee.User_Deletion_Id IS NULL
            JOIN dbo.CommitteeEmployee employee ON employee.Committee_ID=committee.ID AND employee.Employee_Id=@UserId
              AND employee.OperationType=74 AND employee.User_Deletion_Id IS NULL
            WHERE request.ID=@CheckRequestId
            """;
        await using (var access = new SqlCommand(PlantMobile.Infrastructure.ImportCommitteeEligibility.Filter(accessSql), connection))
        {
            access.Parameters.AddWithValue("@UserId", userId);
            access.Parameters.AddWithValue("@CheckRequestId", checkRequestId);
            model.RequestNumber = Convert.ToString(await access.ExecuteScalarAsync()) ?? string.Empty;
        }
        if (string.IsNullOrWhiteSpace(model.RequestNumber)) return Forbid();

        const string lotsSql = """
            SELECT item.ID, COALESCE(plant.Name_Ar, shortName.ShortName_Ar, N'غير محدد'),
                   lot.ID, COALESCE(lot.Lot_Number, CONVERT(nvarchar(30),lot.ID)),
                   result.ResultName,
                   CAST(CASE WHEN result.ID IS NULL OR result.CommitteeResultType_ID IS NULL THEN 0 ELSE 1 END AS bit),
                   ISNULL(samples.SampleCount,0), ISNULL(samples.IncompleteCount,0),
                   activeStatus.IS_Status, activeStatus.Name_AR, activeStatus.Is_Continue,
                   pendingLab.PendingCount
            FROM dbo.Im_CheckRequest_Items_Lot_Category lot
            JOIN dbo.Im_CheckRequest_Items item ON item.ID=lot.Im_CheckRequest_Items_ID AND item.User_Deletion_Id IS NULL
            JOIN dbo.Im_CheckRequset_Shipping_Method shipping ON shipping.ID=item.Im_CheckRequset_Shipping_Method_ID AND shipping.User_Deletion_Id IS NULL
            LEFT JOIN dbo.Item_ShortName shortName ON shortName.ID=item.Item_ShortName_ID
            LEFT JOIN dbo.Item plant ON plant.ID=shortName.Item_ID
            OUTER APPLY (
                SELECT TOP 1 r.ID,r.CommitteeResultType_ID, rt.Name_Ar AS ResultName
                FROM dbo.Im_CommitteeResult r
                LEFT JOIN dbo.CommitteeResultType rt ON rt.ID=r.CommitteeResultType_ID
                JOIN dbo.Im_RequestCommittee c ON c.ID=r.Committee_ID AND c.CommitteeType_ID=11 AND c.User_Deletion_Id IS NULL
                JOIN dbo.CommitteeEmployee e ON e.Committee_ID=c.ID AND e.Employee_Id=@UserId AND e.ISAdmin=1
                  AND e.OperationType=74 AND e.User_Deletion_Id IS NULL
                WHERE c.ImCheckRequest_ID=@CheckRequestId AND r.User_Deletion_Id IS NULL
                  AND (r.LotData_ID=lot.ID OR (r.LotData_ID IS NULL AND r.Im_Request_Item_Id=item.ID))
                ORDER BY r.ID DESC) result
            OUTER APPLY (
                SELECT COUNT(*) SampleCount,
                       SUM(CASE WHEN sample.SampleSize IS NULL OR sample.SampleSize<=0 OR ISNULL(c.IsFinishedAll,0)=0 THEN 1 ELSE 0 END) IncompleteCount
                FROM dbo.Im_CheckRequest_SampleData sample
                JOIN dbo.Im_RequestCommittee c ON c.ID=sample.Im_RequestCommittee_ID AND c.User_Deletion_Id IS NULL
                WHERE c.ImCheckRequest_ID=@CheckRequestId AND c.CommitteeType_ID IN (11,13)
                  AND sample.LotData_ID=lot.ID AND sample.User_Deletion_Id IS NULL) samples
            OUTER APPLY (
                SELECT COUNT(*) PendingCount
                FROM dbo.Im_CheckRequest_SampleData sample
                JOIN dbo.Im_RequestCommittee c ON c.ID=sample.Im_RequestCommittee_ID AND c.User_Deletion_Id IS NULL
                WHERE c.ImCheckRequest_ID=@CheckRequestId AND c.CommitteeType_ID IN (11,13)
                  AND sample.LotData_ID=lot.ID AND sample.User_Deletion_Id IS NULL
                  AND (sample.IsAccepted IS NULL OR NOT EXISTS
                    (SELECT 1 FROM dbo.A_AttachmentData attachment
                     WHERE attachment.RowId=sample.ID AND attachment.A_AttachmentTableNameId=12
                       AND attachment.User_Deletion_Id IS NULL))) pendingLab
            OUTER APPLY (
                SELECT TOP 1 lr.IS_Status,status.Name_AR,status.Is_Continue
                FROM dbo.Im_CheckRequest_Items_Lot_Result lr
                LEFT JOIN dbo.Im_CheckRequest_Lot_Result_Status status ON status.ID=lr.IS_Status
                WHERE lr.Im_CheckRequest_Items_Lot_Category_ID=lot.ID AND lr.IS_Status_Committee=1
                ORDER BY lr.ID DESC) activeStatus
            WHERE shipping.Im_CheckRequest_ID=@CheckRequestId AND lot.User_Deletion_Id IS NULL
              AND (result.ID IS NOT NULL OR activeStatus.IS_Status IS NOT NULL)
            ORDER BY item.ID,lot.ID
            """;
        await using (var command = new SqlCommand(PlantMobile.Infrastructure.ImportCommitteeEligibility.Filter(lotsSql), connection))
        {
            command.Parameters.AddWithValue("@UserId", userId);
            command.Parameters.AddWithValue("@CheckRequestId", checkRequestId);
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                model.Lots.Add(new RequestQuarantineLotVm
                {
                    RequestItemId=reader.GetInt64(0), PlantName=reader.GetString(1), LotId=reader.GetInt64(2), LotNumber=reader.GetString(3),
                    InspectionComplete=reader.GetBoolean(5), HasWithdrawal=reader.GetInt32(6)>0,
                    InspectionUnavailable=!reader.IsDBNull(4) && reader.GetString(4).Contains("تعذر"),
                    WithdrawalComplete=reader.GetInt32(6)==0 || reader.GetInt32(7)==0,
                    ExistingStatusId=reader.IsDBNull(8)?null:reader.GetInt32(8), ExistingStatusName=reader.IsDBNull(9)?string.Empty:reader.GetString(9),
                    ExistingStatusContinues=reader.IsDBNull(10)?null:reader.GetBoolean(10),
                    LaboratoryResultComplete=reader.GetInt32(11)==0
                });
        }
        const string statusesSql = "SELECT ID,COALESCE(Name_AR,Name_En,CONVERT(nvarchar(20),ID)),Is_Continue FROM dbo.Im_CheckRequest_Lot_Result_Status WHERE ID>0 AND ISNULL(IsActive,1)=1 ORDER BY ID";
        await using (var command = new SqlCommand(statusesSql, connection))
        await using (var reader = await command.ExecuteReaderAsync())
            while (await reader.ReadAsync()) model.Statuses.Add(new RequestQuarantineStatusVm { Id=reader.GetInt32(0), Name=reader.GetString(1), IsContinue=!reader.IsDBNull(2)&&reader.GetBoolean(2) });
        return PartialView("_QuarantineLots", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SaveQuarantineLots(RequestQuarantineSaveVm input)
    {
        if (HttpContext.Session.GetString("UserSession") != "Authenticated" ||
            !short.TryParse(HttpContext.Session.GetString("UserId"), out short userId)) return Unauthorized();
        if (input.Lots.Count==0 || input.Lots.Any(x=>x.LotId<=0 || x.StatusId<=0)) return BadRequest("يجب تحديد موقف الحجر لكل لوط جاهز.");
        await using var connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
        await connection.OpenAsync();
        await using var transaction=(SqlTransaction)await connection.BeginTransactionAsync();
        try
        {
            const string readySql="""
                SELECT COUNT(*) FROM dbo.Im_CheckRequest_Items_Lot_Category lot
                JOIN dbo.Im_CheckRequest_Items item ON item.ID=lot.Im_CheckRequest_Items_ID
                JOIN dbo.Im_CheckRequset_Shipping_Method shipping ON shipping.ID=item.Im_CheckRequset_Shipping_Method_ID
                OUTER APPLY (
                    SELECT TOP 1 rt.Name_Ar AS ResultName
                    FROM dbo.Im_CommitteeResult r
                    JOIN dbo.Im_RequestCommittee c ON c.ID=r.Committee_ID
                    LEFT JOIN dbo.CommitteeResultType rt ON rt.ID=r.CommitteeResultType_ID
                    WHERE c.ImCheckRequest_ID=@CheckRequestId AND c.CommitteeType_ID=11
                      AND c.User_Deletion_Id IS NULL AND r.User_Deletion_Id IS NULL
                      AND (r.LotData_ID=lot.ID OR (r.LotData_ID IS NULL AND r.Im_Request_Item_Id=item.ID))
                    ORDER BY r.ID DESC
                ) latestInspection
                WHERE lot.ID=@LotId AND shipping.Im_CheckRequest_ID=@CheckRequestId AND lot.User_Deletion_Id IS NULL
                  AND ISNULL(latestInspection.ResultName,N'') NOT LIKE N'%تعذر%'
                  AND EXISTS (SELECT 1 FROM dbo.Im_CommitteeResult r JOIN dbo.Im_RequestCommittee c ON c.ID=r.Committee_ID
                              JOIN dbo.CommitteeEmployee e ON e.Committee_ID=c.ID AND e.Employee_Id=@UserId AND e.ISAdmin=1 AND e.User_Deletion_Id IS NULL
                              WHERE c.ImCheckRequest_ID=@CheckRequestId AND c.CommitteeType_ID=11 AND c.User_Deletion_Id IS NULL
                                AND r.User_Deletion_Id IS NULL AND r.CommitteeResultType_ID IS NOT NULL
                                AND (r.LotData_ID=lot.ID OR (r.LotData_ID IS NULL AND r.Im_Request_Item_Id=item.ID)))
                  AND NOT EXISTS (SELECT 1 FROM dbo.Im_CheckRequest_SampleData s JOIN dbo.Im_RequestCommittee c ON c.ID=s.Im_RequestCommittee_ID
                                  WHERE c.ImCheckRequest_ID=@CheckRequestId AND c.CommitteeType_ID IN (11,13) AND c.User_Deletion_Id IS NULL
                                    AND s.LotData_ID=lot.ID AND s.User_Deletion_Id IS NULL
                                    AND (s.SampleSize IS NULL OR s.SampleSize<=0 OR ISNULL(c.IsFinishedAll,0)=0))
                  AND
                  (
                    NOT EXISTS
                    (
                        SELECT 1
                        FROM dbo.Im_CheckRequest_SampleData sample
                        JOIN dbo.Im_RequestCommittee sampleCommittee ON sampleCommittee.ID=sample.Im_RequestCommittee_ID
                          AND sampleCommittee.User_Deletion_Id IS NULL
                        WHERE sampleCommittee.ImCheckRequest_ID=@CheckRequestId
                          AND sampleCommittee.CommitteeType_ID IN (11,13)
                          AND sample.LotData_ID=lot.ID
                          AND sample.User_Deletion_Id IS NULL
                          AND
                          (
                            sample.IsAccepted IS NULL
                            OR NOT EXISTS
                            (
                                SELECT 1 FROM dbo.A_AttachmentData attachment
                                WHERE attachment.RowId=sample.ID
                                  AND attachment.A_AttachmentTableNameId=12
                                  AND attachment.User_Deletion_Id IS NULL
                            )
                          )
                    )
                    OR EXISTS
                    (
                        SELECT 1 FROM dbo.Im_CheckRequest_Lot_Result_Status selectedStatus
                        WHERE selectedStatus.ID=@StatusId
                          AND COALESCE(selectedStatus.Name_AR,N'') LIKE N'%التحفظ%'
                    )
                  )
                """;
            const string saveSql="""
                UPDATE dbo.Im_CheckRequest_Items_Lot_Result SET IS_Status_Committee=0 WHERE Im_CheckRequest_Items_Lot_Category_ID=@LotId;
                INSERT dbo.Im_CheckRequest_Items_Lot_Result(ID,Im_CheckRequest_Items_Lot_Category_ID,Nots,User_Creation_Id,User_Creation_Date,IS_Status,IS_Status_Committee)
                VALUES(NEXT VALUE FOR dbo.Im_CheckRequest_Items_Lot_Result_SEQ,@LotId,@Notes,@UserId,GETDATE(),@StatusId,1);
                """;
            foreach(var lot in input.Lots)
            {
                await using var ready=new SqlCommand(PlantMobile.Infrastructure.ImportCommitteeEligibility.Filter(readySql),connection,transaction);
                ready.Parameters.AddWithValue("@LotId",lot.LotId); ready.Parameters.AddWithValue("@CheckRequestId",input.CheckRequestId); ready.Parameters.AddWithValue("@UserId",userId); ready.Parameters.AddWithValue("@StatusId",lot.StatusId);
                if(Convert.ToInt32(await ready.ExecuteScalarAsync())!=1) throw new InvalidOperationException($"اللوط {lot.LotId} غير جاهز لتسجيل موقف الحجر.");
                await using var save=new SqlCommand(saveSql,connection,transaction);
                save.Parameters.AddWithValue("@LotId",lot.LotId); save.Parameters.AddWithValue("@Notes",(object?)lot.Notes?.Trim()??DBNull.Value); save.Parameters.AddWithValue("@UserId",userId); save.Parameters.AddWithValue("@StatusId",lot.StatusId);
                await save.ExecuteNonQueryAsync();
            }
            await transaction.CommitAsync();
            return Ok(new { message="تم حفظ موقف الحجر بنجاح." });
        }
        catch(Exception ex){ await transaction.RollbackAsync(); return BadRequest(ex.Message); }
    }

    public Task<IActionResult> Index(DateTime? date) => LoadRequests(date, null);

    [HttpGet]
    public Task<IActionResult> Committees(long checkRequestId, DateTime? date) => LoadRequests(date, checkRequestId);

    [HttpGet]
    public async Task<IActionResult> AfterCompletion(long checkRequestId)
    {
        if (HttpContext.Session.GetString("UserSession") != "Authenticated" ||
            !long.TryParse(HttpContext.Session.GetString("UserId"), out long userId))
            return RedirectToAction("Index", "Login");

        string? date = HttpContext.Session.GetString($"ImportRequestDate:{checkRequestId}");
        try
        {
            await using var connection = new SqlConnection(_configuration.GetConnectionString("DBConnection"));
            await connection.OpenAsync();
            const string sql = """
                SELECT CASE WHEN EXISTS (
                    SELECT 1 FROM dbo.Im_RequestCommittee assigned
                    JOIN dbo.CommitteeEmployee employee ON employee.Committee_ID = assigned.ID
                    WHERE assigned.ImCheckRequest_ID = @CheckRequestId
                      AND assigned.User_Deletion_Id IS NULL
                      AND employee.Employee_Id = @UserId AND employee.OperationType = 74
                      AND employee.User_Deletion_Id IS NULL
                ) THEN CASE WHEN EXISTS (
                    SELECT 1 FROM dbo.Im_RequestCommittee committee
                    WHERE committee.ImCheckRequest_ID = @CheckRequestId
                      AND committee.User_Deletion_Id IS NULL AND committee.Is_Cancel IS NULL
                      AND (
                        (ISNULL(committee.Status, 0) = 0 AND ISNULL(committee.IsFinishedAll, 0) = 0)
                        OR (committee.CommitteeType_ID = 13 AND EXISTS (
                            SELECT 1 FROM dbo.Im_CheckRequest_SampleData sample
                            WHERE sample.Im_RequestCommittee_ID = committee.ID
                              AND sample.User_Deletion_Id IS NULL AND sample.Sample_BarCode IS NOT NULL
                              AND (sample.IsAccepted IS NULL OR NOT EXISTS (
                                  SELECT 1 FROM dbo.A_AttachmentData attachment
                                  WHERE attachment.RowId = sample.ID AND attachment.A_AttachmentTableNameId = 12
                                    AND attachment.User_Deletion_Id IS NULL))
                        ))
                      )
                ) THEN 1 ELSE 0 END ELSE -1 END
                """;
            await using var command = new SqlCommand(PlantMobile.Infrastructure.ImportCommitteeEligibility.Filter(sql), connection);
            command.Parameters.AddWithValue("@CheckRequestId", checkRequestId);
            command.Parameters.AddWithValue("@UserId", userId);
            int remaining = Convert.ToInt32(await command.ExecuteScalarAsync());
            if (remaining < 0) return Forbid();
            if (remaining == 0) return RedirectToAction(nameof(Index), new { date });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to check remaining committees for request {CheckRequestId}", checkRequestId);
        }

        return RedirectToAction(nameof(Committees), new { checkRequestId, date });
    }

    private async Task<IActionResult> LoadRequests(DateTime? date, long? checkRequestId)
    {
        if (HttpContext.Session.GetString("UserSession") != "Authenticated" ||
            !long.TryParse(HttpContext.Session.GetString("UserId"), out long userId) ||
            !long.TryParse(HttpContext.Session.GetString("EmployeeId"), out long employeeId))
            return RedirectToAction("Index", "Login");

        DateTime selectedDate = (date ?? DateTime.Today).Date;
        if (checkRequestId.HasValue)
            HttpContext.Session.SetString($"ImportRequestDate:{checkRequestId.Value}", selectedDate.ToString("yyyy-MM-dd"));
        var requests = new List<Ex_Im_CheckRequest_GetAllByUser_DateVM>();
        double queryMs = 0;

        try
        {
            var queryStarted = System.Diagnostics.Stopwatch.GetTimestamp();
            var rows = await new ImportRequestReader(_dbFactory).LoadAsync(userId, selectedDate, checkRequestId, HttpContext.RequestAborted);
            foreach (var row in rows)
            {
                bool status = (row.Status ?? false);
                bool finished = (row.IsFinishedAll ?? false);
                bool started = (row.Is_Start_Android ?? false);
                bool canceled = (row.Is_Cancel != null);
                bool isAdmin = row.ISAdmin;
                int resultCount = row.ResultCount;
                int confirmCount = row.ConfirmCount;
                int sampleCount = row.SampleCount;
                int sampleConfirmCount = row.SampleConfirmCount;
                bool pendingSampleFinalResult = row.HasPendingSampleFinalResult;
                string key = isAdmin
                    ? canceled ? "canceled"
                      : row.CommitteeType_ID == 13 && pendingSampleFinalResult ? "working"
                      : (status || finished) ? "completed" : started || (row.User_Updation_Date != null) ? "working" : "new"
                    : canceled ? "canceled"
                    : row.CommitteeType_ID == 13
                        ? sampleCount > 0 && sampleConfirmCount >= sampleCount ? "completed" : sampleConfirmCount > 0 ? "working" : "new"
                        : resultCount > 0 && confirmCount >= resultCount ? "completed" : confirmCount > 0 ? "working" : "new";
                string name = key switch { "completed" => "تم الانتهاء", "working" => "جاري العمل", "canceled" => "ملغي", _ => "جديد" };
                bool quarantineComplete = row.RequestQuarantineComplete;
                byte committeeType = row.CommitteeType_ID;
                bool requiresCompletion = !canceled && (isAdmin
                    ? committeeType switch
                    {
                        11 => !row.InspectionComplete && !finished,
                        13 => !row.WithdrawalComplete || !finished,
                        _ => !status && !finished
                    }
                    : key is not ("completed" or "canceled"));
                if (quarantineComplete && !canceled && !requiresCompletion)
                {
                    key = "completed";
                    name = "تم الانتهاء";
                }

                requests.Add(new Ex_Im_CheckRequest_GetAllByUser_DateVM
                {
                    Row_Num = Convert.ToInt32(row.RowNum),
                    IsExport = 2,
                    CheckRequest_Number = row.CheckRequest_Number ?? string.Empty,
                    CompanyName = row.CompanyName ?? "غير محدد",
                    checkRequest_Id = row.CheckRequestId,
                    Committee_ID = row.CommitteeId,
                    Committee_Type_Name = $"{row.CommitteeTypeName} {row.OperationTypeName}".Trim(),
                    Committee_Type_Id = row.CommitteeType_ID,
                    RequestCommittee_Status = name,
                    RequestCommittee_Status_Id = status ? (byte)1 : (byte)0,
                    WorkStatusKey = key,
                    WorkStatusName = name,
                    IsReadOnly = key is "completed" or "canceled" || (isAdmin && row.CommitteeType_ID == 13 && (status || finished)),
                    IsAdmin = isAdmin,
                    InspectionComplete = row.InspectionComplete,
                    RequestQuarantineComplete = quarantineComplete,
                    RequiresCommitteeCompletion = requiresCompletion,
                    HasPendingQuarantine = row.HasPendingQuarantine,
                    WithdrawalComplete = row.WithdrawalComplete,
                    CanShowLaboratoryResult = row.CanShowLaboratoryResult,
                    PendingLabBarcodes = row.PendingLabBarcodes ?? string.Empty,
                    CommitteeMembers = row.CommitteeMembers == null ? string.Empty : row.CommitteeMembers ?? string.Empty
                    ,SampleAttachmentUploaded = row.SampleAttachmentUploaded
                    ,HasPendingSampleFinalResult = pendingSampleFinalResult
                    ,HasPendingLabResult = row.HasPendingLabResult
                    ,SampleBarcodes = row.SampleBarcodes == null ? string.Empty : row.SampleBarcodes ?? string.Empty
                });
            }

            queryMs = System.Diagnostics.Stopwatch.GetElapsedTime(queryStarted).TotalMilliseconds;
            if (queryMs >= 1000)
                _logger.LogWarning("Import requests timing: CommitteePage={CommitteePage}; EfReadAndMapMs={QueryMs}; Rows={Rows}; TraceId={TraceId}",
                    checkRequestId.HasValue, queryMs, requests.Count, HttpContext.TraceIdentifier);
            ViewBag.SelectedDate = selectedDate.ToString("yyyy-MM-dd");
            ViewBag.UserId = userId;
            ViewBag.EmployeeId = employeeId;
            ViewBag.UserName = HttpContext.Session.GetString("UserName");
        }
        catch (OperationCanceledException) when (HttpContext.RequestAborted.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Failed to load import requests for UserId={UserId}, EmployeeId={EmployeeId}, Date={SelectedDate}; TraceId={TraceId}",
                userId, employeeId, selectedDate, HttpContext.TraceIdentifier);
            ViewBag.PageError = $"تعذر تحميل طلبات الوارد: {ex.Message}";
            ViewBag.SelectedDate = selectedDate.ToString("yyyy-MM-dd");
        }


        if (checkRequestId.HasValue && requests.Count == 0 && ViewBag.PageError == null)
            return RedirectToAction(nameof(Index), new { date = selectedDate.ToString("yyyy-MM-dd") });

        ViewBag.CheckRequestId = checkRequestId;
        return View("Index", requests);
    }

}

public class RequestQuarantineVm { public long CheckRequestId {get;set;} public string RequestNumber {get;set;}=string.Empty; public List<RequestQuarantineLotVm> Lots {get;set;}=new(); public List<RequestQuarantineStatusVm> Statuses {get;set;}=new(); }
public class RequestQuarantineLotVm { public long RequestItemId {get;set;} public string PlantName {get;set;}=string.Empty; public long LotId {get;set;} public string LotNumber {get;set;}=string.Empty; public bool InspectionComplete {get;set;} public bool HasWithdrawal {get;set;} public bool WithdrawalComplete {get;set;} public bool LaboratoryResultComplete {get;set;} public int? ExistingStatusId {get;set;} public string ExistingStatusName {get;set;}=string.Empty; public bool? ExistingStatusContinues {get;set;} public bool InspectionUnavailable {get;set;} public bool IsReady=>!InspectionUnavailable&&InspectionComplete&&WithdrawalComplete; }
public class RequestQuarantineStatusVm { public int Id {get;set;} public string Name {get;set;}=string.Empty; public bool IsContinue {get;set;} }
public class RequestQuarantineSaveVm { public long CheckRequestId {get;set;} public List<RequestQuarantineSaveLotVm> Lots {get;set;}=new(); }
public class RequestQuarantineSaveLotVm { public long LotId {get;set;} public int StatusId {get;set;} public string? Notes {get;set;} }
