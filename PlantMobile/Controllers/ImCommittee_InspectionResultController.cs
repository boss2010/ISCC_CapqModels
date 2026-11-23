using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using PlantMobile.Infrastructure;
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

    [HttpGet("List")]
    public async Task<IActionResult> List([FromQuery] CommitteeInspectionResultFilterVm filter, CancellationToken cancellationToken)
    {
        if (!TryReadUser(out _))
            return RedirectToAction("Index", "Login");

        NormalizeFilter(filter);
        var model = new CommitteeInspectionResultListPageVm { Filter = filter };
        await LoadListOptionsAsync(model, cancellationToken).ConfigureAwait(false);

        var result = await QueryRowsAsync(filter, cancellationToken, takeAll: false).ConfigureAwait(false);
        model.Rows = result.Rows;
        model.Total = result.Total;
        return View(model);
    }

    [HttpGet("ExportExcel")]
    public async Task<IActionResult> ExportExcel(
        [FromQuery] CommitteeInspectionResultFilterVm filter,
        [FromQuery] string? columns,
        CancellationToken cancellationToken)
    {
        if (!TryReadUser(out _))
            return RedirectToAction("Index", "Login");

        NormalizeFilter(filter);
        var result = await QueryRowsAsync(filter, cancellationToken, takeAll: true).ConfigureAwait(false);

        var selectedKeys = (columns ?? string.Empty)
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var selected = CommitteeInspectionResultExportColumns.All
            .Where(c => selectedKeys.Count == 0 || selectedKeys.Contains(c.Key))
            .Where(c => ExportValues.ContainsKey(c.Key))
            .ToArray();
        if (selected.Length == 0)
            selected = CommitteeInspectionResultExportColumns.All;

        var workbook = new SimpleXlsxWriter("نتيجة فحص اللجنة");
        workbook.SetColumnWidths(selected.Select(c => ExportValues[c.Key].Width));
        workbook.AddHeader(selected.Select(c => c.Header).ToArray());
        foreach (var row in result.Rows)
            workbook.AddRow(selected.Select(c => ExportValues[c.Key].Value(row)).ToArray());

        var fileName = $"Committee_Inspection_Result_{DateTime.Now:yyyyMMdd_HHmm}.xlsx";
        return File(
            workbook.Build(),
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            fileName);
    }

    [HttpGet("ExportCsv")]
    public async Task<IActionResult> ExportCsv(
        [FromQuery] CommitteeInspectionResultFilterVm filter,
        [FromQuery] string? columns,
        CancellationToken cancellationToken)
    {
        if (!TryReadUser(out _))
            return RedirectToAction("Index", "Login");

        NormalizeFilter(filter);
        var result = await QueryRowsAsync(filter, cancellationToken, takeAll: true).ConfigureAwait(false);

        var selectedKeys = (columns ?? string.Empty)
            .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var selected = CommitteeInspectionResultExportColumns.All
            .Where(c => selectedKeys.Count == 0 || selectedKeys.Contains(c.Key))
            .Where(c => ExportValues.ContainsKey(c.Key))
            .ToArray();
        if (selected.Length == 0)
            selected = CommitteeInspectionResultExportColumns.All;

        // BOM عشان Excel يقرأ العربي صح عند فتح ملف CSV
        var csv = new StringBuilder("\uFEFF");
        csv.AppendLine(string.Join(',', selected.Select(c => CsvEscape(c.Header))));
        foreach (var row in result.Rows)
        {
            csv.AppendLine(string.Join(',', selected.Select(c =>
                CsvEscape(CellText(ExportValues[c.Key].Value(row))))));
        }

        var fileName = $"Committee_Inspection_Result_{DateTime.Now:yyyyMMdd_HHmm}.csv";
        return File(new UTF8Encoding(false).GetBytes(csv.ToString()), "text/csv; charset=utf-8", fileName);
    }

    private static string CellText(XlsxCell cell) =>
        cell.Text ?? cell.Number?.ToString(CultureInfo.InvariantCulture) ?? string.Empty;

    private static string CsvEscape(string? value)
    {
        if (string.IsNullOrEmpty(value))
            return string.Empty;

        return value.IndexOfAny(new[] { ',', '"', '\r', '\n' }) >= 0
            ? "\"" + value.Replace("\"", "\"\"") + "\""
            : value;
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

    private static readonly Dictionary<string, (double Width, Func<CommitteeInspectionResultRowVm, XlsxCell> Value)> ExportValues =
        new(StringComparer.OrdinalIgnoreCase)
        {
            ["serial"] = (9, r => XlsxCell.OfNumber(r.Serial)),
            ["requestNumber"] = (18, r => XlsxCell.OfText(r.CheckRequestNumber)),
            ["message"] = (9, r => XlsxCell.OfText(r.Message)),
            ["countryType"] = (18, r => XlsxCell.OfText(r.CountryTypeName)),
            ["source"] = (12, r => XlsxCell.OfText(r.Source)),
            ["procedure"] = (34, r => XlsxCell.OfText(r.ProcedureName)),
            ["transportMean"] = (11, r => XlsxCell.OfText(r.TransportMeanName)),
            ["item"] = (22, r => XlsxCell.OfText(r.ItemName)),
            ["originCountry"] = (16, r => XlsxCell.OfText(r.OriginCountryName)),
            ["quantityTon"] = (13, r => XlsxCell.OfNumber((double?)r.QuantityTon)),
            ["inspectionResult"] = (14, r => XlsxCell.OfText(r.InspectionResultName)),
            ["infectionType"] = (20, r => XlsxCell.OfText(r.InfectionTypeName)),
            ["aidType"] = (12, r => XlsxCell.OfText(r.AidTypeName)),
            ["notes"] = (32, r => XlsxCell.OfText(r.Notes)),
            ["outlet"] = (16, r => XlsxCell.OfText(r.OutletName)),
            ["documentDate"] = (12, r => XlsxCell.OfText(r.DocumentDate?.ToString("dd/MM/yyyy")))
        };

    private static void NormalizeFilter(CommitteeInspectionResultFilterVm filter)
    {
        filter.RequestNumber = string.IsNullOrWhiteSpace(filter.RequestNumber) ? null : filter.RequestNumber.Trim();
        filter.Message = string.IsNullOrWhiteSpace(filter.Message) ? null : filter.Message.Trim();
        filter.Search = string.IsNullOrWhiteSpace(filter.Search) ? null : filter.Search.Trim();
        if (filter.Page < 1)
            filter.Page = 1;
        if (filter.PageSize is not (25 or 50 or 100 or 200))
            filter.PageSize = 50;
    }

    private async Task LoadListOptionsAsync(CommitteeInspectionResultListPageVm model, CancellationToken cancellationToken)
    {
        model.CountryTypes = CommitteeInspectionResultDefaults.CountryTypes;
        model.AidTypes = CommitteeInspectionResultDefaults.AidTypes;
        model.Outlets = await ReadOptionsAsync(
            """
            SELECT CAST(ID AS bigint) AS Id, COALESCE(Ar_Name, N'') AS Name
            FROM dbo.Outlet
            ORDER BY Ar_Name;
            """,
            Array.Empty<string>(), cancellationToken).ConfigureAwait(false);
        model.Procedures = await ReadOptionsAsync(
            """
            SELECT CAST(ID AS bigint) AS Id, COALESCE(Name_AR, N'') AS Name
            FROM dbo.Im_CheckRequest_Lot_Result_Status
            ORDER BY ID;
            """,
            Array.Empty<string>(), cancellationToken).ConfigureAwait(false);
        model.InspectionResults = await ReadOptionsAsync(
            """
            SELECT CAST(ID AS bigint) AS Id, COALESCE(Name_Ar, N'') AS Name
            FROM dbo.CommitteeResultType
            WHERE User_Deletion_Id IS NULL AND IsActive = 1
            ORDER BY Name_Ar;
            """,
            Array.Empty<string>(), cancellationToken).ConfigureAwait(false);
        model.Items = await ReadOptionsAsync(
            """
            SELECT CAST(ID AS bigint) AS Id, COALESCE(Name_Ar, N'') AS Name
            FROM dbo.Item
            WHERE User_Deletion_Id IS NULL AND Name_Ar LIKE N'%[أ-ي]%'
            ORDER BY Name_Ar;
            """,
            Array.Empty<string>(), cancellationToken).ConfigureAwait(false);
        model.Countries = await ReadOptionsAsync(
            """
            SELECT CAST(ID AS bigint) AS Id, COALESCE(Ar_Name, N'') AS Name
            FROM dbo.Country
            WHERE User_Deletion_Id IS NULL AND IsActive = 1
            ORDER BY Ar_Name;
            """,
            Array.Empty<string>(), cancellationToken).ConfigureAwait(false);
        model.InfectionTypes = await ReadOptionsAsync(
            """
            SELECT CAST(ID AS bigint) AS Id, COALESCE(Name_Ar, N'') AS Name
            FROM dbo.Item
            WHERE User_Deletion_Id IS NULL
            ORDER BY Name_Ar;
            """,
            Array.Empty<string>(), cancellationToken).ConfigureAwait(false);
    }

    private async Task<(List<CommitteeInspectionResultRowVm> Rows, int Total)> QueryRowsAsync(
        CommitteeInspectionResultFilterVm filter, CancellationToken cancellationToken, bool takeAll)
    {
        var (whereSql, parameters) = BuildWhere(filter);

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

        int total;
        await using (var countCommand = new SqlCommand(
            "SELECT COUNT_BIG(1) FROM dbo.Im_Committee_Inspection_Result r " + whereSql, connection)
        {
            CommandTimeout = 60
        })
        {
            AddParameters(countCommand, parameters);
            var value = await countCommand.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
            total = value is null or DBNull
                ? 0
                : (int)Math.Min(int.MaxValue, Convert.ToInt64(value, CultureInfo.InvariantCulture));
        }

        if (total == 0)
            return (new List<CommitteeInspectionResultRowVm>(), 0);

        var sql = new StringBuilder();
        sql.AppendLine("""
            SELECT r.ID, r.Serial, r.CheckRequest_Number, r.Message, r.CountryType_ID, r.Source,
                   r.Quantity_Ton, r.Notes, r.Document_Date, r.User_Creation_Date,
                   p.Name_AR, t.Ar_Name, i.Name_Ar, c.Ar_Name, cr.Name_Ar, inf.Name_Ar,
                   o.Ar_Name, u.FullName, r.AidType_ID
            FROM dbo.Im_Committee_Inspection_Result r
            LEFT JOIN dbo.Im_CheckRequest_Lot_Result_Status p ON p.ID = r.Procedure_ID
            LEFT JOIN dbo.Transport_Mean t ON t.ID = r.Transport_Mean_ID
            LEFT JOIN dbo.Item i ON i.ID = r.Item_ID
            LEFT JOIN dbo.Country c ON c.ID = r.Origin_Country_ID
            LEFT JOIN dbo.CommitteeResultType cr ON cr.ID = r.Inspection_Result_ID
            LEFT JOIN dbo.Item inf ON inf.ID = r.Infection_Type_Item_ID
            LEFT JOIN dbo.Outlet o ON o.ID = r.Outlet_ID
            LEFT JOIN dbPrivilage.dbo.PR_User u ON u.Id = r.User_Creation_Id
            """);
        sql.Append(whereSql);
        sql.Append(" ORDER BY r.Document_Date DESC, r.ID DESC");

        if (takeAll)
        {
            sql.Append(" OFFSET 0 ROWS FETCH NEXT 100000 ROWS ONLY");
        }
        else
        {
            sql.Append(" OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY");
        }

        var rows = new List<CommitteeInspectionResultRowVm>();
        await using var command = new SqlCommand(sql.ToString(), connection) { CommandTimeout = 120 };
        AddParameters(command, parameters);
        if (!takeAll)
        {
            command.Parameters.AddWithValue("@Offset", (filter.Page - 1) * filter.PageSize);
            command.Parameters.AddWithValue("@PageSize", filter.PageSize);
        }

        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            var countryTypeId = Lng(reader, 4);
            var aidTypeId = Lng(reader, 18);
            rows.Add(new CommitteeInspectionResultRowVm
            {
                Id = Lng(reader, 0) ?? 0,
                Serial = Lng(reader, 1),
                CheckRequestNumber = Str(reader, 2),
                Message = Str(reader, 3),
                CountryTypeName = CommitteeInspectionResultDefaults.CountryTypes
                    .FirstOrDefault(x => x.Id == countryTypeId)?.Name ?? string.Empty,
                Source = Str(reader, 5),
                QuantityTon = Dec(reader, 6),
                Notes = Str(reader, 7),
                DocumentDate = Dt(reader, 8),
                CreationDate = Dt(reader, 9),
                ProcedureName = Str(reader, 10),
                TransportMeanName = Str(reader, 11),
                ItemName = Str(reader, 12),
                OriginCountryName = Str(reader, 13),
                InspectionResultName = Str(reader, 14),
                InfectionTypeName = Str(reader, 15),
                OutletName = Str(reader, 16),
                CreatedBy = Str(reader, 17),
                AidTypeName = CommitteeInspectionResultDefaults.AidTypes
                    .FirstOrDefault(x => x.Id == aidTypeId)?.Name ?? string.Empty
            });
        }

        return (rows, total);
    }

    private static (string Sql, List<(string Name, object? Value)> Parameters) BuildWhere(
        CommitteeInspectionResultFilterVm filter)
    {
        var sql = new StringBuilder(" WHERE 1 = 1");
        var parameters = new List<(string Name, object? Value)>();

        if (!string.IsNullOrWhiteSpace(filter.RequestNumber))
        {
            sql.Append(" AND r.CheckRequest_Number LIKE @RequestNumber");
            parameters.Add(("@RequestNumber", LikePattern(filter.RequestNumber)));
        }
        if (filter.DateFrom.HasValue)
        {
            sql.Append(" AND r.Document_Date >= @DateFrom");
            parameters.Add(("@DateFrom", filter.DateFrom.Value.Date));
        }
        if (filter.DateTo.HasValue)
        {
            sql.Append(" AND r.Document_Date < @DateTo");
            parameters.Add(("@DateTo", filter.DateTo.Value.Date.AddDays(1)));
        }
        if (filter.OutletId.HasValue)
        {
            sql.Append(" AND r.Outlet_ID = @OutletId");
            parameters.Add(("@OutletId", filter.OutletId.Value));
        }
        if (filter.CountryTypeId.HasValue)
        {
            sql.Append(" AND r.CountryType_ID = @CountryTypeId");
            parameters.Add(("@CountryTypeId", filter.CountryTypeId.Value));
        }
        if (filter.ProcedureId.HasValue)
        {
            sql.Append(" AND r.Procedure_ID = @ProcedureId");
            parameters.Add(("@ProcedureId", filter.ProcedureId.Value));
        }
        if (filter.InspectionResultId.HasValue)
        {
            sql.Append(" AND r.Inspection_Result_ID = @InspectionResultId");
            parameters.Add(("@InspectionResultId", filter.InspectionResultId.Value));
        }
        if (filter.ItemId.HasValue)
        {
            sql.Append(" AND r.Item_ID = @ItemId");
            parameters.Add(("@ItemId", filter.ItemId.Value));
        }
        if (filter.OriginCountryId.HasValue)
        {
            sql.Append(" AND r.Origin_Country_ID = @OriginCountryId");
            parameters.Add(("@OriginCountryId", filter.OriginCountryId.Value));
        }
        if (filter.InfectionTypeId.HasValue)
        {
            sql.Append(" AND r.Infection_Type_Item_ID = @InfectionTypeId");
            parameters.Add(("@InfectionTypeId", filter.InfectionTypeId.Value));
        }
        if (filter.AidTypeId.HasValue)
        {
            sql.Append(" AND r.AidType_ID = @AidTypeId");
            parameters.Add(("@AidTypeId", filter.AidTypeId.Value));
        }
        if (!string.IsNullOrWhiteSpace(filter.Message))
        {
            sql.Append(" AND r.Message = @Message");
            parameters.Add(("@Message", filter.Message));
        }
        if (filter.QuantityFrom.HasValue)
        {
            sql.Append(" AND r.Quantity_Ton >= @QuantityFrom");
            parameters.Add(("@QuantityFrom", filter.QuantityFrom.Value));
        }
        if (filter.QuantityTo.HasValue)
        {
            sql.Append(" AND r.Quantity_Ton <= @QuantityTo");
            parameters.Add(("@QuantityTo", filter.QuantityTo.Value));
        }
        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            sql.Append(" AND (r.CheckRequest_Number LIKE @Search OR r.Source LIKE @Search OR r.Notes LIKE @Search)");
            parameters.Add(("@Search", LikePattern(filter.Search)));
        }

        return (sql.ToString(), parameters);
    }

    private static string LikePattern(string value) =>
        "%" + value.Trim().Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]") + "%";

    private static void AddParameters(SqlCommand command, IReadOnlyList<(string Name, object? Value)> parameters)
    {
        foreach (var (name, value) in parameters)
            command.Parameters.AddWithValue(name, value ?? DBNull.Value);
    }

    private static string Str(SqlDataReader reader, int ordinal) =>
        reader.IsDBNull(ordinal)
            ? string.Empty
            : Convert.ToString(reader.GetValue(ordinal), CultureInfo.CurrentCulture) ?? string.Empty;

    private static long? Lng(SqlDataReader reader, int ordinal) =>
        reader.IsDBNull(ordinal) ? null : Convert.ToInt64(reader.GetValue(ordinal), CultureInfo.InvariantCulture);

    private static decimal? Dec(SqlDataReader reader, int ordinal) =>
        reader.IsDBNull(ordinal) ? null : Convert.ToDecimal(reader.GetValue(ordinal), CultureInfo.InvariantCulture);

    private static DateTime? Dt(SqlDataReader reader, int ordinal) =>
        reader.IsDBNull(ordinal) ? null : Convert.ToDateTime(reader.GetValue(ordinal), CultureInfo.InvariantCulture);

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
