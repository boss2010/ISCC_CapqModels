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
        if (model.PermissionRequestId <= 0)
            return BadRequest("بيانات التعديل غير مكتملة.");

        const string sql = """
            UPDATE permissionItem
               SET permissionItem.Im_Initiator_ID = targetInitiator.ID,
                   permissionItem.Item_Permission_Number=@ItemPermissionNumber,
                   permissionItem.Package_Type_ID=@PackageTypeId,
                   permissionItem.Package_Material_ID=@PackageMaterialId,
                   permissionItem.Order_Text=@OrderText
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
              AND permissionRequest.ID = @PermissionRequestId
              AND (@PackageTypeId IS NULL OR EXISTS(SELECT 1 FROM dbo.Package_Type WHERE ID=@PackageTypeId AND IsActive=1))
              AND (@PackageMaterialId IS NULL OR EXISTS(SELECT 1 FROM dbo.Package_Material WHERE ID=@PackageMaterialId AND IsActive=1));

            SELECT @@ROWCOUNT;
            """;

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await using var transaction = await connection.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            const string headerSql="""
            UPDATE dbo.Im_PermissionRequest SET Arrival_Date=@ArrivalDate,Start_Date=@StartDate,End_Date=@EndDate,User_Updation_Id=@UserId,User_Updation_Date=GETDATE() WHERE ID=@PermissionRequestId;
            UPDATE dbo.Im_RequestData SET Ship_Name=@ShipName,DelegateName=@DelegateName,DelegateAddress=@DelegateAddress,ExportCountry_Id=@ExportCountryId,Im_OperationType=@OperationTypeId,Shipment_Mean_Id=@ShipmentMeanId,Transport_Mean_Id=@TransportMeanId,User_Updation_Id=@UserId,User_Updation_Date=GETDATE() WHERE Im_PermissionRequest_ID=@PermissionRequestId AND User_Deletion_Id IS NULL;
            UPDATE port SET Port_ID=CASE port.ReqPortType_ID WHEN 9 THEN @ShippingPortId WHEN 10 THEN @ArrivalPortId ELSE port.Port_ID END,User_Updation_Id=@UserId,User_Updation_Date=GETDATE()
            FROM dbo.Im_Request_Port port JOIN dbo.Im_RequestData data ON data.ID=port.Im_RequestData_ID
            WHERE data.Im_PermissionRequest_ID=@PermissionRequestId AND port.ReqPortType_ID IN(9,10) AND port.User_Deletion_Id IS NULL;
            """;
            await using(var headerCommand=new SqlCommand(headerSql,connection,(SqlTransaction)transaction)){headerCommand.Parameters.Add("@PermissionRequestId",SqlDbType.BigInt).Value=model.PermissionRequestId;headerCommand.Parameters.Add("@ArrivalDate",SqlDbType.Date).Value=model.ArrivalDate?.Date??(object)DBNull.Value;headerCommand.Parameters.Add("@StartDate",SqlDbType.Date).Value=model.StartDate?.Date??(object)DBNull.Value;headerCommand.Parameters.Add("@EndDate",SqlDbType.Date).Value=model.EndDate?.Date??(object)DBNull.Value;headerCommand.Parameters.Add("@ShipName",SqlDbType.NVarChar,500).Value=(object?)model.ShipName?.Trim()??DBNull.Value;headerCommand.Parameters.Add("@DelegateName",SqlDbType.NVarChar,500).Value=(object?)model.DelegateName?.Trim()??DBNull.Value;headerCommand.Parameters.Add("@DelegateAddress",SqlDbType.NVarChar,-1).Value=(object?)model.DelegateAddress?.Trim()??DBNull.Value;headerCommand.Parameters.Add("@ShippingPortId",SqlDbType.BigInt).Value=model.ShippingPortId??(object)DBNull.Value;headerCommand.Parameters.Add("@ArrivalPortId",SqlDbType.BigInt).Value=model.ArrivalPortId??(object)DBNull.Value;headerCommand.Parameters.Add("@ExportCountryId",SqlDbType.SmallInt).Value=model.ExportCountryId??(object)DBNull.Value;headerCommand.Parameters.Add("@OperationTypeId",SqlDbType.Int).Value=model.OperationTypeId??(object)DBNull.Value;headerCommand.Parameters.Add("@ShipmentMeanId",SqlDbType.SmallInt).Value=model.ShipmentMeanId??(object)DBNull.Value;headerCommand.Parameters.Add("@TransportMeanId",SqlDbType.SmallInt).Value=model.TransportMeanId??(object)DBNull.Value;headerCommand.Parameters.Add("@UserId",SqlDbType.SmallInt).Value=userId;await headerCommand.ExecuteNonQueryAsync(cancellationToken);}
            foreach (var item in model.Items.GroupBy(x => x.PermissionItemId).Select(x => x.Last()))
            {
                await using var command = new SqlCommand(sql, connection, (SqlTransaction)transaction);
                command.Parameters.Add("@PermissionRequestId", SqlDbType.BigInt).Value = model.PermissionRequestId;
                command.Parameters.Add("@PermissionItemId", SqlDbType.BigInt).Value = item.PermissionItemId;
                command.Parameters.Add("@InitiatorId", SqlDbType.BigInt).Value = item.InitiatorId;
                command.Parameters.Add("@ItemPermissionNumber",SqlDbType.VarChar,50).Value=(object?)item.ItemPermissionNumber?.Trim()??DBNull.Value;
                command.Parameters.Add("@PackageTypeId",SqlDbType.SmallInt).Value=item.PackageTypeId??(object)DBNull.Value;
                command.Parameters.Add("@PackageMaterialId",SqlDbType.SmallInt).Value=item.PackageMaterialId??(object)DBNull.Value;
                command.Parameters.Add("@OrderText",SqlDbType.NVarChar,-1).Value=(object?)item.OrderText?.Trim()??DBNull.Value;
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
        const string headerSql="""
        SELECT TOP(1) request.ID,data.ID,CONVERT(nvarchar(100),request.ImPermission_Number),request.Arrival_Date,request.Start_Date,request.End_Date,
          data.Ship_Name,data.DelegateName,data.DelegateAddress,country.Ar_Name,operation.Name_Ar,shipment.Ar_Name,transport.Ar_Name,
          data.ExportCountry_Id,data.Im_OperationType,data.Shipment_Mean_Id,data.Transport_Mean_Id
        FROM dbo.Im_PermissionRequest request LEFT JOIN dbo.Im_RequestData data ON data.Im_PermissionRequest_ID=request.ID AND data.User_Deletion_Id IS NULL
        LEFT JOIN dbo.Country country ON country.ID=data.ExportCountry_Id LEFT JOIN dbo.Im_OpertaionType operation ON operation.ID=data.Im_OperationType
        LEFT JOIN dbo.Shipment_Mean shipment ON shipment.ID=data.Shipment_Mean_Id LEFT JOIN dbo.Transport_Mean transport ON transport.ID=data.Transport_Mean_Id
        WHERE request.ImPermission_Number=@PermissionNumber ORDER BY data.ID DESC;
        """;
        const string itemSql = """
            SELECT request.ID,
                   CONVERT(nvarchar(100), request.ImPermission_Number),
                   permissionItem.ID,
                   permissionItem.Im_Initiator_ID,
                   COALESCE(shortName.ShortName_Ar, qualitativeGroup.Name_Ar, item.Name_Ar, N''),
                   COALESCE(item.Scientific_Name, N''),
                   COALESCE(country.Ar_Name, N'كل الدول'),permissionItem.Item_Permission_Number,permissionItem.Package_Type_ID,
                   permissionItem.Package_Material_ID,permissionItem.Order_Text,permissionItem.Package_Count,permissionItem.Package_Weight,
                   permissionItem.Units_Number,permissionItem.Size,permissionItem.GrossWeight
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
        await using(var command=new SqlCommand(headerSql,connection)){command.Parameters.Add("@PermissionNumber",SqlDbType.Decimal).Value=permissionNumber;await using var reader=await command.ExecuteReaderAsync(cancellationToken);if(!await reader.ReadAsync(cancellationToken))return null;model.PermissionRequestId=reader.GetInt64(0);model.RequestDataId=reader.IsDBNull(1)?null:reader.GetInt64(1);model.PermissionNumber=reader.GetString(2);model.ArrivalDate=reader.IsDBNull(3)?null:reader.GetDateTime(3);model.StartDate=reader.IsDBNull(4)?null:reader.GetDateTime(4);model.EndDate=reader.IsDBNull(5)?null:reader.GetDateTime(5);model.ShipName=reader.IsDBNull(6)?"":reader.GetString(6);model.DelegateName=reader.IsDBNull(7)?"":reader.GetString(7);model.DelegateAddress=reader.IsDBNull(8)?"":reader.GetString(8);model.ExportCountry=reader.IsDBNull(9)?"":reader.GetString(9);model.OperationType=reader.IsDBNull(10)?"":reader.GetString(10);model.ShipmentMean=reader.IsDBNull(11)?"":reader.GetString(11);model.TransportMean=reader.IsDBNull(12)?"":reader.GetString(12);model.ExportCountryId=reader.IsDBNull(13)?null:Convert.ToInt16(reader[13]);model.OperationTypeId=reader.IsDBNull(14)?null:Convert.ToInt32(reader[14]);model.ShipmentMeanId=reader.IsDBNull(15)?null:Convert.ToInt16(reader[15]);model.TransportMeanId=reader.IsDBNull(16)?null:Convert.ToInt16(reader[16]);}
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
                    ,ItemPermissionNumber=reader.IsDBNull(7)?"":Convert.ToString(reader[7])??"",PackageTypeId=reader.IsDBNull(8)?null:Convert.ToInt16(reader[8]),PackageMaterialId=reader.IsDBNull(9)?null:Convert.ToInt16(reader[9]),OrderText=reader.IsDBNull(10)?"":Convert.ToString(reader[10])??"",PackageCount=reader.IsDBNull(11)?null:Convert.ToInt32(reader[11]),PackageWeight=reader.IsDBNull(12)?null:Convert.ToDecimal(reader[12]),UnitsNumber=reader.IsDBNull(13)?null:Convert.ToInt32(reader[13]),Size=reader.IsDBNull(14)?null:Convert.ToDouble(reader[14]),GrossWeight=reader.IsDBNull(15)?null:Convert.ToDecimal(reader[15])
                });
            }
        }
        if (model.PermissionRequestId == 0) return null;

        await using(var lookup=new SqlCommand("SELECT ID,COALESCE(NULLIF(Ar_Name,N''),En_Name) Name,1 Kind FROM dbo.Package_Type WHERE IsActive=1 UNION ALL SELECT ID,COALESCE(NULLIF(Ar_Name,N''),En_Name),2 FROM dbo.Package_Material WHERE IsActive=1 ORDER BY Kind,Name",connection)){await using var reader=await lookup.ExecuteReaderAsync(cancellationToken);while(await reader.ReadAsync(cancellationToken)){var entry=new PermissionLookupVm{Id=Convert.ToInt64(reader[0]),Name=Convert.ToString(reader[1])??""};if(Convert.ToInt32(reader[2])==1)model.PackageTypes.Add(entry);else model.PackageMaterials.Add(entry);}}
        if(model.RequestDataId.HasValue)
        {
            await using(var ports=new SqlCommand("SELECT ReqPortType_ID,Port_ID FROM dbo.Im_Request_Port WHERE Im_RequestData_ID=@DataId AND User_Deletion_Id IS NULL; SELECT ID,COALESCE(NULLIF(Name_Ar,N''),Name_En) FROM dbo.Port_International ORDER BY Name_Ar; SELECT ID,COALESCE(NULLIF(Name_Ar,N''),Name_En) FROM dbo.PortNational ORDER BY Name_Ar;",connection)){ports.Parameters.Add("@DataId",SqlDbType.BigInt).Value=model.RequestDataId.Value;await using var reader=await ports.ExecuteReaderAsync(cancellationToken);while(await reader.ReadAsync(cancellationToken)){if(Convert.ToInt32(reader[0])==9)model.ShippingPortId=reader.IsDBNull(1)?null:Convert.ToInt64(reader[1]);else if(Convert.ToInt32(reader[0])==10)model.ArrivalPortId=reader.IsDBNull(1)?null:Convert.ToInt64(reader[1]);}await reader.NextResultAsync(cancellationToken);while(await reader.ReadAsync(cancellationToken))model.ShippingPorts.Add(new(){Id=Convert.ToInt64(reader[0]),Name=Convert.ToString(reader[1])??""});await reader.NextResultAsync(cancellationToken);while(await reader.ReadAsync(cancellationToken))model.ArrivalPorts.Add(new(){Id=Convert.ToInt64(reader[0]),Name=Convert.ToString(reader[1])??""});}
        }
        await using(var options=new SqlCommand("SELECT ID,Ar_Name,1 Kind FROM dbo.Country WHERE IsActive=1 UNION ALL SELECT ID,Name_Ar,2 FROM dbo.Im_OpertaionType UNION ALL SELECT ID,Ar_Name,3 FROM dbo.Shipment_Mean WHERE IsActive=1 UNION ALL SELECT ID,Ar_Name,4 FROM dbo.Transport_Mean WHERE IsActive=1 ORDER BY Kind,2",connection)){await using var reader=await options.ExecuteReaderAsync(cancellationToken);while(await reader.ReadAsync(cancellationToken)){var option=new PermissionLookupVm{Id=Convert.ToInt64(reader[0]),Name=Convert.ToString(reader[1])??""};switch(Convert.ToInt32(reader[2])){case 1:model.Countries.Add(option);break;case 2:model.OperationTypes.Add(option);break;case 3:model.ShipmentMeans.Add(option);break;case 4:model.TransportMeans.Add(option);break;}}}

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
