using System.Data;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using PlantMobile.Data;
using ViewModels;

namespace PlantMobile.Controllers;

[Route("Im_Permissions/OriginChange")]
public sealed class PermissionOriginChangeController : Controller
{
    private readonly string _connectionString;
    private readonly MenuAccessReader _accessReader;
    private readonly ILogger<PermissionOriginChangeController> _logger;

    public PermissionOriginChangeController(
        IConfiguration configuration,
        MenuAccessReader accessReader,
        ILogger<PermissionOriginChangeController> logger)
    {
        _connectionString = configuration.GetConnectionString("DBConnection")
            ?? throw new InvalidOperationException("DBConnection is not configured.");
        _accessReader = accessReader;
        _logger = logger;
    }

    [HttpGet("")]
    [HttpGet("Index")]
    public async Task<IActionResult> Index(
        string? ImPermission_Number,
        string? returnUrl,
        CancellationToken cancellationToken)
    {
        if (!TryCurrentUser(out short userId)) return RedirectToAction("Index", "Login");
        if (!await CanEditAsync(userId, cancellationToken).ConfigureAwait(false)) return Forbid();

        if (string.IsNullOrWhiteSpace(ImPermission_Number))
            return View(new PermissionOriginChangePageVm());
        if (!decimal.TryParse(ImPermission_Number.Trim(), NumberStyles.None, CultureInfo.InvariantCulture, out decimal number))
        {
            ModelState.AddModelError(string.Empty, "رقم إذن الاستيراد غير صحيح.");
            return View(new PermissionOriginChangePageVm { PermissionNumber = ImPermission_Number.Trim() });
        }

        try
        {
            var model = await LoadAsync(number, cancellationToken).ConfigureAwait(false);
            if (model is null) return NotFound("لم يتم العثور على إذن الاستيراد.");
            model.ReturnUrl = LocalReturnUrl(returnUrl, model.PermissionNumber);
            return View(model);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to load origin editor for permission {PermissionNumber}", ImPermission_Number);
            ModelState.AddModelError(string.Empty, $"تعذر تحميل بيانات الإذن. رقم التتبع: {HttpContext.TraceIdentifier}");
            return View(new PermissionOriginChangePageVm { PermissionNumber = ImPermission_Number.Trim() });
        }
    }

    [HttpPost("Save")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Save(
        PermissionOriginChangeSaveVm model,
        CancellationToken cancellationToken)
    {
        if (!TryCurrentUser(out short userId)) return RedirectToAction("Index", "Login");
        if (!await CanEditAsync(userId, cancellationToken).ConfigureAwait(false)) return Forbid();
        if (model.PermissionRequestId <= 0 || model.Items.Count == 0)
            return BadRequest("بيانات التعديل غير مكتملة.");

        const string sql = """
            UPDATE permissionItem
               SET permissionItem.Im_Initiator_ID = targetInitiator.ID
            FROM dbo.Im_PermissionItems permissionItem
            INNER JOIN dbo.Im_PermissionRequest permissionRequest
                ON permissionRequest.ID = permissionItem.Im_PermissionRequest_ID
            INNER JOIN dbo.Im_Initiator currentInitiator
                ON currentInitiator.ID = permissionItem.Im_Initiator_ID
            INNER JOIN dbo.Im_Initiator targetInitiator
                ON targetInitiator.ID = @InitiatorId
               AND targetInitiator.IsActive = 1
               AND targetInitiator.User_Deletion_Id IS NULL
               AND targetInitiator.Initiator_Status = currentInitiator.Initiator_Status
               AND
               (
                   (currentInitiator.Item_ShortName_ID IS NOT NULL
                    AND targetInitiator.Item_ShortName_ID = currentInitiator.Item_ShortName_ID)
                   OR
                   (currentInitiator.QualitativeGroup_Id IS NOT NULL
                    AND targetInitiator.QualitativeGroup_Id = currentInitiator.QualitativeGroup_Id)
               )
            WHERE permissionItem.ID = @PermissionItemId
              AND permissionRequest.ID = @PermissionRequestId;

            SELECT @@ROWCOUNT;
            """;

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            foreach (var item in model.Items.GroupBy(x => x.PermissionItemId).Select(x => x.Last()))
            {
                await using var command = new SqlCommand(sql, connection, (SqlTransaction)transaction);
                command.Parameters.Add("@PermissionRequestId", SqlDbType.BigInt).Value = model.PermissionRequestId;
                command.Parameters.Add("@PermissionItemId", SqlDbType.BigInt).Value = item.PermissionItemId;
                command.Parameters.Add("@InitiatorId", SqlDbType.BigInt).Value = item.InitiatorId;
                int affected = Convert.ToInt32(await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false));
                if (affected != 1)
                    throw new InvalidOperationException("أحد المناشئ المختارة لا يتوافق مع الصنف أو تم تغيير بيانات الإذن.");
            }

            await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
            TempData["Message"] = $"تم تعديل منشأ إذن الاستيراد {model.PermissionNumber} بنجاح.";
            return LocalRedirect(LocalReturnUrl(model.ReturnUrl, model.PermissionNumber));
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
            _logger.LogError(ex, "Failed to update origins for permission {PermissionId}", model.PermissionRequestId);
            TempData["OriginError"] = ex is InvalidOperationException
                ? ex.Message
                : $"تعذر حفظ التعديل. رقم التتبع: {HttpContext.TraceIdentifier}";
            return RedirectToAction(nameof(Index), new
            {
                ImPermission_Number = model.PermissionNumber,
                returnUrl = model.ReturnUrl
            });
        }
    }

    private async Task<PermissionOriginChangePageVm?> LoadAsync(
        decimal permissionNumber,
        CancellationToken cancellationToken)
    {
        const string itemSql = """
            SELECT request.ID,
                   CONVERT(nvarchar(100), request.ImPermission_Number),
                   permissionItem.ID,
                   permissionItem.Im_Initiator_ID,
                   COALESCE(shortName.ShortName_Ar, qualitativeGroup.Name_Ar, item.Name_Ar, N''),
                   COALESCE(item.Scientific_Name, N''),
                   COALESCE(country.Ar_Name, N'كل الدول')
            FROM dbo.Im_PermissionRequest request
            INNER JOIN dbo.Im_PermissionItems permissionItem
                ON permissionItem.Im_PermissionRequest_ID = request.ID
            LEFT JOIN dbo.Im_Initiator initiator ON initiator.ID = permissionItem.Im_Initiator_ID
            LEFT JOIN dbo.Item_ShortName shortName ON shortName.ID = initiator.Item_ShortName_ID
            LEFT JOIN dbo.Item item ON item.ID = shortName.Item_ID
            LEFT JOIN dbo.QualitativeGroup qualitativeGroup ON qualitativeGroup.ID = initiator.QualitativeGroup_Id
            LEFT JOIN dbo.Country country ON country.ID = initiator.Country_Id
            WHERE request.ImPermission_Number = @PermissionNumber
            ORDER BY permissionItem.ID;
            """;
        const string originSql = """
            WITH AvailableOrigins AS
            (
                SELECT candidate.ID, candidate.Country_Id, country.Ar_Name,
                       ROW_NUMBER() OVER (PARTITION BY candidate.Country_Id ORDER BY candidate.ID DESC) AS RowNumber
                FROM dbo.Im_Initiator currentInitiator
                INNER JOIN dbo.Im_Initiator candidate
                    ON candidate.IsActive = 1
                   AND candidate.User_Deletion_Id IS NULL
                   AND candidate.Initiator_Status = currentInitiator.Initiator_Status
                   AND
                   (
                       (currentInitiator.Item_ShortName_ID IS NOT NULL
                        AND candidate.Item_ShortName_ID = currentInitiator.Item_ShortName_ID)
                       OR
                       (currentInitiator.QualitativeGroup_Id IS NOT NULL
                        AND candidate.QualitativeGroup_Id = currentInitiator.QualitativeGroup_Id)
                   )
                INNER JOIN dbo.Country country ON country.ID = candidate.Country_Id
                WHERE currentInitiator.ID = @CurrentInitiatorId
            )
            SELECT ID, Country_Id, Ar_Name
            FROM AvailableOrigins
            WHERE RowNumber = 1
            ORDER BY Ar_Name;
            """;

        var model = new PermissionOriginChangePageVm();
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await using (var command = new SqlCommand(itemSql, connection) { CommandTimeout = 30 })
        {
            command.Parameters.Add("@PermissionNumber", SqlDbType.Decimal).Value = permissionNumber;
            await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                model.PermissionRequestId = Convert.ToInt64(reader.GetValue(0));
                model.PermissionNumber = reader.GetString(1);
                model.Items.Add(new PermissionOriginChangeItemVm
                {
                    PermissionItemId = Convert.ToInt64(reader.GetValue(2)),
                    SelectedInitiatorId = reader.IsDBNull(3) ? null : Convert.ToInt64(reader.GetValue(3)),
                    ItemName = reader.GetString(4),
                    ScientificName = reader.GetString(5),
                    CurrentCountry = reader.GetString(6)
                });
            }
        }
        if (model.PermissionRequestId == 0) return null;

        foreach (var item in model.Items.Where(x => x.SelectedInitiatorId.HasValue))
        {
            await using var command = new SqlCommand(originSql, connection) { CommandTimeout = 30 };
            command.Parameters.Add("@CurrentInitiatorId", SqlDbType.BigInt).Value = item.SelectedInitiatorId!.Value;
            await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                item.Origins.Add(new PermissionOriginOptionVm
                {
                    InitiatorId = Convert.ToInt64(reader.GetValue(0)),
                    CountryId = Convert.ToInt16(reader.GetValue(1)),
                    CountryName = reader.GetString(2)
                });
            }
        }
        return model;
    }

    private bool TryCurrentUser(out short userId)
    {
        userId = 0;
        return HttpContext.Session.GetString("UserSession") == "Authenticated"
            && short.TryParse(HttpContext.Session.GetString("UserId"), out userId);
    }

    private Task<bool> CanEditAsync(short userId, CancellationToken cancellationToken) =>
        _accessReader.CanViewAsync(userId, MenuAccessReader.PermissionOriginChangeMenuUrl, cancellationToken);

    private string LocalReturnUrl(string? returnUrl, string permissionNumber)
    {
        if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl)) return returnUrl;
        return Url.Action("Index", "Im_permissionDetail", new { ImPermission_Number = permissionNumber }) ?? "/";
    }
}
