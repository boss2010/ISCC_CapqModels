using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Antiforgery;
using PlantMobile.Data;
using PlantMobile.Models;

namespace PlantMobile.Controllers;

[Route("Im_CheckRequests/Im_CheckRequestDisplay")]
public sealed class Im_CheckRequestDisplayController : Controller
{
    private readonly ImCheckRequestDisplayReader _displayReader;
    private readonly IAntiforgery _antiforgery;
    private readonly ILogger<Im_CheckRequestDisplayController> _logger;

    public Im_CheckRequestDisplayController(
        ImCheckRequestDisplayReader displayReader,
        IAntiforgery antiforgery,
        ILogger<Im_CheckRequestDisplayController> logger)
    {
        _displayReader = displayReader;
        _antiforgery = antiforgery;
        _logger = logger;
    }

    [HttpGet("Index")]
    [HttpGet] // bare menu URL Im_CheckRequests/Im_CheckRequestDisplay/... (MVC5 default-action behavior)
    public async Task<IActionResult> Index(string ImCheckRequest_Number, long? OutLet_ID)
    {
        if (HttpContext.Session.GetString("UserSession") != "Authenticated")
            return RedirectToAction("Index", "Login");

        var model = new ImCheckRequestDisplayVm
        {
            CheckRequest_Number = ImCheckRequest_Number?.Trim() ?? string.Empty
        };

        if (!TryReadUser(out var userId))
            return RedirectToAction("Index", "Login");

        if (string.IsNullOrWhiteSpace(model.CheckRequest_Number))
        {
            model.ErrorMessage = "رقم طلب الفحص غير موجود في الرابط.";
            return View(model);
        }

        try
        {
            var result = await _displayReader.ReadDetailsAsync(model.CheckRequest_Number, HttpContext.RequestAborted)
                .ConfigureAwait(false);
            if (result is null)
            {
                model.ErrorMessage = $"لم يتم العثور على طلب فحص برقم {model.CheckRequest_Number}.";
                return View(model);
            }

            return View(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load check request display page. CheckNumber={CheckNumber}; TraceId={TraceId}", ImCheckRequest_Number, HttpContext.TraceIdentifier);
            model.ErrorMessage = $"تعذر تحميل بيانات الطلب. رقم تتبع المشكلة: {HttpContext.TraceIdentifier}";
            return View(model);
        }
    }

    [HttpPost("acceptRequest")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AcceptRequest([FromForm] AcceptDisplayRequestVm model)
    {
        if (HttpContext.Session.GetString("UserSession") != "Authenticated")
            return Unauthorized(new { success = false, message = "الجلسة غير صالحة." });

        if (model is null)
            return BadRequest(new { success = false, message = "بيانات الطلب غير موجودة." });

        try
        {
            await _displayReader.AcceptRequestAsync(model.ID, model.IsAccepted ?? false, HttpContext.RequestAborted)
                .ConfigureAwait(false);

            TempData["Message"] = model.IsAccepted == true ? "تم قبول الطلب بنجاح" : "تم رفض الطلب بنجاح";
            return RedirectToAction(nameof(Index), new
            {
                ImCheckRequest_Number = model.CheckRequest_Number,
                Outlet_ID = model.Outlet_ID
            });
        }
        catch (InvalidOperationException ex)
        {
            TempData["Message"] = ex.Message;
            return RedirectToAction(nameof(Index), new
            {
                ImCheckRequest_Number = model.CheckRequest_Number,
                Outlet_ID = model.Outlet_ID
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to accept/reject check request {RequestId}. TraceId={TraceId}", model.ID, HttpContext.TraceIdentifier);
            TempData["Message"] = $"تعذر تحديث الطلب. رقم تتبع المشكلة: {HttpContext.TraceIdentifier}";
            return RedirectToAction(nameof(Index), new
            {
                ImCheckRequest_Number = model.CheckRequest_Number,
                Outlet_ID = model.Outlet_ID
            });
        }
    }

    [HttpPost("ContinueRequest")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ContinueRequest([FromForm] ContinueDisplayRequestVm model)
    {
        if (HttpContext.Session.GetString("UserSession") != "Authenticated")
            return Unauthorized(new { success = false, message = "الجلسة غير صالحة." });

        if (model is null || string.IsNullOrWhiteSpace(model.Completion_Notes))
        {
            TempData["Message"] = "يرجى كتابة ملاحظات الاستكمال";
            return RedirectToAction(nameof(Index), new
            {
                ImCheckRequest_Number = model?.CheckRequest_Number,
                Outlet_ID = model?.Outlet_ID
            });
        }

        try
        {
            await _displayReader.ContinueRequestAsync(model.ID, model.Completion_Notes, HttpContext.RequestAborted)
                .ConfigureAwait(false);

            TempData["Message"] = "تم عمل استكمال للطلب بنجاح";
        }
        catch (InvalidOperationException ex)
        {
            TempData["Message"] = ex.Message;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to continue check request {RequestId}. TraceId={TraceId}", model.ID, HttpContext.TraceIdentifier);
            TempData["Message"] = $"تعذر استكمال الطلب. رقم تتبع المشكلة: {HttpContext.TraceIdentifier}";
        }

        return RedirectToAction(nameof(Index), new
        {
            ImCheckRequest_Number = model.CheckRequest_Number,
            Outlet_ID = model.Outlet_ID
        });
    }

    [HttpPost("AppendHSCode")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AppendHSCode(long requestItemId, string addition)
    {
        if (HttpContext.Session.GetString("UserSession") != "Authenticated")
            return Json(new { success = false, message = "الجلسة غير صالحة." });

        try
        {
            await _displayReader.AppendHSCodeAsync(requestItemId, addition, HttpContext.RequestAborted)
                .ConfigureAwait(false);
            return Json(new { success = true, message = "تم تعديل HS Code بنجاح" });
        }
        catch (InvalidOperationException ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to append HS Code for item {ItemId}. TraceId={TraceId}", requestItemId, HttpContext.TraceIdentifier);
            return Json(new { success = false, message = $"تعذر حفظ التعديل. رقم تتبع المشكلة: {HttpContext.TraceIdentifier}" });
        }
    }

    [HttpPost("MarkCompletionResponded")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MarkCompletionResponded(long requestId)
    {
        if (HttpContext.Session.GetString("UserSession") != "Authenticated")
            return Json(new { success = false, message = "الجلسة غير صالحة." });

        if (!TryReadUser(out var userId))
            return Json(new { success = false, message = "تعذر تحديد المستخدم الحالي." });

        try
        {
            await _displayReader.MarkCompletionRespondedAsync(requestId, userId, HttpContext.RequestAborted)
                .ConfigureAwait(false);
            return Json(new { success = true, message = "تم إرسال الاستكمال للمراجعة" });
        }
        catch (InvalidOperationException ex)
        {
            return Json(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to mark completion responded for request {RequestId}. TraceId={TraceId}", requestId, HttpContext.TraceIdentifier);
            return Json(new { success = false, message = $"تعذر إرسال الاستكمال. رقم تتبع المشكلة: {HttpContext.TraceIdentifier}" });
        }
    }

    private bool TryReadUser(out short userId)
    {
        userId = 0;
        return short.TryParse(HttpContext.Session.GetString("UserId"), NumberStyles.Integer, CultureInfo.InvariantCulture, out userId);
    }
}
