using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ViewModels;

namespace PlantMobile.Controllers;

/// <summary>
/// شاشة نتيجة فحص اللجنة لطلبات الحجر الزراعي.
/// القوائم تُقرأ من جداول السليوشن الموجودة؛ الحفظ في dbo.Im_Committee_Inspection_Result.
/// </summary>
[Route("Committees/ImCommittee_InspectionResult")]
public sealed class ImCommittee_InspectionResultController : Controller
{
    private readonly string _connectionString;
    private readonly ILogger<ImCommittee_InspectionResultController> _logger;

    public ImCommittee_InspectionResultController(
        IConfiguration configuration,
        ILogger<ImCommittee_InspectionResultController> logger)
    {
        _connectionString = configuration.GetConnectionString("DBConnection")
            ?? throw new InvalidOperationException("DBConnection is not configured.");
        _logger = logger;
    }

    [HttpGet("")]
    [HttpGet("Index")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        if (!TryReadUser(out var userId))
            return RedirectToAction("Index", "Login");

        var model = new CommitteeInspectionResultPageVm
        {
            OutletName = await ReadOutletNameAsync(userId, cancellationToken).ConfigureAwait(false),
            // الإجراء: من جدول السليوشن Im_CheckRequest_Lot_Result_Status
            Procedures = await ReadOptionsAsync(
                """
                SELECT CAST(ID AS bigint) AS Id, COALESCE(Name_AR, N'') AS Name
                FROM dbo.Im_CheckRequest_Lot_Result_Status
                ORDER BY ID;
                """,
                new[]
                {
                    "الرسالة مطابقة لتشريعات الحجر الزراعي",
                    "الرسالة غير مطابقة لتشريعات الحجر الزراعي",
                    "الرسالة تحول الى التبخير",
                    "الرسالة تحول الي التطهير",
                    "ايقاف المعاملة",
                    "تم جميع الاجراءات الحجرية",
                    "الرسالة فى انتظار الصرف",
                    "الرسالة نقل وتخزين تحت التحفظ",
                    "الرسالة إعدام",
                    "الرسالة تحول الى إعادة التصدير",
                    "تم جميع الاجراءات الحجرية علي الرسالة"
                }, cancellationToken).ConfigureAwait(false),
            CountryTypes = CommitteeInspectionResultDefaults.CountryTypes,
            AidTypes = CommitteeInspectionResultDefaults.AidTypes,
            TransportMeans = await ReadOptionsAsync(
                """
                SELECT CAST(ID AS bigint) AS Id, COALESCE(Ar_Name, N'') AS Name
                FROM dbo.Transport_Mean
                WHERE User_Deletion_Id IS NULL AND IsActive = 1
                ORDER BY Ar_Name;
                """,
                new[] { "بري", "بحري", "جوي" }, cancellationToken).ConfigureAwait(false),
            Items = await ReadOptionsAsync(
                """
                SELECT CAST(ID AS bigint) AS Id, COALESCE(Name_Ar, N'') AS Name
                FROM dbo.Item
                WHERE User_Deletion_Id IS NULL AND Name_Ar LIKE N'%[أ-ي]%'
                ORDER BY Name_Ar;
                """,
                Array.Empty<string>(), cancellationToken).ConfigureAwait(false),
            Countries = await ReadOptionsAsync(
                """
                SELECT CAST(ID AS bigint) AS Id, COALESCE(Ar_Name, N'') AS Name
                FROM dbo.Country
                WHERE User_Deletion_Id IS NULL AND IsActive = 1
                ORDER BY Ar_Name;
                """,
                new[] { "الجزائر" }, cancellationToken).ConfigureAwait(false),
            InspectionResults = await ReadOptionsAsync(
                """
                SELECT CAST(ID AS bigint) AS Id, COALESCE(Name_Ar, N'') AS Name
                FROM dbo.CommitteeResultType
                WHERE User_Deletion_Id IS NULL AND IsActive = 1
                ORDER BY Name_Ar;
                """,
                new[] { "سليم", "غير سليم" }, cancellationToken).ConfigureAwait(false),
            InfectionTypes = await ReadOptionsAsync(
                """
                SELECT CAST(ID AS bigint) AS Id, COALESCE(Name_Ar, N'') AS Name
                FROM dbo.Item
                WHERE User_Deletion_Id IS NULL
                ORDER BY Name_Ar;
                """,
                Array.Empty<string>(), cancellationToken).ConfigureAwait(false),
            Requests = await ReadOptionsAsync(
                """
                SELECT TOP (200) CAST(ID AS bigint) AS Id, COALESCE(CheckRequest_Number, N'') AS Name
                FROM dbo.Im_CheckRequest
                WHERE User_Deletion_Id IS NULL
                ORDER BY ID DESC;
                """,
                Array.Empty<string>(), cancellationToken).ConfigureAwait(false)
        };

        return View(model);
    }

    [HttpPost("Save")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save([FromForm] CommitteeInspectionResultSaveVm input, CancellationToken cancellationToken)
    {
        if (!TryReadUser(out var userId))
            return Json(new { ok = false, message = "انتهت جلسة المستخدم. سجل الدخول مرة أخرى." });

        var requestNumber = (input.RequestNumber ?? string.Empty).Trim();
        if (requestNumber.Length == 0)
            return Json(new { ok = false, message = "أدخل رقم طلب الفحص." });

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await using var transaction = (SqlTransaction)await connection.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            const string findRequestSql = """
                SELECT TOP (1) ID, COALESCE(Outlet_ID, 0)
                FROM dbo.Im_CheckRequest
                WHERE CheckRequest_Number = @Number;
                """;
            long requestId = 0;
            long outletId = 0;
            await using (var command = new SqlCommand(findRequestSql, connection, transaction))
            {
                command.Parameters.AddWithValue("@Number", requestNumber);
                await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
                if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                {
                    requestId = Convert.ToInt64(reader.GetValue(0));
                    outletId = Convert.ToInt64(reader.GetValue(1));
                }
            }

            if (requestId == 0 && long.TryParse(requestNumber, out var parsedId))
            {
                const string findByIdSql = """
                    SELECT TOP (1) ID, COALESCE(Outlet_ID, 0)
                    FROM dbo.Im_CheckRequest
                    WHERE ID = @Id;
                    """;
                await using var command = new SqlCommand(findByIdSql, connection, transaction);
                command.Parameters.AddWithValue("@Id", parsedId);
                await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
                if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                {
                    requestId = Convert.ToInt64(reader.GetValue(0));
                    outletId = Convert.ToInt64(reader.GetValue(1));
                }
            }

            if (requestId == 0)
                return Json(new { ok = false, message = "رقم طلب الفحص غير موجود." });

            if (outletId == 0)
                outletId = await ReadUserOutletIdAsync(userId, connection, transaction, cancellationToken).ConfigureAwait(false);

            var now = DateTime.Now;
            const string saveSql = """
                DECLARE @Id bigint = NEXT VALUE FOR dbo.Im_Committee_Inspection_Result_SEQ;
                INSERT dbo.Im_Committee_Inspection_Result
                 (ID, Serial, Im_CheckRequest_ID, CheckRequest_Number, Message, CountryType_ID, Source,
                  Procedure_ID, Transport_Mean_ID, Item_ID, Origin_Country_ID, Quantity_Ton,
                  Inspection_Result_ID, Infection_Type_Item_ID, AidType_ID, Notes, Outlet_ID,
                  Document_Date, User_Creation_Id, User_Creation_Date)
                VALUES
                 (@Id, @Serial, @RequestId, @RequestNumber, @Message, @CountryTypeId, @Source,
                  @ProcedureId, @TransportMeanId, @ItemId, @OriginCountryId, @QuantityTon,
                  @InspectionResultId, @InfectionTypeId, @AidTypeId, @Notes, @OutletId,
                  @Now, @UserId, @Now);
                SELECT @Id;
                """;
            long formId;
            await using (var command = new SqlCommand(saveSql, connection, transaction))
            {
                Add(command, "@Serial", input.Serial);
                Add(command, "@RequestId", requestId);
                Add(command, "@RequestNumber", requestNumber);
                Add(command, "@Message", string.IsNullOrWhiteSpace(input.Message) ? "صادر" : input.Message);
                Add(command, "@CountryTypeId", input.CountryTypeId);
                Add(command, "@Source", input.Source ?? string.Empty);
                Add(command, "@ProcedureId", input.ProcedureId);
                Add(command, "@TransportMeanId", input.TransportMeanId);
                Add(command, "@ItemId", input.ItemId);
                Add(command, "@OriginCountryId", input.OriginCountryId);
                Add(command, "@QuantityTon", input.QuantityTon);
                Add(command, "@InspectionResultId", input.InspectionResultId);
                Add(command, "@InfectionTypeId", input.InfectionTypeId);
                Add(command, "@AidTypeId", input.AidTypeId);
                Add(command, "@Notes", input.Notes ?? string.Empty);
                Add(command, "@OutletId", outletId);
                Add(command, "@Now", now);
                Add(command, "@UserId", userId);
                formId = Convert.ToInt64(await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false));
            }

            const string logSql = """
                DECLARE @LogId bigint = NEXT VALUE FOR dbo.Table_Action_Log_CheckRequest_SEQ;
                INSERT dbo.Table_Action_Log_CheckRequest
                 (ID, ID_Table_Action, Im_CheckRequest_ID, ID_TableActionValue,
                  User_Creation_Id, User_Creation_Date, NOTS, User_Type_ID, Type_log_ID)
                VALUES
                 (@LogId, 15, @RequestId, @FormId, @UserId, @Now,
                  N'حفظ شاشة نتيجة فحص اللجنة', 127, 135);
                """;
            await using (var command = new SqlCommand(logSql, connection, transaction))
            {
                Add(command, "@RequestId", requestId);
                Add(command, "@FormId", formId);
                Add(command, "@UserId", userId);
                Add(command, "@Now", now);
                await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
            }

            await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
            return Json(new { ok = true, message = "تم الحفظ", id = formId });
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
            _logger.LogError(ex, "Failed to save committee inspection result for request {Request}", requestNumber);
            return Json(new
            {
                ok = false,
                message = "تعذر الحفظ. تأكد من تنفيذ سكربت جدول نتيجة فحص اللجنة.",
                details = ex.GetBaseException().Message
            });
        }
    }

    private async Task<List<CommitteeInspectionResultOptionVm>> ReadOptionsAsync(
        string sql, IReadOnlyList<string> fallback, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
            await using var command = new SqlCommand(sql, connection) { CommandTimeout = 30 };
            var options = new List<CommitteeInspectionResultOptionVm>();
            await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                options.Add(new CommitteeInspectionResultOptionVm
                {
                    Id = Convert.ToInt64(reader.GetValue(0)),
                    Name = reader.IsDBNull(1) ? string.Empty : Convert.ToString(reader.GetValue(1)) ?? string.Empty
                });
            }
            if (options.Count > 0)
                return options;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Falling back to local options for committee inspection result screen.");
        }

        return fallback.Select((name, index) => new CommitteeInspectionResultOptionVm
        {
            Id = index + 1,
            Name = name
        }).ToList();
    }

    private async Task<string> ReadOutletNameAsync(short userId, CancellationToken cancellationToken)
    {
        try
        {
            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
            const string sql = """
                SELECT TOP (1) COALESCE(outlet.Ar_Name, N'')
                FROM dbPrivilage.dbo.PR_User [user]
                LEFT JOIN dbo.Outlet outlet ON outlet.ID_HR = [user].Outlet_ID
                WHERE [user].Id = @UserId;
                """;
            await using var command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@UserId", userId);
            var value = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
            return value is null or DBNull ? "المكتب" : Convert.ToString(value) ?? "المكتب";
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Could not read outlet name for user {UserId}", userId);
            return "المكتب";
        }
    }

    private static async Task<long> ReadUserOutletIdAsync(
        short userId, SqlConnection connection, SqlTransaction transaction, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT TOP (1) COALESCE([user].Outlet_ID, 0)
            FROM dbPrivilage.dbo.PR_User [user]
            WHERE [user].Id = @UserId;
            """;
        await using var command = new SqlCommand(sql, connection, transaction);
        command.Parameters.AddWithValue("@UserId", userId);
        var value = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return value is null or DBNull ? 0 : Convert.ToInt64(value);
    }

    private bool TryReadUser(out short userId)
    {
        userId = 0;
        return HttpContext.Session.GetString("UserSession") == "Authenticated"
            && short.TryParse(HttpContext.Session.GetString("UserId"), out userId);
    }

    private static void Add(SqlCommand command, string name, object? value) =>
        command.Parameters.AddWithValue(name, value ?? DBNull.Value);
}
