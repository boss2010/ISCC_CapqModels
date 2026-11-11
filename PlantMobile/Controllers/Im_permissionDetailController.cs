using System.Data;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using PlantMobile.Infrastructure;
using ViewModels;

namespace PlantMobile.Controllers;

[Route("Im_Permissions/Im_permissionDetail")]
public class Im_permissionDetailController : Controller
{
    private readonly string _connectionString;
    private readonly string _attachmentServer;
    private readonly IConfiguration _configuration;
    private readonly ILogger<Im_permissionDetailController> _logger;

    public Im_permissionDetailController(
        IConfiguration configuration,
        ILogger<Im_permissionDetailController> logger)
    {
        _configuration = configuration;
        _connectionString = configuration.GetConnectionString("DBConnection")
            ?? throw new InvalidOperationException("DBConnection is not configured.");
        _attachmentServer = configuration["Path_NetworkShare"] is { Length: > 0 } share
            ? @"\\" + share.Trim().Trim('\\', '/')
            : string.Empty;
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

        var header = await ReadHeaderAsync(connection, model, permissionNumber);
        if (header is null)
        {
            model.ErrorMessage = $"لم يتم العثور على إذن استيراد برقم {model.PermissionNumber}.";
            return;
        }

        await ReadPortsAsync(connection, model, header.RequestDataId);
        await ReadImporterAsync(connection, model, header.ImporterTypeId, header.ImporterId);
        await ReadContactsAsync(connection, model, header.ImporterTypeId, header.ImporterId);
        await ReadCompanyActivitiesAsync(connection, model, header.ImporterId);
        await ReadImporterCompaniesAsync(connection, model, header.RequestDataId);

        var itemKeys = await ReadItemsAsync(connection, model, header.PermissionRequestId);
        await ReadConstrainsAsync(connection, model, itemKeys);

        await ReadAttachmentsAsync(connection, model, header.PermissionRequestId);
        await ReadRefuseReasonsAsync(connection, model, header.PermissionRequestId);
    }

    private sealed record HeaderInfo(long PermissionRequestId, long RequestDataId, long ImporterId, int ImporterTypeId);

    private sealed record ItemKeys(long ItemId, long? InitiatorId, short? CountryId, long? ShortNameId, short? QualitativeGroupId);

    private static async Task<HeaderInfo?> ReadHeaderAsync(SqlConnection connection, ImPermissionDetailVm model, decimal permissionNumber)
    {
        const string sql = """
            SELECT TOP (1)
                   cc.ID,
                   rr.ID,
                   CONVERT(nvarchar(100), cc.ImPermission_Number),
                   cc.Arrival_Date,
                   cc.Start_Date,
                   cc.End_Date,
                   cc.IsAcceppted,
                   cc.IsPaid,
                   cc.Renewal_Status,
                   rr.Importer_ID,
                   rr.ImporterType_Id,
                   COALESCE(country.Ar_Name, N''),
                   COALESCE(country.En_Name, N''),
                   COALESCE(operation.Name_Ar, N''),
                   COALESCE(shipMean.Ar_Name, N''),
                   COALESCE(shipMean.En_Name, N''),
                   COALESCE(transportMean.Ar_Name, N''),
                   COALESCE(transportMean.En_Name, N''),
                   COALESCE(rr.Ship_Name, N''),
                   COALESCE(rr.DelegateName, N''),
                   COALESCE(rr.DelegateAddress, N'')
            FROM dbo.Im_PermissionRequest cc
            INNER JOIN dbo.Im_RequestData rr ON rr.Im_PermissionRequest_ID = cc.ID
            LEFT JOIN dbo.Country country ON country.ID = rr.ExportCountry_Id
            LEFT JOIN dbo.Im_OpertaionType operation ON operation.ID = rr.Im_OperationType
            LEFT JOIN dbo.Shipment_Mean shipMean ON shipMean.ID = rr.Shipment_Mean_Id
            LEFT JOIN dbo.Transport_Mean transportMean ON transportMean.ID = rr.Transport_Mean_Id
            WHERE cc.ImPermission_Number = @PermissionNumber
            ORDER BY cc.ID DESC;
            """;

        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 30 };
        command.Parameters.Add("@PermissionNumber", SqlDbType.Decimal).Value = permissionNumber;
        await using var reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
            return null;

        long permissionRequestId = Convert.ToInt64(reader.GetValue(0));
        long requestDataId = Convert.ToInt64(reader.GetValue(1));
        model.Id = permissionRequestId;
        model.RequestDataId = requestDataId;
        model.PermissionNumber = DbString(reader, 2);
        model.ArrivalDate = ReadNullableDate(reader, 3);
        model.StartDate = ReadNullableDate(reader, 4);
        model.EndDate = ReadNullableDate(reader, 5);
        model.IsAccepted = reader.IsDBNull(6) ? null : Convert.ToBoolean(reader.GetValue(6));
        model.IsPaid = reader.IsDBNull(7) ? null : Convert.ToBoolean(reader.GetValue(7));
        model.RenewalStatus = reader.IsDBNull(8) ? null : Convert.ToByte(reader.GetValue(8));
        long importerId = Convert.ToInt64(reader.GetValue(9));
        int importerTypeId = Convert.ToInt32(reader.GetValue(10));
        model.ImporterTypeId = importerTypeId;
        model.ImporterType = importerTypeId switch { 6 => "شركة", 7 => "هيئة", _ => "فرد" };
        model.Country = DbString(reader, 11);
        model.CountryEn = DbString(reader, 12);
        model.OperationType = DbString(reader, 13);
        model.ShipmentMeanName = DbString(reader, 14);
        model.ShipmentMeanNameEn = DbString(reader, 15);
        model.TransportMeanName = DbString(reader, 16);
        model.TransportMeanNameEn = DbString(reader, 17);
        model.ShipName = DbString(reader, 18);
        model.DelegateName = DbString(reader, 19);
        model.DelegateAddress = DbString(reader, 20);
        return new HeaderInfo(permissionRequestId, requestDataId, importerId, importerTypeId);
    }

    private static async Task ReadPortsAsync(SqlConnection connection, ImPermissionDetailVm model, long requestDataId)
    {
        const string sql = """
            SELECT p.ReqPortType_ID,
                   COALESCE(pn.Name_Ar, pi.Name_Ar, N''),
                   COALESCE(pn.Name_En, pi.Name_En, N''),
                   COALESCE(pt.Name_Ar, N''),
                   COALESCE(pt.Name_En, N'')
            FROM dbo.Im_Request_Port p
            LEFT JOIN dbo.PortNational pn ON pn.ID = p.Port_ID AND p.ReqPortType_ID = 10
            LEFT JOIN dbo.Port_International pi ON pi.ID = p.Port_ID AND p.ReqPortType_ID = 9
            LEFT JOIN dbo.Port_Type pt ON pt.ID = p.Port_Type_ID
            WHERE p.Im_RequestData_ID = @RequestDataId AND p.User_Deletion_Id IS NULL
            ORDER BY p.ID;
            """;

        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 30 };
        command.Parameters.Add("@RequestDataId", SqlDbType.BigInt).Value = requestDataId;
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            int portKind = Convert.ToInt32(reader.GetValue(0));
            if (portKind == 10)
            {
                model.ArrivePortName = DbString(reader, 1);
                model.ArrivePortNameType = DbString(reader, 3);
            }
            else if (portKind == 9)
            {
                model.TransportPortName = DbString(reader, 1);
                model.TransportPortNameType = DbString(reader, 3);
            }
        }
    }

    private static async Task ReadImporterAsync(SqlConnection connection, ImPermissionDetailVm model, int importerTypeId, long importerId)
    {
        string sql = importerTypeId switch
        {
            6 => "SELECT TOP (1) COALESCE(Name_Ar, N''), COALESCE(Name_En, N''), COALESCE(Address_Ar, N''), COALESCE(Address_En, N'') FROM dbo.Company_National WHERE ID = @ImporterId;",
            7 => "SELECT TOP (1) COALESCE(Name_Ar, N''), COALESCE(Name_En, N''), COALESCE(Address_Ar, N''), COALESCE(Address_En, N'') FROM dbo.Public_Organization WHERE ID = @ImporterId;",
            _ => "SELECT TOP (1) COALESCE(Name, N''), COALESCE(Name_EN, N''), COALESCE(Address, N''), COALESCE(Address_EN, N''), COALESCE(IDNumber, N'') FROM dbo.Person WHERE ID = @ImporterId;"
        };

        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 30 };
        command.Parameters.Add("@ImporterId", SqlDbType.BigInt).Value = importerId;
        await using var reader = await command.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            model.ImporterName = DbString(reader, 0);
            model.ImporterNameEn = DbString(reader, 1);
            model.ImporterAddress = DbString(reader, 2);
            model.ImporterAddressEn = DbString(reader, 3);
            if (importerTypeId is not (6 or 7))
                model.PersonIdNo = DbString(reader, 4);
        }

        model.Company = string.IsNullOrWhiteSpace(model.ImporterName) ? model.ImporterNameEn : model.ImporterName;
    }

    private static async Task ReadContactsAsync(SqlConnection connection, ImPermissionDetailVm model, int importerTypeId, long importerId)
    {
        if (importerTypeId is 6 or 7)
        {
            const string sql = """
                SELECT COALESCE(ct.Name_Ar, N''), COALESCE(e.Value, N'')
                FROM dbo.Ex_ContactData e
                LEFT JOIN dbo.ContactType ct ON ct.ID = e.ContactType_ID
                WHERE e.Exporter_ID = @ImporterId AND e.ExporterType_Id = @ExporterType
                ORDER BY e.ID;
                """;
            await using var command = new SqlCommand(sql, connection) { CommandTimeout = 30 };
            command.Parameters.Add("@ImporterId", SqlDbType.BigInt).Value = importerId;
            command.Parameters.Add("@ExporterType", SqlDbType.Int).Value = importerTypeId;
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                model.Contacts.Add(new ImPermissionDetailContactVm
                {
                    TypeName = DbString(reader, 0),
                    Value = DbString(reader, 1)
                });
            }
            return;
        }

        const string personSql = "SELECT COALESCE(Email, N''), COALESCE(Phone, N'') FROM dbo.Person WHERE ID = @ImporterId;";
        await using var personCommand = new SqlCommand(personSql, connection) { CommandTimeout = 30 };
        personCommand.Parameters.Add("@ImporterId", SqlDbType.BigInt).Value = importerId;
        await using var personReader = await personCommand.ExecuteReaderAsync();
        if (await personReader.ReadAsync())
        {
            string email = DbString(personReader, 0);
            string phone = DbString(personReader, 1);
            if (!string.IsNullOrWhiteSpace(email) || !string.IsNullOrWhiteSpace(phone))
                model.Contacts.Add(new ImPermissionDetailContactVm { TypeName = email, Value = phone });
        }
    }

    private static async Task ReadCompanyActivitiesAsync(SqlConnection connection, ImPermissionDetailVm model, long importerId)
    {
        const string sql = """
            SELECT COALESCE(sys.ValueName, N''),
                   COALESCE(ca.Enrollment_Number, N''),
                   COALESCE(et.Ar_Name, N''),
                   ca.Enrollment_Start,
                   ca.Enrollment_End
            FROM dbo.CompanyActivity ca
            LEFT JOIN dbo.A_SystemCode sys ON sys.Id = ca.MainActivityType
            LEFT JOIN dbo.Enrollment_type et ON et.ID = ca.Enrollment_type_ID
            WHERE ca.Company_ID = @CompanyId AND ca.IsActive = 1
            ORDER BY ca.ID;
            """;

        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 30 };
        command.Parameters.Add("@CompanyId", SqlDbType.BigInt).Value = importerId;
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            model.CompanyActivities.Add(new ImPermissionDetailActivityVm
            {
                ActivityType = DbString(reader, 0),
                EnrollmentNumber = DbString(reader, 1),
                EnrollmentType = DbString(reader, 2),
                EnrollmentStart = ReadDateText(reader, 3),
                EnrollmentEnd = ReadDateText(reader, 4)
            });
        }
    }

    private static async Task ReadImporterCompaniesAsync(SqlConnection connection, ImPermissionDetailVm model, long requestDataId)
    {
        const string sql = """
            SELECT COALESCE(ImportCompany, N''), COALESCE(ImportCompany_EN, N''),
                   COALESCE(ImporeterCompanyAddress, N''), COALESCE(ImporeterCompanyAddress_EN, N'')
            FROM dbo.Im_RequestDat_Extra
            WHERE Im_RequestData_ID = @RequestDataId
            ORDER BY ID;
            """;

        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 30 };
        command.Parameters.Add("@RequestDataId", SqlDbType.BigInt).Value = requestDataId;
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            model.ImporterCompanies.Add(new ImPermissionDetailImporterCompanyVm
            {
                Company = DbString(reader, 0),
                CompanyEn = DbString(reader, 1),
                Address = DbString(reader, 2),
                AddressEn = DbString(reader, 3)
            });
        }
    }

    private static async Task<List<ItemKeys>> ReadItemsAsync(SqlConnection connection, ImPermissionDetailVm model, long permissionRequestId)
    {
        const string sql = """
            SELECT p.ID,
                   COALESCE(p.Item_Permission_Number, N''),
                   p.Size, p.Package_Count, p.Package_Weight, p.Units_Number, p.GrossWeight,
                   COALESCE(p.Order_Text, N''),
                   p.IsAccepted,
                   COALESCE(ptype.Ar_Name, N''), COALESCE(ptype.En_Name, N''),
                   COALESCE(pmat.Ar_Name, N''), COALESCE(pmat.En_Name, N''),
                   i.ID, i.Country_Id, i.Item_ShortName_ID, i.QualitativeGroup_Id,
                   COALESCE(qg.Name_Ar, N''), COALESCE(qg.Name_En, N''),
                   COALESCE(sn.ShortName_Ar, N''), COALESCE(sn.ShortName_En, N''),
                   COALESCE(st.Ar_Name, N''), COALESCE(st.En_Name, N''),
                   COALESCE(pp.Ar_Name, N''), COALESCE(pp.En_Name, N''),
                   COALESCE(sp.Name_Ar, N''), COALESCE(sp.Name_En, N''),
                   COALESCE(it.Name_Ar, N''), COALESCE(it.Name_En, N''), COALESCE(it.Scientific_Name, N''),
                   COALESCE(ctry.Ar_Name, N''), COALESCE(ctry.En_Name, N'')
            FROM dbo.Im_PermissionItems p
            LEFT JOIN dbo.Package_Type ptype ON ptype.ID = p.Package_Type_ID
            LEFT JOIN dbo.Package_Material pmat ON pmat.ID = p.Package_Material_ID
            LEFT JOIN dbo.Im_Initiator i ON i.ID = p.Im_Initiator_ID
            LEFT JOIN dbo.QualitativeGroup qg ON qg.Id = i.QualitativeGroup_Id
            LEFT JOIN dbo.Item_ShortName sn ON sn.ID = i.Item_ShortName_ID
            LEFT JOIN dbo.Item_Status st ON st.ID = sn.Item_Status_ID
            LEFT JOIN dbo.Item_Purpose pp ON pp.ID = sn.Item_Purpose_ID
            LEFT JOIN dbo.SubPart sp ON sp.ID = sn.SubPart_ID
            LEFT JOIN dbo.Item it ON it.ID = sn.Item_ID
            LEFT JOIN dbo.Country ctry ON ctry.ID = i.Country_Id
            WHERE p.Im_PermissionRequest_ID = @PermissionRequestId
            ORDER BY p.ID;
            """;

        var keys = new List<ItemKeys>();
        await using (var command = new SqlCommand(sql, connection) { CommandTimeout = 30 })
        {
            command.Parameters.Add("@PermissionRequestId", SqlDbType.BigInt).Value = permissionRequestId;
            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                long itemId = Convert.ToInt64(reader.GetValue(0));
                long? initiatorId = reader.IsDBNull(13) ? null : Convert.ToInt64(reader.GetValue(13));
                short? countryId = reader.IsDBNull(14) ? null : Convert.ToInt16(reader.GetValue(14));
                long? shortNameId = reader.IsDBNull(15) ? null : Convert.ToInt64(reader.GetValue(15));
                short? qualitativeGroupId = reader.IsDBNull(16) ? null : Convert.ToInt16(reader.GetValue(16));

                string originCountry = DbString(reader, 30);
                string originCountryEn = DbString(reader, 31);
                if (!countryId.HasValue)
                {
                    originCountry = "كل الدول";
                    originCountryEn = "All Country";
                }

                var item = new ImPermissionDetailItemVm
                {
                    Id = itemId,
                    ItemPermissionNumber = DbString(reader, 1),
                    Size = reader.IsDBNull(2) ? null : Convert.ToDouble(reader.GetValue(2)),
                    PackageCount = reader.IsDBNull(3) ? null : Convert.ToInt32(reader.GetValue(3)),
                    PackageWeight = reader.IsDBNull(4) ? null : Convert.ToDecimal(reader.GetValue(4)),
                    UnitsNumber = reader.IsDBNull(5) ? null : Convert.ToInt32(reader.GetValue(5)),
                    GrossWeight = reader.IsDBNull(6) ? null : Convert.ToDecimal(reader.GetValue(6)),
                    OrderText = DbString(reader, 7),
                    IsAccepted = !reader.IsDBNull(8) && Convert.ToBoolean(reader.GetValue(8)),
                    PackageType = DbString(reader, 9),
                    PackageMaterial = DbString(reader, 11),
                    QualitativeGroup = DbString(reader, 17),
                    ShortName = DbString(reader, 19),
                    ShortNameEn = DbString(reader, 20),
                    Status = DbString(reader, 21),
                    Purpose = DbString(reader, 23),
                    SubPartName = DbString(reader, 25),
                    ItemName = DbString(reader, 27),
                    ItemNameEn = DbString(reader, 28),
                    ScientificName = DbString(reader, 29),
                    OriginCountry = originCountry,
                    OriginCountryEn = originCountryEn
                };

                model.Items.Add(item);
                keys.Add(new ItemKeys(itemId, initiatorId, countryId, shortNameId, qualitativeGroupId));
            }
        }

        foreach (var item in model.Items)
            await ReadCategoriesAsync(connection, item);

        return keys;
    }

    private static async Task ReadCategoriesAsync(SqlConnection connection, ImPermissionDetailItemVm item)
    {
        const string sql = """
            SELECT c.ItemCategory_ID,
                   c.ItemCategoryGroup_ID,
                   c.Size, c.Package_Count, c.Package_Weight, c.Units_Number, c.GrossWeight,
                   COALESCE(c.Order_Text, N''),
                   COALESCE(c.Reason_Entry, N''),
                   COALESCE(ptype.Ar_Name, N''),
                   COALESCE(pmat.Ar_Name, N''),
                   COALESCE(ic.Name_Ar, N''),
                   ic.IsRegister,
                   COALESCE(icg1.Name_Ar, N''),
                   COALESCE(icg2.Name_Ar, N'')
            FROM dbo.Im_PermissionItems_Category c
            LEFT JOIN dbo.Package_Type ptype ON ptype.ID = c.Package_Type_ID
            LEFT JOIN dbo.Package_Material pmat ON pmat.ID = c.Package_Material_ID
            LEFT JOIN dbo.ItemCategories ic ON ic.ID = c.ItemCategory_ID
            LEFT JOIN dbo.ItemCategories_Group icg1 ON icg1.ID = ic.ItemCategories_Group_ID
            LEFT JOIN dbo.ItemCategories_Group icg2 ON icg2.ID = c.ItemCategoryGroup_ID
            WHERE c.Im_PermissionItems_ID = @PermissionItemId
            ORDER BY c.ID;
            """;

        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 30 };
        command.Parameters.Add("@PermissionItemId", SqlDbType.BigInt).Value = item.Id;
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            bool hasCategory = !reader.IsDBNull(0);
            item.Categories.Add(new ImPermissionDetailCategoryVm
            {
                CategoryName = hasCategory ? DbString(reader, 11) : DbString(reader, 14),
                ItemCatGroup = hasCategory ? DbString(reader, 13) : string.Empty,
                Size = reader.IsDBNull(2) ? null : Convert.ToDouble(reader.GetValue(2)),
                PackageCount = reader.IsDBNull(3) ? null : Convert.ToInt32(reader.GetValue(3)),
                PackageWeight = reader.IsDBNull(4) ? null : Convert.ToDecimal(reader.GetValue(4)),
                UnitsNumber = reader.IsDBNull(5) ? null : Convert.ToInt32(reader.GetValue(5)),
                GrossWeight = reader.IsDBNull(6) ? null : Convert.ToDecimal(reader.GetValue(6)),
                OrderText = DbString(reader, 7),
                RegisterReason = hasCategory ? DbString(reader, 8) : string.Empty,
                PackageType = DbString(reader, 9),
                PackageMaterial = DbString(reader, 10),
                IsRegister = hasCategory && !reader.IsDBNull(12) ? Convert.ToBoolean(reader.GetValue(12)) : null
            });
        }
    }

    private static async Task ReadConstrainsAsync(SqlConnection connection, ImPermissionDetailVm model, List<ItemKeys> keys)
    {
        foreach (var key in keys)
        {
            var item = model.Items.FirstOrDefault(x => x.Id == key.ItemId);
            if (item is null || !key.InitiatorId.HasValue)
                continue;

            var constrain = new ImPermissionDetailConstrainVm
            {
                Country = string.IsNullOrWhiteSpace(item.OriginCountry) ? "كل الدول" : item.OriginCountry
            };

            const string textSql = """
                SELECT COALESCE(t.InSide_Certificate_Ar, N'')
                FROM dbo.Im_Constrain_Initiator_Text c
                INNER JOIN dbo.Im_CountryConstrain_Text t ON t.ID = c.ConstrainText_ID
                WHERE c.Im_Initiator_ID = @InitiatorId AND c.IsActive = 1
                ORDER BY c.ID;
                """;
            await using (var command = new SqlCommand(textSql, connection) { CommandTimeout = 30 })
            {
                command.Parameters.Add("@InitiatorId", SqlDbType.BigInt).Value = key.InitiatorId.Value;
                await using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    string text = DbString(reader, 0);
                    if (!string.IsNullOrWhiteSpace(text))
                        constrain.Texts.Add(text);
                }
            }

            if (key.QualitativeGroupId.HasValue || key.ShortNameId.HasValue)
            {
                string portSql = key.QualitativeGroupId.HasValue
                    ? """
                        SELECT COALESCE(pt.Name_Ar, N''), COALESCE(pt.Name_En, N''), COALESCE(pn.Name_Ar, N''), COALESCE(pn.Name_En, N'')
                        FROM dbo.Im_CountryConstrain_ArrivalPort p
                        LEFT JOIN dbo.Port_Type pt ON pt.ID = p.Port_Type_ID
                        LEFT JOIN dbo.PortNational pn ON pn.ID = p.Port_National_Id
                        WHERE p.IsActive = 1 AND p.Id_QualitativeGroup = @GroupId
                        ORDER BY p.Id;
                        """
                    : """
                        SELECT COALESCE(pt.Name_Ar, N''), COALESCE(pt.Name_En, N''), COALESCE(pn.Name_Ar, N''), COALESCE(pn.Name_En, N'')
                        FROM dbo.Im_CountryConstrain_ArrivalPort p
                        LEFT JOIN dbo.Port_Type pt ON pt.ID = p.Port_Type_ID
                        LEFT JOIN dbo.PortNational pn ON pn.ID = p.Port_National_Id
                        WHERE p.IsActive = 1 AND p.Item_ShortName_ID = @ShortNameId
                        ORDER BY p.Id;
                        """;
                await using var command = new SqlCommand(portSql, connection) { CommandTimeout = 30 };
                if (key.QualitativeGroupId.HasValue)
                    command.Parameters.Add("@GroupId", SqlDbType.SmallInt).Value = key.QualitativeGroupId.Value;
                else
                    command.Parameters.Add("@ShortNameId", SqlDbType.BigInt).Value = key.ShortNameId!.Value;
                await using var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    constrain.Ports.Add(new ImPermissionDetailPortVm
                    {
                        PortType = DbString(reader, 0),
                        PortTypeEn = DbString(reader, 1),
                        PortName = DbString(reader, 2),
                        PortNameEn = DbString(reader, 3)
                    });
                }
            }

            item.Constrains = constrain;
        }
    }

    private static async Task ReadAttachmentsAsync(SqlConnection connection, ImPermissionDetailVm model, long permissionRequestId)
    {
        const string sql = """
            SELECT a.Id, COALESCE(a.Attachment_TypeName, N''), COALESCE(a.Attachment_Number, N''),
                   a.StartDate, a.EndDate, COALESCE(a.AttachmentPath, N'')
            FROM dbo.A_AttachmentData a
            WHERE a.RowId = @PermissionRequestId
              AND a.A_AttachmentTableNameId = 4
              AND a.User_Deletion_Id IS NULL
            ORDER BY a.Id;
            """;

        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 30 };
        command.Parameters.Add("@PermissionRequestId", SqlDbType.BigInt).Value = permissionRequestId;
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            model.Attachments.Add(new ImPermissionDetailAttachmentVm
            {
                Id = Convert.ToInt64(reader.GetValue(0)),
                TypeName = DbString(reader, 1),
                Number = DbString(reader, 2),
                StartDate = ReadNullableDate(reader, 3),
                EndDate = ReadNullableDate(reader, 4),
                Path = DbString(reader, 5)
            });
        }
    }

    private static async Task ReadRefuseReasonsAsync(SqlConnection connection, ImPermissionDetailVm model, long permissionRequestId)
    {
        const string sql = """
            SELECT COALESCE(r.Name_Ar, N''), COALESCE(r.Name_En, N''), COALESCE(rr.Nots, N'')
            FROM dbo.Im_PermissionRequest_RefuseReason rr
            INNER JOIN dbo.Refuse_Reason r ON r.ID = rr.Refuse_Reason_Id
            WHERE rr.Im_PermissionRequest_Id = @PermissionRequestId
            ORDER BY rr.ID;
            """;

        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 30 };
        command.Parameters.Add("@PermissionRequestId", SqlDbType.BigInt).Value = permissionRequestId;
        await using var reader = await command.ExecuteReaderAsync();
        while (await reader.ReadAsync())
        {
            model.RefuseReasons.Add(new ImPermissionDetailRefuseReasonVm
            {
                NameAr = DbString(reader, 0),
                NameEn = DbString(reader, 1),
                Note = DbString(reader, 2)
            });
        }
    }

    [HttpGet("ViewAttachment")]
    public async Task<IActionResult> ViewAttachment(long id)
    {
        if (HttpContext.Session.GetString("UserSession") != "Authenticated")
            return RedirectToAction("Index", "Login");

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync();
        const string sql = """
            SELECT attachment.AttachmentPath_Binary, attachment.AttachmentPath,
                   attachment.Attachment_TypeName, attachment.Attachment_Number
            FROM dbo.A_AttachmentData attachment
            WHERE attachment.Id = @Id
              AND attachment.A_AttachmentTableNameId = 4
              AND attachment.User_Deletion_Id IS NULL;
            """;
        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 30 };
        command.Parameters.Add("@Id", SqlDbType.BigInt).Value = id;
        await using var reader = await command.ExecuteReaderAsync();
        if (!await reader.ReadAsync())
            return NotFound("المرفق غير موجود.");

        string path = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
        string fileName = reader.IsDBNull(3) ? string.Empty : reader.GetString(3);
        string contentType = ImportAttachmentFiles.ContentType(
            string.IsNullOrWhiteSpace(path) ? fileName : path,
            reader.IsDBNull(2) ? null : reader.GetString(2));

        if (!reader.IsDBNull(0) && ((byte[])reader[0]).Length > 0)
            return File((byte[])reader[0], contentType);

        await reader.CloseAsync();
        if (Uri.TryCreate(path, UriKind.Absolute, out var uri) && uri.Scheme is "http" or "https")
            return Redirect(path);
        if (string.IsNullOrWhiteSpace(path))
            return NotFound("مسار المرفق غير مسجل.");
        if (string.IsNullOrWhiteSpace(_attachmentServer))
            return NotFound("مسار مجلد المرفقات غير مهيأ على السيرفر.");

        string resolvedPath = ImportAttachmentFiles.ResolvePath(path, _attachmentServer);
        try
        {
            AttachmentShareConnection.Connect(resolvedPath, _configuration);
            byte[] bytes = await System.IO.File.ReadAllBytesAsync(resolvedPath, HttpContext.RequestAborted);
            if (contentType == "application/pdf" || contentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                return File(bytes, contentType);
            return File(bytes, System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(resolvedPath));
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException)
        {
            _logger.LogError(ex, "Unable to open permission attachment {AttachmentId} at {AttachmentPath}", id, resolvedPath);
            return NotFound("تعذر فتح المرفق من السيرفر. تأكد من وجود الملف وصلاحية الوصول للمجلد المشترك.");
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
        reader.IsDBNull(ordinal) ? string.Empty : Convert.ToString(reader.GetValue(ordinal), CultureInfo.InvariantCulture) ?? string.Empty;

    private static DateTime? ReadNullableDate(SqlDataReader reader, int ordinal)
    {
        if (reader.IsDBNull(ordinal))
            return null;
        object value = reader.GetValue(ordinal);
        return value switch
        {
            DateTime dt => dt,
            DateOnly d => d.ToDateTime(TimeOnly.MinValue),
            _ => Convert.ToDateTime(value, CultureInfo.InvariantCulture)
        };
    }

    private static string ReadDateText(SqlDataReader reader, int ordinal)
    {
        if (reader.IsDBNull(ordinal))
            return string.Empty;
        object value = reader.GetValue(ordinal);
        return value switch
        {
            DateTime dt => dt.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            DateOnly d => d.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
            _ => Convert.ToString(value, CultureInfo.InvariantCulture) ?? string.Empty
        };
    }
}
