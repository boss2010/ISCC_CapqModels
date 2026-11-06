using System.Globalization;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using PlantMobile.Data;

namespace PlantMobile.Controllers;

/// <summary>
/// Legacy screen: Im_Permissions/Stopping_Permission
/// Search by permission number, show the request state and stop the permission.
/// </summary>
[Route("Im_Permissions/Stopping_Permission")]
public sealed class Stopping_PermissionController : Controller
{
    // Keep property names as-is (PascalCase) so the JavaScript in the view keeps working.
    private static readonly JsonSerializerOptions ScreenJsonOptions = new() { PropertyNamingPolicy = null };

    private readonly PermissionScreenReader _reader;
    private readonly ILogger<Stopping_PermissionController> _logger;

    public Stopping_PermissionController(PermissionScreenReader reader,
        ILogger<Stopping_PermissionController> logger)
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
                    IS_Print_Ar = row.IS_Print_Ar,
                    IS_Print_EN = row.IS_Print_EN,
                    row.IsPaid,
                    row.IsAcceppted,
                    row.Im_CheckRequest_ID,
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
    public async Task<IActionResult> UpdateUserDatal(decimal ImPermission_Number, CancellationToken cancellationToken)
    {
        if (HttpContext.Session.GetString("UserSession") != "Authenticated")
            return Unauthorized(new { Result = "ERROR", Message = "انتهت الجلسة، برجاء تسجيل الدخول مرة أخرى." });

        if (ImPermission_Number <= 0)
            return Json(new { Result = "ERROR", Message = "رقم الإذن غير صحيح." }, ScreenJsonOptions);

        try
        {
            var affected = await _reader.StopAsync(ImPermission_Number, cancellationToken).ConfigureAwait(false);
            if (affected == 0)
                return Json(new { Result = "ERROR", Message = "لا يوجد إذن بهذا الرقم" }, ScreenJsonOptions);

            return Json(new { Result = "OK" }, ScreenJsonOptions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to stop permission {PermissionNumber}. TraceId={TraceId}", ImPermission_Number, HttpContext.TraceIdentifier);
            return Json(new { Result = "ERROR", Message = "تعذر إيقاف الإذن." }, ScreenJsonOptions);
        }
    }
}
