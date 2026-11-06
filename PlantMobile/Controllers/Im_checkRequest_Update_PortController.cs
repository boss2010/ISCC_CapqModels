using System.Globalization;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Antiforgery;
using PlantMobile.Data;
using PlantMobile.Models;

namespace PlantMobile.Controllers;

[Route("Im_CheckRequests/Im_checkRequest_Update_Port")]
public sealed class Im_checkRequest_Update_PortController : Controller
{
    private readonly CheckRequestPortReader _portReader;
    private readonly IAntiforgery _antiforgery;
    private readonly ILogger<Im_checkRequest_Update_PortController> _logger;

    public Im_checkRequest_Update_PortController(
        CheckRequestPortReader portReader,
        IAntiforgery antiforgery,
        ILogger<Im_checkRequest_Update_PortController> logger)
    {
        _portReader = portReader;
        _antiforgery = antiforgery;
        _logger = logger;
    }

    [HttpGet("Index")]
    [HttpGet] // bare menu URL Im_CheckRequests/Im_checkRequest_Update_Port (MVC5 default-action behavior)
    public IActionResult Index()
    {
        if (HttpContext.Session.GetString("UserSession") != "Authenticated")
            return RedirectToAction("Index", "Login");

        return View();
    }

    [HttpPost("Port_List")]
    public async Task<IActionResult> Port_List(string CheckNumber)
    {
        if (HttpContext.Session.GetString("UserSession") != "Authenticated")
            return Unauthorized(new { Result = "ERROR", Message = "الجلسة غير صالحة." });

        if (string.IsNullOrWhiteSpace(CheckNumber))
            return BadRequest(new { Result = "ERROR", Message = "رقم الطلب مطلوب." });

        try
        {
            var rows = await _portReader.ReadByCheckNumberAsync(CheckNumber.Trim(), HttpContext.RequestAborted)
                .ConfigureAwait(false);
            if (rows.Count == 0)
                return Json(new { Result = 2, Records = rows });

            var committeeFormed = rows.Any(row => row.CommitteeFormed);
            return Json(new { Result = committeeFormed ? 1 : 0, Records = rows });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load check request ports. CheckNumber={CheckNumber}; TraceId={TraceId}", CheckNumber, HttpContext.TraceIdentifier);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { Result = "ERROR", Message = $"تعذر تحميل بيانات الميناء. رقم تتبع المشكلة: {HttpContext.TraceIdentifier}" });
        }
    }

    [HttpPost("Port_ID")]
    public async Task<IActionResult> Port_ID(int government_ID)
    {
        var options = await _portReader.GetNationalPortsAsync(government_ID, HttpContext.RequestAborted).ConfigureAwait(false);
        return Json(new { Result = "OK", Options = options });
    }

    [HttpPost("InternationalPort_ID")]
    public async Task<IActionResult> InternationalPort_ID(int country_ID)
    {
        var options = await _portReader.GetInternationalPortsAsync(country_ID, HttpContext.RequestAborted).ConfigureAwait(false);
        return Json(new { Result = "OK", Options = options });
    }

    [HttpPost("country_ID")]
    public async Task<IActionResult> Country_ID()
    {
        var options = await _portReader.GetCountryOptionsAsync(HttpContext.RequestAborted).ConfigureAwait(false);
        return Json(new { Result = "OK", Options = options });
    }

    [HttpPost("Government_ID")]
    public async Task<IActionResult> Government_ID()
    {
        var options = await _portReader.GetGovernmentOptionsAsync(HttpContext.RequestAborted).ConfigureAwait(false);
        return Json(new { Result = "OK", Options = options });
    }

    [HttpPost("update_port")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> UpdatePort([FromBody] UpdatePortsRequestVm request)
    {
        if (HttpContext.Session.GetString("UserSession") != "Authenticated")
            return Unauthorized(new { Result = "ERROR", Message = "الجلسة غير صالحة." });

        if (request is null)
            return BadRequest(new { Result = "ERROR", Message = "بيانات الحفظ غير موجودة." });

        if (!TryReadUser(out var userId))
            return Unauthorized(new { Result = "ERROR", Message = "تعذر تحديد المستخدم الحالي." });

        try
        {
            request.CheckNumber = request.CheckNumber?.Trim() ?? string.Empty;
            await _portReader.UpdatePortsAsync(request, userId, HttpContext.RequestAborted).ConfigureAwait(false);
            return Json(new { Result = "OK" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update check request ports. CheckNumber={CheckNumber}; TraceId={TraceId}", request.CheckNumber, HttpContext.TraceIdentifier);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { Result = "ERROR", Message = $"تعذر حفظ الميناء. رقم تتبع المشكلة: {HttpContext.TraceIdentifier}" });
        }
    }

    private bool TryReadUser(out short userId)
    {
        userId = 0;
        return short.TryParse(HttpContext.Session.GetString("UserId"), NumberStyles.Integer, CultureInfo.InvariantCulture, out userId);
    }
}
