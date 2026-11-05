using System.Globalization;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Antiforgery;
using PlantMobile.Data;
using PlantMobile.Models;
using SelectPdf;

namespace PlantMobile.Controllers;

[Route("Im_Permissions/Im_PermissionRequestPrint")]
public sealed class Im_PermissionRequestPrintController : Controller
{
    private readonly PermissionPrintReader _printReader;
    private readonly PermissionMenuAccessReader _accessReader;
    private readonly IConfiguration _configuration;
    private readonly IAntiforgery _antiforgery;
    private readonly ILogger<Im_PermissionRequestPrintController> _logger;

    public Im_PermissionRequestPrintController(
        PermissionPrintReader printReader,
        PermissionMenuAccessReader accessReader,
        IConfiguration configuration,
        IAntiforgery antiforgery,
        ILogger<Im_PermissionRequestPrintController> logger)
    {
        _printReader = printReader;
        _accessReader = accessReader;
        _configuration = configuration;
        _antiforgery = antiforgery;
        _logger = logger;
    }

    [HttpGet("Index")]
    public async Task<IActionResult> Index(string? ImPermission_Number)
    {
        if (HttpContext.Session.GetString("UserSession") != "Authenticated")
            return RedirectToAction("Index", "Login");

        if (!TryReadUser(out var userId))
            return RedirectToAction("Index", "Login");

        if (!short.TryParse(userId.ToString(CultureInfo.InvariantCulture), CultureInfo.InvariantCulture, out var smallUserId))
            return RedirectToAction("Index", "Login");

        var model = new ImPermissionPrintDetailsVm
        {
            ImPermission_Number = ImPermission_Number?.Trim() ?? string.Empty
        };

        if (string.IsNullOrWhiteSpace(model.ImPermission_Number))
        {
            model.ErrorMessage = "رقم إذن الاستيراد غير موجود في الرابط.";
            return View(model);
        }

        if (!decimal.TryParse(model.ImPermission_Number, NumberStyles.None, CultureInfo.InvariantCulture, out var permissionNumber))
        {
            model.ErrorMessage = "رقم إذن الاستيراد غير صحيح؛ يجب أن يحتوي على أرقام فقط.";
            return View(model);
        }

        var access = await _accessReader.ReadListPermissionsAsync(smallUserId, HttpContext.RequestAborted).ConfigureAwait(false);

        try
        {
            var details = await _printReader.ReadDetailsAsync(permissionNumber, smallUserId, HttpContext.RequestAborted).ConfigureAwait(false);
            if (details is null)
            {
                TempData["ErrorMessage"] = "تعذر تحميل بيانات الطباعة لهذا الإذن.";
                return RedirectToAction("Index", "ListIm_Permission");
            }

            details.CanPrint = access.CanPrint;
            return View(details);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load import permission print page. PermissionNumber={PermissionNumber}; TraceId={TraceId}", model.ImPermission_Number, HttpContext.TraceIdentifier);
            model.ErrorMessage = "تعذر تحميل بيانات الطباعة لهذا الإذن.";
            return View(model);
        }
    }

    [HttpPost("PrintArabic")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PrintArabic(long Im_PermissionRequest_ID) =>
        await MarkPrintedAsync(Im_PermissionRequest_ID, arabic: true);

    [HttpPost("PrintEnglish")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> PrintEnglish(long Im_PermissionRequest_ID) =>
        await MarkPrintedAsync(Im_PermissionRequest_ID, arabic: false);

    private async Task<IActionResult> MarkPrintedAsync(long permissionRequestId, bool arabic)
    {
        if (HttpContext.Session.GetString("UserSession") != "Authenticated")
            return Unauthorized(new { success = false, message = "تعذر تحديد الجلسة الحالية." });

        if (!TryReadUser(out var userId))
            return Unauthorized(new { success = false, message = "تعذر تحديد المستخدم الحالي." });

        var access = await _accessReader.ReadListPermissionsAsync((short)userId, HttpContext.RequestAborted).ConfigureAwait(false);
        if (access.CanPrint != true)
            return StatusCode(StatusCodes.Status403Forbidden, new { success = false, message = "لا توجد صلاحيات للطباعة." });

        try
        {
            await _printReader.MarkPrintedAsync(permissionRequestId, (short)userId, arabic, HttpContext.RequestAborted).ConfigureAwait(false);
            return Json(new { success = true });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Print update failed for permission {PermissionId}. TraceId={TraceId}", permissionRequestId, HttpContext.TraceIdentifier);
            return NotFound(new { success = false, message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Print update failed for permission {PermissionId}. TraceId={TraceId}", permissionRequestId, HttpContext.TraceIdentifier);
            return StatusCode(StatusCodes.Status500InternalServerError, new { success = false, message = "تعذر تسجيل الطباعة." });
        }
    }

    [HttpPost("ExportToPDF")]
    [ValidateAntiForgeryToken]
    public IActionResult ExportToPDF(string richTxt, decimal? permissionNumber, int lang)
    {
        if (HttpContext.Session.GetString("UserSession") != "Authenticated")
            return Unauthorized(new { filename = string.Empty });

        if (!TryReadUser(out var userId))
            return Unauthorized(new { filename = string.Empty });

        var access = _accessReader.ReadListPermissionsAsync((short)userId, HttpContext.RequestAborted).GetAwaiter().GetResult();
        if (access.CanPrint != true)
            return StatusCode(StatusCodes.Status403Forbidden, new { filename = string.Empty, message = "لا توجد صلاحية للطباعة." });

        if (string.IsNullOrWhiteSpace(richTxt) || permissionNumber is null || permissionNumber <= 0)
            return BadRequest(new { filename = string.Empty });

        try
        {
            var appUrl = (_configuration["applicationUrl"] ?? "http://localhost").TrimEnd('/') + "/";
            var html = richTxt.Replace("src=\"", $"src=\"{appUrl}", StringComparison.OrdinalIgnoreCase);
            var extension = lang == 1 ? "-AR.pdf" : "-EN.pdf";
            var fileName = permissionNumber.Value.ToString(CultureInfo.InvariantCulture) + extension;

            var permissionFolder = _configuration["permissionsUpload"];
            if (string.IsNullOrWhiteSpace(permissionFolder))
                return StatusCode(StatusCodes.Status500InternalServerError, new { filename = string.Empty, message = "مسار حفظ ملف PDF غير مضبوط." });

            var folder = permissionFolder.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            Directory.CreateDirectory(folder);
            var fileLocation = Path.Combine(folder, fileName);
            if (System.IO.File.Exists(fileLocation))
                System.IO.File.Delete(fileLocation);

            var converter = new HtmlToPdf
            {
                Options =
                {
                    PdfPageSize = PdfPageSize.A4,
                    PdfPageOrientation = PdfPageOrientation.Portrait
                }
            };
            var doc = converter.ConvertHtmlString(html);
            doc.Save(fileLocation);
            doc.Close();
            return Json(new { filename = fileName });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to export permission PDF. PermissionNumber={PermissionNumber}; TraceId={TraceId}", permissionNumber, HttpContext.TraceIdentifier);
            return StatusCode(StatusCodes.Status500InternalServerError, new { filename = string.Empty });
        }
    }

    private bool TryReadUser(out long userId) =>
        long.TryParse(HttpContext.Session.GetString("UserId"), NumberStyles.Integer, CultureInfo.InvariantCulture, out userId);
}
