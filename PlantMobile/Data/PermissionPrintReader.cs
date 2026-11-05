using System.Data;
using Microsoft.Data.SqlClient;
using PlantMobile.Models;

namespace PlantMobile.Data;

/// <summary>Loads and updates the import-permission print page.</summary>
public sealed class PermissionPrintReader
{
    private readonly string _connectionString;
    private readonly string _privilegeConnectionString;

    public PermissionPrintReader(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DBConnection")
            ?? throw new InvalidOperationException("DBConnection is not configured.");
        _privilegeConnectionString = configuration.GetConnectionString("PrivilegeDBConnection")
            ?? throw new InvalidOperationException("PrivilegeDBConnection is not configured.");
    }

    public async Task<ImPermissionPrintDetailsVm?> ReadDetailsAsync(decimal permissionNumber, short userId, CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

        var model = await ReadHeaderAsync(connection, permissionNumber, cancellationToken).ConfigureAwait(false);
        if (model is null)
            return null;

        await FillUserAndOutletAsync(connection, model, userId, cancellationToken).ConfigureAwait(false);
        await FillImporterAsync(connection, model, cancellationToken).ConfigureAwait(false);
        await FillImporterCompaniesAsync(connection, model, cancellationToken).ConfigureAwait(false);
        await FillItemsAsync(connection, model, cancellationToken).ConfigureAwait(false);
        return model;
    }

    private async Task<ImPermissionPrintDetailsVm?> ReadHeaderAsync(SqlConnection connection, decimal permissionNumber, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT TOP (1)
                   cc.ID,
                   rr.ID,
                   CONVERT(nvarchar(100), cc.ImPermission_Number),
                   cc.IS_Print_Ar,
                   cc.IS_Print_EN,
                   cc.IsPaid,
                   cc.IsAcceppted,
                   cc.Renewal_Status,
                   cc.Print_Count,
                   cc.Start_Date,
                   cc.End_Date,
                   rr.Ship_Name,
                   rr.Shipment_Mean_Id,
                   rr.Transport_Mean_Id,
                   rr.Importer_ID,
                   rr.ImporterType_Id,
                   COALESCE(country.Ar_Name, N''),
                   COALESCE(country.En_Name, N''),
                   shipMean.Ar_Name,
                   shipMean.En_Name,
                   transportMean.Ar_Name,
                   transportMean.En_Name
            FROM dbo.Im_PermissionRequest cc
            INNER JOIN dbo.Im_RequestData rr ON rr.Im_PermissionRequest_ID = cc.ID
            INNER JOIN dbo.Country country ON country.ID = rr.ExportCountry_Id
            LEFT JOIN dbo.Shipment_Mean shipMean ON shipMean.ID = rr.Shipment_Mean_Id
            LEFT JOIN dbo.Transport_Mean transportMean ON transportMean.ID = rr.Transport_Mean_Id
            WHERE cc.ImPermission_Number = @PermissionNumber
            ORDER BY cc.ID DESC;
            """;

        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 30 };
        command.Parameters.Add("@PermissionNumber", SqlDbType.Decimal).Value = permissionNumber;

        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (!await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            return null;

        var importerType = Convert.ToInt32(reader.GetValue(15));
        return new ImPermissionPrintDetailsVm
        {
            Im_PermissionRequest_ID = Convert.ToInt64(reader.GetValue(0)),
            Im_RequestData_ID = Convert.ToInt64(reader.GetValue(1)),
            ImPermission_Number = reader.IsDBNull(2) ? string.Empty : reader.GetString(2),
            IS_Print_Ar = ReadNullableBoolean(reader, 3),
            IS_Print_EN = ReadNullableBoolean(reader, 4),
            IsPaid = ReadNullableBoolean(reader, 5),
            IsAcceppted = ReadNullableBoolean(reader, 6),
            Renewal_Status = reader.IsDBNull(7) ? null : reader.GetByte(7),
            Print_Count = reader.IsDBNull(8) ? null : reader.GetByte(8),
            Start_Date = reader.IsDBNull(9) ? null : reader.GetDateTime(9),
            End_Date = reader.IsDBNull(10) ? null : reader.GetDateTime(10),
            Ship_Name = ReadString(reader, 11),
            Shipment_Mean_Id = reader.IsDBNull(12) ? null : reader.GetByte(12),
            Transport_Mean_Id = reader.IsDBNull(13) ? null : reader.GetByte(13),
            Importer_ID = Convert.ToInt64(reader.GetValue(14)),
            ImporterType_Id = importerType,
            ExportCountryName = reader.IsDBNull(16) ? string.Empty : reader.GetString(16),
            ExportCountryNameEn = reader.IsDBNull(17) ? string.Empty : reader.GetString(17),
            Shipment_MeanName = reader.IsDBNull(18) ? "####" : reader.GetString(18),
            Shipment_MeanNameEn = reader.IsDBNull(19) ? "####" : reader.GetString(19),
            Transport_MeanName = reader.IsDBNull(20) ? "####" : reader.GetString(20),
            Transport_MeanNameEn = reader.IsDBNull(21) ? "####" : reader.GetString(21)
        };
    }

    private async Task FillUserAndOutletAsync(SqlConnection connection, ImPermissionPrintDetailsVm model, short userId, CancellationToken cancellationToken)
    {
        await using var privilegeConnection = new SqlConnection(_privilegeConnectionString);
        await privilegeConnection.OpenAsync(cancellationToken).ConfigureAwait(false);
        const string userSql = """
            SELECT TOP (1) FullName, FullNameEn, Outlet_ID
            FROM dbo.PR_User
            WHERE Id = @UserId;
            """;
        await using var command = new SqlCommand(userSql, privilegeConnection) { CommandTimeout = 30 };
        command.Parameters.Add("@UserId", SqlDbType.SmallInt).Value = userId;

        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            model.UserName = ReadString(reader, 0);
            model.UserNameEn = ReadString(reader, 1);
            var outletId = reader.IsDBNull(2) ? (long?)null : Convert.ToInt64(reader.GetValue(2));
            if (outletId.HasValue)
                await FillOutletAsync(connection, model, outletId.Value, cancellationToken).ConfigureAwait(false);
        }
    }

    private static async Task FillOutletAsync(SqlConnection connection, ImPermissionPrintDetailsVm model, long outletId, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT TOP (1) COALESCE(Ar_Name, N''), COALESCE(En_Name, N'')
            FROM dbo.Outlet
            WHERE ID_HR = @OutletId;
            """;
        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 30 };
        command.Parameters.Add("@OutletId", SqlDbType.BigInt).Value = outletId;
        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            model.outletName = reader.IsDBNull(0) ? string.Empty : reader.GetString(0);
            model.outletName_En = reader.IsDBNull(1) ? string.Empty : reader.GetString(1);
        }
    }

    private async Task FillImporterAsync(SqlConnection connection, ImPermissionPrintDetailsVm model, CancellationToken cancellationToken)
    {
        string sql = model.ImporterType_Id switch
        {
            6 => """
                SELECT TOP (1) Name_Ar, Name_En, Address_Ar, Address_En
                FROM dbo.Company_National
                WHERE ID = @ImporterId;
                """,
            7 => """
                SELECT TOP (1) Name_Ar, Name_En, Address_Ar, Address_En
                FROM dbo.Public_Organization
                WHERE ID = @ImporterId;
                """,
            8 => """
                SELECT TOP (1) Name, Name_EN, Address, Address_EN
                FROM dbo.Person
                WHERE ID = @ImporterId;
                """,
            _ => string.Empty
        };
        if (sql.Length == 0)
            return;

        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 30 };
        command.Parameters.Add("@ImporterId", SqlDbType.BigInt).Value = model.Importer_ID;
        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            model.ImporterName = ReadString(reader, 0);
            model.ImporterNameEn = ReadString(reader, 1);
            model.ImporterAddress = ReadString(reader, 2);
            model.ImporterAddressEn = ReadString(reader, 3);
        }
    }

    private static async Task FillImporterCompaniesAsync(SqlConnection connection, ImPermissionPrintDetailsVm model, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT ImportCompany, ImportCompany_EN, ImporeterCompanyAddress, ImporeterCompanyAddress_EN
            FROM dbo.Im_RequestDat_Extra
            WHERE Im_RequestData_ID = @RequestDataId
            ORDER BY ID;
            """;
        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 30 };
        command.Parameters.Add("@RequestDataId", SqlDbType.BigInt).Value = model.Im_RequestData_ID ?? 0L;
        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            model.ImportersCompanies.Add(new PrintImportersVm
            {
                ImporterCompany = ReadString(reader, 0),
                ImporterCompanyEn = ReadString(reader, 1),
                ImporterCompanyAddress = ReadString(reader, 2),
                ImporterCompanyAddressEn = ReadString(reader, 3)
            });
        }
    }

    private static async Task FillItemsAsync(SqlConnection connection, ImPermissionPrintDetailsVm model, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT DISTINCT p.ID,
                   p.Im_Initiator_ID,
                   p.Size,
                   p.Package_Count,
                   p.Package_Weight,
                   p.Units_Number,
                   p.Package_Type_ID,
                   p.Package_Material_ID,
                   p.GrossWeight,
                   COALESCE(NULLIF(p.Order_Text, N''), N'####')
            FROM dbo.Im_PermissionItems p
            WHERE p.Im_PermissionRequest_ID = @PermissionId
            ORDER BY p.ID;
            """;
        var items = new List<PrintItemVm>();
        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 30 };
        command.Parameters.Add("@PermissionId", SqlDbType.BigInt).Value = model.Im_PermissionRequest_ID;
        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            items.Add(new PrintItemVm
            {
                ImPermissionItem_ID = Convert.ToInt64(reader.GetValue(0)),
                Im_Initiator_ID = ReadNullableInt64(reader, 1),
                Size = ReadNullableDouble(reader, 2),
                Package_Count = ReadNullableInt32(reader, 3),
                Package_Weight = ReadNullableDecimal(reader, 4),
                Units_Number = ReadNullableInt32(reader, 5),
                packageTypeID = ReadNullableInt16(reader, 6),
                packageMaterialID = ReadNullableInt16(reader, 7),
                GrossWeight = ReadNullableDecimal(reader, 8),
                Order_Text = ReadString(reader, 9)
            });
        }
        await reader.DisposeAsync().ConfigureAwait(false);

        foreach (var item in items)
            await FillItemAsync(connection, item, cancellationToken).ConfigureAwait(false);

        model.Items = items;
    }

    private static async Task FillItemAsync(SqlConnection connection, PrintItemVm item, CancellationToken cancellationToken)
    {
        item.packageType = await ReadLookupNameAsync(connection, "dbo.Package_Type", item.packageTypeID, "####", cancellationToken).ConfigureAwait(false);
        item.packageTypeEn = await ReadLookupNameAsync(connection, "dbo.Package_Type", item.packageTypeID, "####", cancellationToken, english: true).ConfigureAwait(false);
        item.packageMaterial = await ReadLookupNameAsync(connection, "dbo.Package_Material", item.packageMaterialID, "####", cancellationToken).ConfigureAwait(false);
        item.packageMaterialEn = await ReadLookupNameAsync(connection, "dbo.Package_Material", item.packageMaterialID, "####", cancellationToken, english: true).ConfigureAwait(false);

        long? initiatorId = item.Im_Initiator_ID;
        long? itemShortNameId = null;
        short? qualitativeGroupId = null;
        if (initiatorId.HasValue)
        {
            const string initiatorSql = """
                SELECT TOP (1) Country_Id, Item_ShortName_ID, QualitativeGroup_Id
                FROM dbo.Im_Initiator
                WHERE ID = @InitiatorId;
                """;

            short? countryId = null;
            await using (var command = new SqlCommand(initiatorSql, connection) { CommandTimeout = 30 })
            {
                command.Parameters.Add("@InitiatorId", SqlDbType.BigInt).Value = initiatorId.Value;
                await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
                if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                {
                    countryId = ReadNullableInt16(reader, 0);
                    itemShortNameId = ReadNullableInt64(reader, 1);
                    qualitativeGroupId = ReadNullableInt16(reader, 2);
                }
            }

            await FillInitiatorCountryAsync(connection, item, countryId, cancellationToken).ConfigureAwait(false);
        }

        if (itemShortNameId.HasValue)
            await FillItemShortNameAsync(connection, item, itemShortNameId.Value, cancellationToken).ConfigureAwait(false);
        else
            FillUnknownItemNames(item);

        if (qualitativeGroupId.HasValue)
        {
            const string qualitativeSql = """
                SELECT TOP (1) Name_Ar, Name_En
                FROM dbo.QualitativeGroup
                WHERE Id = @GroupId;
                """;
            await using var command = new SqlCommand(qualitativeSql, connection) { CommandTimeout = 30 };
            command.Parameters.Add("@GroupId", SqlDbType.SmallInt).Value = qualitativeGroupId.Value;
            await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
            if (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                item.qualitiveGroupName = ReadString(reader, 0);
                item.qualitiveGroupNameEn = ReadString(reader, 1);
            }
        }

        item.ItemCategories = await ReadCategoriesAsync(connection, item.ImPermissionItem_ID ?? 0L, cancellationToken).ConfigureAwait(false);
        item.Itemconstrains = await ReadConstrainsAsync(connection, initiatorId, itemShortNameId, qualitativeGroupId, cancellationToken).ConfigureAwait(false);
    }

    private static async Task FillInitiatorCountryAsync(SqlConnection connection, PrintItemVm item, short? countryId, CancellationToken cancellationToken)
    {
        if (!countryId.HasValue)
        {
            item.InitiatorCountry = "كل الدول";
            item.InitiatorCountryEn = "All Country";
            return;
        }

        const string countrySql = """
            SELECT TOP (1) Ar_Name, En_Name
            FROM dbo.Country
            WHERE ID = @CountryId;
            """;
        await using var command = new SqlCommand(countrySql, connection) { CommandTimeout = 30 };
        command.Parameters.Add("@CountryId", SqlDbType.SmallInt).Value = countryId.Value;
        await using var countryReader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (await countryReader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            item.InitiatorCountry = ReadString(countryReader, 0);
            item.InitiatorCountryEn = ReadString(countryReader, 1);
        }
    }

    private static async Task FillItemShortNameAsync(SqlConnection connection, PrintItemVm item, long itemShortNameId, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT TOP (1) ShortName_Ar, ShortName_En, Item_Status_ID, Item_Purpose_ID, SubPart_ID, Item_ID
            FROM dbo.Item_ShortName
            WHERE ID = @ShortNameId;
            """;
        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 30 };
        command.Parameters.Add("@ShortNameId", SqlDbType.BigInt).Value = itemShortNameId;

        long? itemId = null;
        int? subPartId = null;
        int? statusId = null;
        int? purposeId = null;
        await using (var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false))
        {
            if (!await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                FillUnknownItemNames(item);
                return;
            }

            item.ItemShortNameAr = ReadString(reader, 0);
            item.ItemShortNameEn = ReadString(reader, 1);
            statusId = ReadNullableInt32(reader, 2);
            purposeId = ReadNullableInt32(reader, 3);
            subPartId = ReadNullableInt32(reader, 4);
            itemId = ReadNullableInt64(reader, 5);
        }

        item.Status = await ReadLookupNameAsync(connection, "dbo.Item_Status", statusId, "####", cancellationToken).ConfigureAwait(false);
        item.StatusEn = await ReadLookupNameAsync(connection, "dbo.Item_Status", statusId, "####", cancellationToken, english: true).ConfigureAwait(false);
        item.Purpose = await ReadLookupNameAsync(connection, "dbo.Item_Purpose", purposeId, "####", cancellationToken).ConfigureAwait(false);
        item.PurposeEn = await ReadLookupNameAsync(connection, "dbo.Item_Purpose", purposeId, "####", cancellationToken, english: true).ConfigureAwait(false);

        if (subPartId.HasValue)
        {
            const string subSql = """
                SELECT TOP (1) Name_Ar, Name_En
                FROM dbo.SubPart
                WHERE ID = @SubPartId;
                """;
            await using var subCommand = new SqlCommand(subSql, connection) { CommandTimeout = 30 };
            subCommand.Parameters.Add("@SubPartId", SqlDbType.Int).Value = subPartId.Value;
            await using var subReader = await subCommand.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
            if (await subReader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                item.subPartName = ReadString(subReader, 0);
                item.subPartNameEn = ReadString(subReader, 1);
            }
        }
        else
        {
            item.subPartName = "####";
            item.subPartNameEn = "####";
        }

        if (itemId.HasValue)
        {
            const string itemSql = """
                SELECT TOP (1) Name_Ar, COALESCE(Scientific_Name, N'####')
                FROM dbo.Item
                WHERE ID = @ItemId;
                """;
            await using var itemCommand = new SqlCommand(itemSql, connection) { CommandTimeout = 30 };
            itemCommand.Parameters.Add("@ItemId", SqlDbType.BigInt).Value = itemId.Value;
            await using var itemReader = await itemCommand.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
            if (await itemReader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                item.ItemNameAr = ReadString(itemReader, 0);
                item.ScientificNameAr = ReadString(itemReader, 1);
            }
        }
    }

    private static void FillUnknownItemNames(PrintItemVm item)
    {
        item.ItemShortNameAr = "####";
        item.ItemShortNameEn = "####";
        item.Status = "####";
        item.StatusEn = "####";
        item.Purpose = "####";
        item.PurposeEn = "####";
        item.subPartName = "####";
        item.subPartNameEn = "####";
        item.ItemNameAr = "####";
        item.ScientificNameAr = "####";
    }

    private static async Task<List<PrintCategoryVm>> ReadCategoriesAsync(SqlConnection connection, long permissionItemId, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT c.ID,
                   c.Im_PermissionItems_ID,
                   c.ItemCategory_ID,
                   c.ItemCategoryGroup_ID,
                   c.GrossWeight,
                   c.Package_Type_ID
            FROM dbo.Im_PermissionItems_Category c
            WHERE c.Im_PermissionItems_ID = @PermissionItemId
            ORDER BY c.ID;
            """;
        var rows = new List<PrintCategoryVm>();
        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 30 };
        command.Parameters.Add("@PermissionItemId", SqlDbType.BigInt).Value = permissionItemId;
        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            rows.Add(new PrintCategoryVm
            {
                Im_PermissionItemsCategory_ID = Convert.ToInt64(reader.GetValue(0)),
                Im_PermissionItems_ID = ReadNullableInt64(reader, 1),
                ItemCategory_ID = ReadNullableInt64(reader, 2),
                GrossWeight = ReadNullableDecimal(reader, 4),
                packageTypeID = ReadNullableInt16(reader, 5)
            });
        }
        await reader.DisposeAsync().ConfigureAwait(false);

        foreach (var category in rows)
        {
            category.packageType = await ReadLookupNameAsync(connection, "dbo.Package_Type", category.packageTypeID, string.Empty, cancellationToken).ConfigureAwait(false);
            if (category.ItemCategory_ID.HasValue)
            {
                const string itemCategorySql = """
                    SELECT TOP (1) Name_Ar, Name_En
                    FROM dbo.ItemCategory
                    WHERE ID = @CategoryId;
                    """;
                await using var categoryCommand = new SqlCommand(itemCategorySql, connection) { CommandTimeout = 30 };
                categoryCommand.Parameters.Add("@CategoryId", SqlDbType.BigInt).Value = category.ItemCategory_ID.Value;
                await using var categoryReader = await categoryCommand.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
                if (await categoryReader.ReadAsync(cancellationToken).ConfigureAwait(false))
                {
                    category.categoryName = ReadString(categoryReader, 0);
                    category.categoryNameEn = ReadString(categoryReader, 1);
                }
            }
            else
            {
                // Legacy model did not carry the group id forward when only a group was present; fetch it here.
                // No additional lookup is required for the printed page beyond the group name fields.
                category.categoryName = await ReadGroupCategoryNameAsync(connection, permissionItemId, category.Im_PermissionItemsCategory_ID, cancellationToken).ConfigureAwait(false);
                category.categoryNameEn = await ReadGroupCategoryNameAsync(connection, permissionItemId, category.Im_PermissionItemsCategory_ID, cancellationToken, english: true).ConfigureAwait(false);
            }
        }

        return rows;
    }

    private static async Task<string> ReadGroupCategoryNameAsync(SqlConnection connection, long permissionItemId, long? categoryId, CancellationToken cancellationToken, bool english = false)
    {
        if (!categoryId.HasValue)
            return string.Empty;

        const string sql = """
            SELECT TOP (1) g.Name_Ar, g.Name_En
            FROM dbo.Im_PermissionItems_Category c
            INNER JOIN dbo.ItemCategories_Group g ON g.ID = c.ItemCategoryGroup_ID
            WHERE c.Im_PermissionItems_ID = @PermissionItemId
              AND c.ID = @CategoryId;
            """;
        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 30 };
        command.Parameters.Add("@PermissionItemId", SqlDbType.BigInt).Value = permissionItemId;
        command.Parameters.Add("@CategoryId", SqlDbType.BigInt).Value = categoryId.Value;
        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (!await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            return string.Empty;
        return english ? ReadString(reader, 1) : ReadString(reader, 0);
    }

    private static async Task<PrintConstrainVm> ReadConstrainsAsync(SqlConnection connection, long? initiatorId, long? itemShortNameId, short? qualitativeGroupId, CancellationToken cancellationToken)
    {
        var result = new PrintConstrainVm();
        if (!initiatorId.HasValue)
            return result;

        const string textSql = """
            SELECT t.InSide_Certificate_Ar, t.InSide_Certificate_En
            FROM dbo.Im_Constrain_Initiator_Text c
            INNER JOIN dbo.Im_CountryConstrain_Text t ON t.ID = c.ConstrainText_ID
            WHERE c.Im_Initiator_ID = @InitiatorId
              AND c.IsActive = 1;
            """;
        await using var textCommand = new SqlCommand(textSql, connection) { CommandTimeout = 30 };
        textCommand.Parameters.Add("@InitiatorId", SqlDbType.BigInt).Value = initiatorId.Value;
        await using var textReader = await textCommand.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await textReader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            var ar = ReadString(textReader, 0);
            var en = ReadString(textReader, 1);
            if (!string.IsNullOrWhiteSpace(ar)) result.texts_Ar.Add(ar);
            if (!string.IsNullOrWhiteSpace(en)) result.text_En.Add(en);
        }
        await textReader.DisposeAsync().ConfigureAwait(false);

        string? portItemShortName = itemShortNameId?.ToString(System.Globalization.CultureInfo.InvariantCulture);
        if (qualitativeGroupId.HasValue)
        {
            result.itemConstrainPorts = await ReadConstrainPortsAsync(connection, null, qualitativeGroupId, cancellationToken).ConfigureAwait(false);
        }
        else if (itemShortNameId.HasValue)
        {
            result.itemConstrainPorts = await ReadConstrainPortsAsync(connection, itemShortNameId, null, cancellationToken).ConfigureAwait(false);
        }
        _ = portItemShortName;
        return result;
    }

    private static async Task<List<PrintPortVm>> ReadConstrainPortsAsync(SqlConnection connection, long? itemShortNameId, short? qualitativeGroupId, CancellationToken cancellationToken)
    {
        string sql = qualitativeGroupId.HasValue
            ? """
                SELECT p.Port_Type_ID, p.Port_National_Id, pt.Name_Ar, pt.Name_En, pn.Name_Ar, pn.Name_En
                FROM dbo.Im_CountryConstrain_ArrivalPort p
                LEFT JOIN dbo.Port_Type pt ON pt.ID = p.Port_Type_ID
                LEFT JOIN dbo.PortNational pn ON pn.ID = p.Port_National_Id
                WHERE p.IsActive = 1
                  AND p.Id_QualitativeGroup = @GroupId;
                """
            : """
                SELECT p.Port_Type_ID, p.Port_National_Id, pt.Name_Ar, pt.Name_En, pn.Name_Ar, pn.Name_En
                FROM dbo.Im_CountryConstrain_ArrivalPort p
                LEFT JOIN dbo.Port_Type pt ON pt.ID = p.Port_Type_ID
                LEFT JOIN dbo.PortNational pn ON pn.ID = p.Port_National_Id
                WHERE p.IsActive = 1
                  AND p.Item_ShortName_ID = @ShortNameId;
                """;
        var rows = new List<PrintPortVm>();
        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 30 };
        if (qualitativeGroupId.HasValue)
            command.Parameters.Add("@GroupId", SqlDbType.SmallInt).Value = qualitativeGroupId.Value;
        else
            command.Parameters.Add("@ShortNameId", SqlDbType.BigInt).Value = itemShortNameId ?? 0L;

        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            rows.Add(new PrintPortVm
            {
                portTypeId = ReadNullableInt16(reader, 0),
                portId = ReadNullableInt32(reader, 1),
                portType = ReadString(reader, 2),
                portTypeEn = ReadString(reader, 3),
                portName = ReadString(reader, 4),
                portNameEn = ReadString(reader, 5)
            });
        }
        return rows;
    }

    public async Task MarkPrintedAsync(long permissionRequestId, short userId, bool arabic, CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await using var transaction = (SqlTransaction)await connection.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            const string readSql = """
                SELECT Renewal_Status, Start_Date, End_Date
                FROM dbo.Im_PermissionRequest
                WHERE ID = @PermissionId;
                """;
            await using var readCommand = new SqlCommand(readSql, connection, transaction) { CommandTimeout = 30 };
            readCommand.Parameters.Add("@PermissionId", SqlDbType.BigInt).Value = permissionRequestId;
            await using var readReader = await readCommand.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
            if (!await readReader.ReadAsync(cancellationToken).ConfigureAwait(false))
                throw new InvalidOperationException($"Permission request {permissionRequestId} was not found.");

            var renewalStatus = readReader.IsDBNull(0) ? (byte?)null : readReader.GetByte(0);
            var hasStartDate = !readReader.IsDBNull(1);
            var hasEndDate = !readReader.IsDBNull(2);
            await readReader.DisposeAsync().ConfigureAwait(false);

            var actionId = arabic ? (renewalStatus == 0 ? 1 : 5) : (renewalStatus == 0 ? 2 : 6);
            var note = arabic
                ? (renewalStatus == 0 ? "تمت الطباعة العربي" : "تمت الطباعة العربي للتجديد")
                : (renewalStatus == 0 ? "تمت الطباعة English" : "تمت الطباعة English للتجديد");

            const string updateSql = """
                UPDATE dbo.Im_PermissionRequest
                SET IS_Print_Ar = CASE WHEN @Arabic = 1 THEN CONVERT(bit, 1) ELSE IS_Print_Ar END,
                    IS_Print_EN = CASE WHEN @Arabic = 0 THEN CONVERT(bit, 1) ELSE IS_Print_EN END,
                    Start_Date = COALESCE(Start_Date, GETDATE()),
                    End_Date = COALESCE(End_Date, DATEADD(month, 3, GETDATE()))
                WHERE ID = @PermissionId;
                """;
            await using var updateCommand = new SqlCommand(updateSql, connection, transaction) { CommandTimeout = 30 };
            updateCommand.Parameters.Add("@Arabic", SqlDbType.Bit).Value = arabic;
            updateCommand.Parameters.Add("@PermissionId", SqlDbType.BigInt).Value = permissionRequestId;
            if (await updateCommand.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false) != 1)
                throw new InvalidOperationException("Permission request could not be updated.");

            const string logSql = """
                INSERT INTO dbo.Table_Action_Log
                    (ID, ID_Table_Action, NOTS, ID_TableActionValue,
                     User_Creation_Id, User_Creation_Date, User_Type_ID, Type_log_ID)
                VALUES
                    (NEXT VALUE FOR dbo.Table_Action_Log_seq, @ActionId, @Note, @PermissionId,
                     @UserId, GETDATE(), 127, 135);
                """;
            await using var logCommand = new SqlCommand(logSql, connection, transaction) { CommandTimeout = 30 };
            logCommand.Parameters.Add("@ActionId", SqlDbType.SmallInt).Value = actionId;
            logCommand.Parameters.Add("@Note", SqlDbType.NVarChar, 100).Value = note;
            logCommand.Parameters.Add("@PermissionId", SqlDbType.BigInt).Value = permissionRequestId;
            logCommand.Parameters.Add("@UserId", SqlDbType.BigInt).Value = userId;
            if (await logCommand.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false) != 1)
                throw new InvalidOperationException("Print action could not be logged.");

            await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
            _ = hasStartDate;
            _ = hasEndDate;
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
            throw;
        }
    }

    private static async Task<string> ReadLookupNameAsync(SqlConnection connection, string tableName, short? id, string fallback, CancellationToken cancellationToken, bool english = false)
    {
        if (!id.HasValue)
            return fallback;
        var column = english ? "En_Name" : "Ar_Name";
        var sql = $"SELECT TOP (1) {column} FROM {tableName} WHERE ID = @Id;";
        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 30 };
        command.Parameters.Add("@Id", SqlDbType.SmallInt).Value = id.Value;
        var value = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return value is DBNull or null ? fallback : Convert.ToString(value) ?? fallback;
    }

    private static async Task<string> ReadLookupNameAsync(SqlConnection connection, string tableName, int? id, string fallback, CancellationToken cancellationToken, bool english = false)
    {
        if (!id.HasValue)
            return fallback;
        var column = english ? "En_Name" : "Ar_Name";
        var sql = $"SELECT TOP (1) {column} FROM {tableName} WHERE ID = @Id;";
        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 30 };
        command.Parameters.Add("@Id", SqlDbType.Int).Value = id.Value;
        var value = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return value is DBNull or null ? fallback : Convert.ToString(value) ?? fallback;
    }

    private static string ReadString(SqlDataReader reader, int ordinal) =>
        reader.IsDBNull(ordinal) ? string.Empty : Convert.ToString(reader.GetValue(ordinal)) ?? string.Empty;

    private static bool? ReadNullableBoolean(SqlDataReader reader, int ordinal) =>
        reader.IsDBNull(ordinal) ? null : Convert.ToBoolean(reader.GetValue(ordinal));

    private static long? ReadNullableInt64(SqlDataReader reader, int ordinal) =>
        reader.IsDBNull(ordinal) ? null : Convert.ToInt64(reader.GetValue(ordinal));

    private static int? ReadNullableInt32(SqlDataReader reader, int ordinal) =>
        reader.IsDBNull(ordinal) ? null : Convert.ToInt32(reader.GetValue(ordinal));

    private static short? ReadNullableInt16(SqlDataReader reader, int ordinal) =>
        reader.IsDBNull(ordinal) ? null : Convert.ToInt16(reader.GetValue(ordinal));

    private static decimal? ReadNullableDecimal(SqlDataReader reader, int ordinal) =>
        reader.IsDBNull(ordinal) ? null : Convert.ToDecimal(reader.GetValue(ordinal));

    private static double? ReadNullableDouble(SqlDataReader reader, int ordinal) =>
        reader.IsDBNull(ordinal) ? null : Convert.ToDouble(reader.GetValue(ordinal));
}
