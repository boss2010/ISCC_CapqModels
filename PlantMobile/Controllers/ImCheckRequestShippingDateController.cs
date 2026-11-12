using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using PlantMobile.Data;
using PlantMobile.Models;

namespace PlantMobile.Controllers;

[Route("Im_CheckRequests/ShippingDate")]
public sealed class ImCheckRequestShippingDateController : Controller
{
    private readonly ImCheckRequestDisplayReader _reader;
    private readonly MenuAccessReader _accessReader;
    private readonly ILogger<ImCheckRequestShippingDateController> _logger;

    public ImCheckRequestShippingDateController(
        ImCheckRequestDisplayReader reader,
        MenuAccessReader accessReader,
        ILogger<ImCheckRequestShippingDateController> logger)
    {
        _reader = reader;
        _accessReader = accessReader;
        _logger = logger;
    }

    [HttpGet("Index")]
    [HttpGet("")]
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        if (!IsAuthenticated() || !TryReadUser(out short userId))
            return RedirectToAction("Index", "Login");

        if (!await _accessReader.CanViewAsync(
                userId, MenuAccessReader.ShippingDateMenuUrl, cancellationToken).ConfigureAwait(false))
            return Forbid();

        return View();
    }

    [HttpGet("Edit")]
    public async Task<IActionResult> Edit(
        string ImCheckRequest_Number,
        string? returnUrl,
        CancellationToken cancellationToken)
    {
        if (!IsAuthenticated() || !TryReadUser(out short userId))
            return RedirectToAction("Index", "Login");

        if (!await _accessReader.CanViewAsync(
                userId, MenuAccessReader.ShippingDateMenuUrl, cancellationToken).ConfigureAwait(false))
            return Forbid();

        if (string.IsNullOrWhiteSpace(ImCheckRequest_Number))
            return BadRequest("رقم طلب الفحص غير موجود.");

        try
        {
            var model = await _reader.ReadShippingDateAsync(
                ImCheckRequest_Number.Trim(), cancellationToken).ConfigureAwait(false);
            if (model is null)
                return NotFound("لم يتم العثور على الطلب أو بيانات الشهادة الجمركية الخاصة به.");

            model.ReturnUrl = LocalReturnUrl(returnUrl, model.CheckRequestNumber);
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Failed to load shipping date editor. CheckNumber={CheckNumber}; TraceId={TraceId}",
                ImCheckRequest_Number, HttpContext.TraceIdentifier);
            TempData["Message"] = $"تعذر تحميل بيانات الطلب. رقم تتبع المشكلة: {HttpContext.TraceIdentifier}";
            return RedirectToAction("Index", "Im_CheckRequestDisplay", new { ImCheckRequest_Number });
        }
    }

    [HttpPost("Edit")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(
        EditShippingDateVm model,
        CancellationToken cancellationToken)
    {
        if (!IsAuthenticated() || !TryReadUser(out short userId))
            return RedirectToAction("Index", "Login");

        if (!await _accessReader.CanViewAsync(
                userId, MenuAccessReader.ShippingDateMenuUrl, cancellationToken).ConfigureAwait(false))
            return Forbid();

        model.ReturnUrl = LocalReturnUrl(model.ReturnUrl, model.CheckRequestNumber);

        if (model.ShippingDate.HasValue
            && model.ArrivalDate.HasValue
            && model.ShippingDate.Value.Date > model.ArrivalDate.Value.Date)
        {
            ModelState.AddModelError(nameof(model.ShippingDate),
                "تاريخ الشحن لا يمكن أن يكون بعد تاريخ الوصول.");
        }

        if (!ModelState.IsValid)
            return View(model);

        try
        {
            await _reader.UpdateShippingDateAsync(model, userId, cancellationToken)
                .ConfigureAwait(false);
            TempData["Message"] = $"تم تعديل تاريخ الشحن للطلب {model.CheckRequestNumber} بنجاح.";
            return LocalRedirect(model.ReturnUrl);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Failed to update shipping date. CheckNumber={CheckNumber}; TraceId={TraceId}",
                model.CheckRequestNumber, HttpContext.TraceIdentifier);
            ModelState.AddModelError(string.Empty,
                $"تعذر حفظ تاريخ الشحن. رقم تتبع المشكلة: {HttpContext.TraceIdentifier}");
            return View(model);
        }
    }

    private bool IsAuthenticated() =>
        HttpContext.Session.GetString("UserSession") == "Authenticated";

    private bool TryReadUser(out short userId)
    {
        return short.TryParse(
            HttpContext.Session.GetString("UserId"),
            NumberStyles.Integer,
            CultureInfo.InvariantCulture,
            out userId);
    }

    private string LocalReturnUrl(string? returnUrl, string checkNumber)
    {
        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
            return returnUrl;

        return Url.Action(
            "Index",
            "Im_CheckRequestDisplay",
            new { ImCheckRequest_Number = checkNumber }) ?? "/";
    }
}
