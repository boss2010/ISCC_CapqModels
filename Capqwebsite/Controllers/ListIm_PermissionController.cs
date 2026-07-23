using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ViewModels;

namespace Capqwebsite.Controllers;

public class ListIm_PermissionController : Controller
{
    private readonly string _connectionString;
    private readonly ILogger<ListIm_PermissionController> _logger;

    public ListIm_PermissionController(
        IConfiguration configuration,
        ILogger<ListIm_PermissionController> logger)
    {
        _connectionString = configuration.GetConnectionString("DBConnection")
            ?? throw new InvalidOperationException("DBConnection is not configured.");
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index(
        DateTime? fromDate,
        DateTime? toDate,
        string? permissionNumber,
        string? country,
        string? company,
        string? shortName)
    {
        if (HttpContext.Session.GetString("UserSession") != "Authenticated")
            return RedirectToAction("Index", "Login");

        DateTime today = DateTime.Today;
        var model = new ListImPermissionFilterVm
        {
            FromDate = (fromDate ?? today.AddDays(-6)).Date,
            ToDate = (toDate ?? today).Date,
            PermissionNumber = permissionNumber?.Trim() ?? string.Empty,
            Country = country?.Trim() ?? string.Empty,
            Company = company?.Trim() ?? string.Empty,
            ShortName = shortName?.Trim() ?? string.Empty
        };

        if (model.FromDate > model.ToDate)
        {
            ModelState.AddModelError(string.Empty, "تاريخ البداية يجب أن يكون قبل تاريخ النهاية.");
            return View(model);
        }

        try
        {
            model.Results = await LoadPermissionsAsync(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Failed to filter import permissions. From={FromDate}, To={ToDate}, Permission={PermissionNumber}, Country={Country}, Company={Company}, ShortName={ShortName}; TraceId={TraceId}",
                model.FromDate, model.ToDate, model.PermissionNumber, model.Country,
                model.Company, model.ShortName, HttpContext.TraceIdentifier);
            ViewBag.PageError = $"تعذر تحميل أذون الاستيراد. رقم تتبع المشكلة: {HttpContext.TraceIdentifier}";
        }

        return View(model);
    }

    private async Task<List<ListImPermissionRowVm>> LoadPermissionsAsync(ListImPermissionFilterVm filter)
    {
        var results = new List<ListImPermissionRowVm>();
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        const string sql = """
            SELECT TOP (1000)
                   Im_PermissionRequest_ID,
                   CONVERT(nvarchar(50), ImPermission_Number),
                   Arrival_Date, Start_Date, End_Date,
                   COALESCE(operationTypeName, N''),
                   COALESCE(ExportCountryName, N''),
                   COALESCE(ImporterName, N''),
                   COALESCE(ImporterTypeName, N''),
                   COALESCE(shortName, N''),
                   IsAcceppted, IsPaid, Renewal_Status
            FROM dbo.View_List_Im_PermissionRequest
            WHERE Arrival_Date >= @FromDate
              AND Arrival_Date < DATEADD(day, 1, @ToDate)
              AND (@PermissionNumber = N'' OR CONVERT(nvarchar(50), ImPermission_Number) LIKE N'%' + @PermissionNumber + N'%')
              AND (@Country = N'' OR ExportCountryName LIKE N'%' + @Country + N'%')
              AND (@Company = N'' OR ImporterName LIKE N'%' + @Company + N'%')
              AND (@ShortName = N'' OR shortName LIKE N'%' + @ShortName + N'%')
            ORDER BY Arrival_Date DESC, Im_PermissionRequest_ID DESC
            """;

        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 90 };
        command.Parameters.AddWithValue("@FromDate", filter.FromDate);
        command.Parameters.AddWithValue("@ToDate", filter.ToDate);
        command.Parameters.AddWithValue("@PermissionNumber", filter.PermissionNumber);
        command.Parameters.AddWithValue("@Country", filter.Country);
        command.Parameters.AddWithValue("@Company", filter.Company);
        command.Parameters.AddWithValue("@ShortName", filter.ShortName);

        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            results.Add(new ListImPermissionRowVm
            {
                Id = reader.GetInt64(0),
                PermissionNumber = reader.IsDBNull(1) ? string.Empty : reader.GetString(1),
                ArrivalDate = Convert.ToDateTime(reader.GetValue(2)),
                StartDate = reader.IsDBNull(3) ? null : Convert.ToDateTime(reader.GetValue(3)),
                EndDate = reader.IsDBNull(4) ? null : Convert.ToDateTime(reader.GetValue(4)),
                OperationType = DbString(reader, 5),
                Country = DbString(reader, 6),
                Company = DbString(reader, 7),
                ImporterType = DbString(reader, 8),
                ShortName = DbString(reader, 9),
                IsAccepted = reader.IsDBNull(10) ? null : reader.GetBoolean(10),
                IsPaid = reader.IsDBNull(11) ? null : reader.GetBoolean(11),
                RenewalStatus = reader.IsDBNull(12) ? null : reader.GetByte(12)
            });
        }

        return results;
    }

    private static string DbString(SqlDataReader reader, int ordinal) =>
        reader.IsDBNull(ordinal) ? string.Empty : Convert.ToString(reader.GetValue(ordinal)) ?? string.Empty;
}
