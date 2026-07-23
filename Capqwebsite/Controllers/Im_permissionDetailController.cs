using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ViewModels;

namespace Capqwebsite.Controllers;

[Route("Im_Permissions/Im_permissionDetail")]
public class Im_permissionDetailController : Controller
{
    private readonly string _connectionString;
    private readonly ILogger<Im_permissionDetailController> _logger;

    public Im_permissionDetailController(
        IConfiguration configuration,
        ILogger<Im_permissionDetailController> logger)
    {
        _connectionString = configuration.GetConnectionString("DBConnection")
            ?? throw new InvalidOperationException("DBConnection is not configured.");
        _logger = logger;
    }

    [HttpGet("Index")]
    public async Task<IActionResult> Index(string? ImPermission_Number)
    {
        if (HttpContext.Session.GetString("UserSession") != "Authenticated")
            return RedirectToAction("Index", "Login");

        var model = new ImPermissionDetailVm
        {
            PermissionNumber = ImPermission_Number?.Trim() ?? string.Empty,
            TraceId = HttpContext.TraceIdentifier
        };

        if (string.IsNullOrWhiteSpace(model.PermissionNumber))
        {
            model.ErrorMessage = "رقم إذن الاستيراد غير موجود في الرابط.";
            return View(model);
        }

        if (!decimal.TryParse(model.PermissionNumber, NumberStyles.None, CultureInfo.InvariantCulture, out decimal permissionNumber))
        {
            model.ErrorMessage = "رقم إذن الاستيراد غير صحيح؛ يجب أن يحتوي على أرقام فقط.";
            return View(model);
        }

        try
        {
            await LoadDetailAsync(model, permissionNumber);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Failed to load import permission detail. ImPermissionNumber={ImPermissionNumber}; TraceId={TraceId}",
                model.PermissionNumber, model.TraceId);
            model.ErrorMessage = BuildVisibleError(ex);
        }

        return View(model);
    }

    private async Task LoadDetailAsync(ImPermissionDetailVm model, decimal permissionNumber)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();

        const string headerSql = """
            SELECT TOP (1) Im_PermissionRequest_ID,
                   CONVERT(nvarchar(50), ImPermission_Number),
                   Arrival_Date, Start_Date, End_Date,
                   COALESCE(operationTypeName, N''), COALESCE(ExportCountryName, N''),
                   COALESCE(ImporterName, N''), COALESCE(ImporterTypeName, N''),
                   IsAcceppted, IsPaid, Renewal_Status
            FROM dbo.View_List_Im_PermissionRequest
            WHERE ImPermission_Number = @PermissionNumber
            ORDER BY Im_PermissionRequest_ID DESC
            """;
        await using (var command = new SqlCommand(headerSql, connection))
        {
            command.Parameters.AddWithValue("@PermissionNumber", permissionNumber);
            await using var reader = await command.ExecuteReaderAsync();
            if (!await reader.ReadAsync())
            {
                model.ErrorMessage = $"لم يتم العثور على إذن استيراد برقم {model.PermissionNumber}.";
                return;
            }

            model.Id = reader.GetInt64(0);
            model.PermissionNumber = DbString(reader, 1);
            model.ArrivalDate = reader.IsDBNull(2) ? null : Convert.ToDateTime(reader.GetValue(2));
            model.StartDate = reader.IsDBNull(3) ? null : Convert.ToDateTime(reader.GetValue(3));
            model.EndDate = reader.IsDBNull(4) ? null : Convert.ToDateTime(reader.GetValue(4));
            model.OperationType = DbString(reader, 5);
            model.Country = DbString(reader, 6);
            model.Company = DbString(reader, 7);
            model.ImporterType = DbString(reader, 8);
            model.IsAccepted = reader.IsDBNull(9) ? null : reader.GetBoolean(9);
            model.IsPaid = reader.IsDBNull(10) ? null : reader.GetBoolean(10);
            model.RenewalStatus = reader.IsDBNull(11) ? null : reader.GetByte(11);
        }

        const string itemsSql = """
            SELECT permissionItem.ID, COALESCE(permissionItem.Item_Permission_Number, N''),
                   COALESCE(item.Name_Ar, N''), COALESCE(shortName.ShortName_Ar, N''),
                   COALESCE(country.Ar_Name, N''), permissionItem.Package_Count,
                   permissionItem.Package_Weight, permissionItem.GrossWeight,
                   permissionItem.Size, permissionItem.IsAccepted
            FROM dbo.Im_PermissionItems permissionItem
            LEFT JOIN dbo.Im_Initiator initiator ON initiator.ID = permissionItem.Im_Initiator_ID
            LEFT JOIN dbo.Country country ON country.ID = initiator.Country_Id
            LEFT JOIN dbo.Item_ShortName shortName ON shortName.ID = permissionItem.Item_ShortName_ID
            LEFT JOIN dbo.Item item ON item.ID = shortName.Item_ID
            WHERE permissionItem.Im_PermissionRequest_ID = @PermissionRequestId
            ORDER BY permissionItem.ID
            """;
        await using (var command = new SqlCommand(itemsSql, connection))
        {
            command.Parameters.AddWithValue("@PermissionRequestId", model.Id);
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                model.Items.Add(new ImPermissionDetailItemVm
                {
                    Id = reader.GetInt64(0),
                    ItemPermissionNumber = DbString(reader, 1),
                    ItemName = DbString(reader, 2),
                    ShortName = DbString(reader, 3),
                    OriginCountry = DbString(reader, 4),
                    PackageCount = reader.IsDBNull(5) ? null : reader.GetInt32(5),
                    PackageWeight = reader.IsDBNull(6) ? null : reader.GetDecimal(6),
                    GrossWeight = reader.IsDBNull(7) ? null : reader.GetDecimal(7),
                    Size = reader.IsDBNull(8) ? null : Convert.ToDouble(reader.GetValue(8)),
                    IsAccepted = reader.GetBoolean(9)
                });
            }
        }
    }

    private static string BuildVisibleError(Exception exception)
    {
        var messages = new List<string>();
        for (Exception? current = exception; current is not null; current = current.InnerException)
        {
            if (!string.IsNullOrWhiteSpace(current.Message) && !messages.Contains(current.Message))
                messages.Add(current.Message);
        }
        return string.Join(" ← ", messages);
    }

    private static string DbString(SqlDataReader reader, int ordinal) =>
        reader.IsDBNull(ordinal) ? string.Empty : Convert.ToString(reader.GetValue(ordinal)) ?? string.Empty;
}
