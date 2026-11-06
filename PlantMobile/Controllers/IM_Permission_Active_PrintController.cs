using System.Globalization;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using PlantMobile.Data;

namespace PlantMobile.Controllers;

/// <summary>
/// Legacy screen: Im_Permissions/IM_Permission_Active_Print
/// Search by permission number, show the Arabic/English print state and re-enable printing.
/// </summary>
[Route("Im_Permissions/IM_Permission_Active_Print")]
public sealed class IM_Permission_Active_PrintController : Controller
{
    // Keep property names as-is (PascalCase) so the legacy JavaScript in the view keeps working.
    private static readonly JsonSerializerOptions ScreenJsonOptions = new() { PropertyNamingPolicy = null };

    private readonly PermissionScreenReader _reader;
    private readonly ILogger<IM_Permission_Active_PrintController> _logger;

    public IM_Permission_Active_PrintController(PermissionScreenReader reader,
        ILogger<IM_Permission_Active_PrintController> logger)
    {
        _reader = reader;
        _logger = logger;
    }

    [HttpGet("")]
    [HttpGet("Index")]
    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("UserSession") != "Authenticated")
            return RedirectToAction("Index", "Login");

        return View();
    }

    [HttpPost("GetPermissionsList")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> GetPermissionsList(decimal? ImPermission_Number, CancellationToken cancellationToken)
    {
        if (HttpContext.Session.GetString("UserSession") != "Authenticated")
            return Unauthorized(new { Result = "ERROR", Message = "انتهت الجلسة، برجاء تسجيل الدخول مرة أخرى." });

        if (ImPermission_Number is null or <= 0)
            return Json(new { Result = "ERROR", Message = "أدخل رقم الإذن." }, ScreenJsonOptions);

        try
        {
            var row = await _reader.ReadAsync(ImPermission_Number.Value, cancellationToken).ConfigureAwait(false);
            if (row is null)
                return Json(new { Result = "ERROR", Message = "لا يوجد إذن بهذا الرقم" }, ScreenJsonOptions);

            return Json(new[]
            {
                new
                {
                    row.Im_PermissionRequest_ID,
                    row.ImPermission_Number,
                    IS_Print_Ar = row.IS_Print_Ar ?? false,
                    IS_Print_EN = row.IS_Print_EN ?? false,
                    row.IsPaid,
                    row.IsAcceppted,
                    Arrival_Date = row.Arrival_Date?.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) ?? string.Empty,
                    row.ImporterName,
                    row.ImporterTypeName,
                    row.ExportCountryName,
                    row.ShortName
                }
            }, ScreenJsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load permission {PermissionNumber}. TraceId={TraceId}", ImPermission_Number, HttpContext.TraceIdentifier);
            return StatusCode(StatusCodes.Status500InternalServerError, new { Result = "ERROR", Message = "حدث خطأ أثناء تحميل بيانات الإذن." });
        }
    }

    [HttpPost("UpdateUserDatal")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdateUserDatal(long ImPermission_Number_ID, bool IS_Print_Ar, bool IS_Print_EN,
        string? NOTS_AR, string? NOTS_EN, CancellationToken cancellationToken)
    {
        if (HttpContext.Session.GetString("UserSession") != "Authenticated")
            return Unauthorized(new { Result = "ERROR", Message = "انتهت الجلسة، برجاء تسجيل الدخول مرة أخرى." });

        if (!short.TryParse(HttpContext.Session.GetString("UserId"), NumberStyles.Integer, CultureInfo.InvariantCulture, out var userId))
            return Unauthorized(new { Result = "ERROR", Message = "تعذر تحديد المستخدم الحالي." });

        if (ImPermission_Number_ID <= 0)
            return Json(new { Result = "ERROR", Message = "رقم الإذن غير صحيح." }, ScreenJsonOptions);

        try
        {
            await _reader.UpdatePrintFlagsAsync(ImPermission_Number_ID, IS_Print_Ar, IS_Print_EN, userId,
                NOTS_AR ?? string.Empty, NOTS_EN ?? string.Empty, cancellationToken).ConfigureAwait(false);
            return Json(new { Result = "OK" }, ScreenJsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update print flags for permission {PermissionId}. TraceId={TraceId}", ImPermission_Number_ID, HttpContext.TraceIdentifier);
            return Json(new { Result = "ERROR", Message = "تعذر حفظ التعديل." }, ScreenJsonOptions);
        }
    }
}
