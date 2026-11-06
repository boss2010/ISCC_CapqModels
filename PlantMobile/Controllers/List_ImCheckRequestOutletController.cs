using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Antiforgery;
using PlantMobile.Data;
using PlantMobile.Models;

namespace PlantMobile.Controllers;

[Route("Im_CheckRequests/List_ImCheckRequestOutlet")]
public sealed class List_ImCheckRequestOutletController : Controller
{
    private readonly CheckRequestOutletReader _outletReader;
    private readonly IAntiforgery _antiforgery;
    private readonly ILogger<List_ImCheckRequestOutletController> _logger;

    public List_ImCheckRequestOutletController(
        CheckRequestOutletReader outletReader,
        IAntiforgery antiforgery,
        ILogger<List_ImCheckRequestOutletController> logger)
    {
        _outletReader = outletReader;
        _antiforgery = antiforgery;
        _logger = logger;
    }

    [HttpGet("Index")]
    [HttpGet] // bare menu URL Im_CheckRequests/List_ImCheckRequestOutlet (MVC5 default-action behavior)
    public async Task<IActionResult> Index()
    {
        if (HttpContext.Session.GetString("UserSession") != "Authenticated")
            return RedirectToAction("Index", "Login");

        if (!TryReadUser(out var userId))
        {
            ViewBag.NoPort = true;
            return View(new List<CheckRequestOutletRowVm>());
        }

        try
        {
            var result = await _outletReader.ReadForUserAsync(userId, HttpContext.RequestAborted).ConfigureAwait(false);
            ViewBag.NoPort = result.PortNationalId is null;
            ViewBag.OutletOptions = result.OutletOptions;
            ViewBag.PortNationalId = result.PortNationalId;
            return View(result.Requests);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load check requests for outlet. TraceId={TraceId}", HttpContext.TraceIdentifier);
            ViewBag.NoPort = true;
            ViewBag.LoadError = $"تعذر تحميل طلبات الفحص. رقم تتبع المشكلة: {HttpContext.TraceIdentifier}";
            return View(new List<CheckRequestOutletRowVm>());
        }
    }

    [HttpPost("Update_Check_Outlet")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Update_Check_Outlet([FromBody] UpdateOutletRequestVm request)
    {
        if (HttpContext.Session.GetString("UserSession") != "Authenticated")
            return Unauthorized(new { Result = "ERROR", Message = "الجلسة غير صالحة." });

        if (request is null || request.model.Count == 0)
            return Json(new { Result = "OK", Message = "لا توجد طلبات للتحديث." });

        if (!TryReadUser(out var userId))
            return Unauthorized(new { Result = "ERROR", Message = "تعذر تحديد المستخدم الحالي." });

        try
        {
            await _outletReader.UpdateOutletAssignmentsAsync(request.model, userId, HttpContext.RequestAborted)
                .ConfigureAwait(false);
            return Json(new { Result = "OK", Message = "تم الحفظ بنجاح." });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to assign requests to outlet. TraceId={TraceId}", HttpContext.TraceIdentifier);
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { Result = "ERROR", Message = $"تعذر حفظ المنفذ. رقم تتبع المشكلة: {HttpContext.TraceIdentifier}" });
        }
    }

    private bool TryReadUser(out short userId)
    {
        userId = 0;
        return short.TryParse(HttpContext.Session.GetString("UserId"), NumberStyles.Integer, CultureInfo.InvariantCulture, out userId);
    }
}
