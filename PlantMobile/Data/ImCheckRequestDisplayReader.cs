using System.Data;
using Microsoft.Data.SqlClient;
using PlantMobile.Models;

namespace PlantMobile.Data;

/// <summary>Reads and updates the check request display page.</summary>
public sealed class ImCheckRequestDisplayReader
{
    private readonly string _connectionString;

    public ImCheckRequestDisplayReader(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DBConnection")
            ?? throw new InvalidOperationException("DBConnection is not configured.");
    }

    public async Task<ImCheckRequestDisplayVm?> ReadDetailsAsync(string checkNumber, CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

        var model = await ReadHeaderAsync(connection, checkNumber, cancellationToken).ConfigureAwait(false);
        if (model is null)
            return null;

        model.List_Permission = await ReadPermissionsAsync(connection, model.ID, cancellationToken).ConfigureAwait(false);
        model.List_Items = await ReadItemsAsync(connection, model.ID, cancellationToken).ConfigureAwait(false);
        model.List_Attachments = await ReadAttachmentsAsync(connection, model.ID, cancellationToken).ConfigureAwait(false);
        return model;
    }

    public async Task<EditShippingDateVm?> ReadShippingDateAsync(
        string checkNumber,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT TOP (1)
                   request.ID,
                   customs.ID,
                   request.CheckRequest_Number,
                   COALESCE(customs.Customs_Certificate_Number, N''),
                   customs.Shipment_Date,
                   customs.Arrival_Date
            FROM dbo.Im_CheckRequest request
            INNER JOIN dbo.Im_CheckRequest_Customs_Message customs
                ON customs.Im_CheckRequest_ID = request.ID
            WHERE request.CheckRequest_Number = @CheckNumber
            ORDER BY customs.ID DESC;
            """;

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 30 };
        command.Parameters.Add("@CheckNumber", SqlDbType.VarChar, 50).Value = checkNumber.Trim();

        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (!await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            return null;

        return new EditShippingDateVm
        {
            RequestId = Convert.ToInt64(reader.GetValue(0)),
            CustomsMessageId = Convert.ToInt32(reader.GetValue(1)),
            CheckRequestNumber = ReadString(reader, 2),
            CustomsCertificateNumber = ReadString(reader, 3),
            ShippingDate = ReadNullableDateTime(reader, 4),
            ArrivalDate = ReadNullableDateTime(reader, 5)
        };
    }

    public async Task UpdateShippingDateAsync(
        EditShippingDateVm model,
        short userId,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            IF EXISTS
            (
                SELECT 1
                FROM dbo.Im_CheckRequest_Customs_Message customs
                INNER JOIN dbo.Im_CheckRequest request
                    ON request.ID = customs.Im_CheckRequest_ID
                WHERE customs.ID = @CustomsMessageId
                  AND request.ID = @RequestId
                  AND request.CheckRequest_Number = @CheckNumber
                  AND customs.Arrival_Date IS NOT NULL
                  AND @ShippingDate > customs.Arrival_Date
            )
                THROW 50001, N'تاريخ الشحن لا يمكن أن يكون بعد تاريخ الوصول.', 1;

            UPDATE customs
               SET customs.Shipment_Date = @ShippingDate,
                   customs.User_Updation_Id = @UserId,
                   customs.User_Updation_Date = SYSDATETIME()
            FROM dbo.Im_CheckRequest_Customs_Message customs
            INNER JOIN dbo.Im_CheckRequest request
                ON request.ID = customs.Im_CheckRequest_ID
            WHERE customs.ID = @CustomsMessageId
              AND request.ID = @RequestId
              AND request.CheckRequest_Number = @CheckNumber;

            SELECT @@ROWCOUNT;
            """;

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 30 };
        command.Parameters.Add("@ShippingDate", SqlDbType.Date).Value = model.ShippingDate!.Value.Date;
        command.Parameters.Add("@UserId", SqlDbType.SmallInt).Value = userId;
        command.Parameters.Add("@CustomsMessageId", SqlDbType.Int).Value = model.CustomsMessageId;
        command.Parameters.Add("@RequestId", SqlDbType.BigInt).Value = model.RequestId;
        command.Parameters.Add("@CheckNumber", SqlDbType.VarChar, 50).Value = model.CheckRequestNumber.Trim();

        int affectedRows;
        try
        {
            affectedRows = Convert.ToInt32(
                await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false));
        }
        catch (SqlException ex) when (ex.Number == 50001)
        {
            throw new InvalidOperationException(
                "تاريخ الشحن لا يمكن أن يكون بعد تاريخ الوصول.", ex);
        }

        if (affectedRows != 1)
            throw new InvalidOperationException("لم يتم العثور على الطلب أو تم تغيير بياناته قبل الحفظ.");
    }

    private async Task<ImCheckRequestDisplayVm?> ReadHeaderAsync(SqlConnection connection, string checkNumber, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT TOP (1)
                   cr.ID,
                   cr.Completion_Notes,
                   cr.Completion_Status,
                   cr.Completion_Request_Date,
                   cr.Completion_Response_Date,
                   cr.CheckRequest_Number,
                   cr.ExportCompanyAddress,
                   cr.User_Creation_Date,
                   cr.IsAccepted,
                   cr.IsPaid,
                   cr.IsActive,
                   cm.Customs_Certificate_Number,
                   cm.Certification_Date,
                   cm.Manifest_Number,
                   cm.Shipment_Date,
                   cm.Arrival_Date,
                   cm.ID,
                   COALESCE(op.Name_Ar, N''),
                   COALESCE(icde.ImportCompany, N''),
                   COALESCE(arr.ArArrivalPortType, N''),
                   COALESCE(arr.ArPortName, N''),
                   COALESCE(exp.ArExportPortType, N''),
                   COALESCE(exp.ArExportPortName, N''),
                   COALESCE(exp.ArExportCountryName, N''),
                   COALESCE(tar.ArTransitPortType, N''),
                   COALESCE(tar.ArTransitPortName, N''),
                   COALESCE(tar.ArTransitCountryName, N'')
            FROM dbo.Im_CheckRequest cr
            INNER JOIN dbo.Im_CheckRequest_Data data ON data.Im_CheckRequest_ID = cr.ID
            LEFT JOIN dbo.Im_CheckRequest_Customs_Message cm ON cm.Im_CheckRequest_ID = cr.ID
            LEFT JOIN dbo.Im_OpertaionType op ON op.ID = cm.Im_OperationType
            LEFT JOIN dbo.Im_CheckRequestData_Extra icde ON icde.Im_CheckRequest_Data_ID = data.ID
            CROSS APPLY (
                SELECT TOP (1) pt.Name_Ar AS ArArrivalPortType, pn.Name_Ar AS ArPortName
                FROM dbo.Im_CheckRequest_Port p
                INNER JOIN dbo.PortNational pn ON pn.ID = p.Port_ID
                INNER JOIN dbo.Port_Type pt ON pt.ID = p.Port_Type_ID
                WHERE p.Im_CheckRequest_Data_ID = data.ID
                  AND p.ReqPortType_ID = 10
            ) arr
            CROSS APPLY (
                SELECT TOP (1) pt.Name_Ar AS ArExportPortType, pi.Name_Ar AS ArExportPortName, country.Ar_Name AS ArExportCountryName
                FROM dbo.Im_CheckRequest_Port p
                INNER JOIN dbo.Port_International pi ON pi.ID = p.Port_ID
                INNER JOIN dbo.Port_Type pt ON pt.ID = p.Port_Type_ID
                INNER JOIN dbo.Country country ON country.ID = pi.Country_ID
                WHERE p.Im_CheckRequest_Data_ID = data.ID
                  AND p.ReqPortType_ID = 9
            ) exp
            CROSS APPLY (
                SELECT TOP (1) pt.Name_Ar AS ArTransitPortType, pi.Name_Ar AS ArTransitPortName, country.Ar_Name AS ArTransitCountryName
                FROM dbo.Im_CheckRequest_Port p
                INNER JOIN dbo.Port_International pi ON pi.ID = p.Port_ID
                INNER JOIN dbo.Port_Type pt ON pt.ID = p.Port_Type_ID
                INNER JOIN dbo.Country country ON country.ID = pi.Country_ID
                WHERE p.Im_CheckRequest_Data_ID = data.ID
                  AND p.ReqPortType_ID = 11
            ) tar
            WHERE cr.CheckRequest_Number = @CheckNumber;
            """;

        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 60 };
        command.Parameters.Add("@CheckNumber", SqlDbType.VarChar, 50).Value = checkNumber?.Trim() ?? string.Empty;

        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (!await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            return null;

        return new ImCheckRequestDisplayVm
        {
            ID = Convert.ToInt64(reader.GetValue(0)),
            Completion_Notes = ReadString(reader, 1),
            Completion_Status = ReadNullableByte(reader, 2),
            Completion_Request_Date = ReadNullableDateTime(reader, 3),
            Completion_Response_Date = ReadNullableDateTime(reader, 4),
            CheckRequest_Number = ReadString(reader, 5),
            ExportCompanyAddress = ReadString(reader, 6),
            User_Creation_Date = ReadNullableDateTime(reader, 7),
            IsAccepted = ReadNullableBoolean(reader, 8),
            IsPaid = ReadNullableBoolean(reader, 9),
            IsActive = ReadNullableBoolean(reader, 10),
            Customs_Certificate_Number = ReadString(reader, 11),
            Certificate_Date = ReadNullableDateTime(reader, 12),
            Manifest_Number = ReadString(reader, 13),
            Shipping_Date = ReadNullableDateTime(reader, 14),
            Arrival_Date = ReadNullableDateTime(reader, 15),
            Im_CheckRequest_Customs_MessageID = ReadNullableInt64(reader, 16),
            Operation_Type_Name = ReadString(reader, 17),
            Importer_Name = ReadString(reader, 18),
            ArrivalPortType = ReadString(reader, 19),
            ArrivalPortName = ReadString(reader, 20),
            ExportPortType = ReadString(reader, 21),
            ExportPortName = ReadString(reader, 22),
            ExportCountryName = ReadString(reader, 23),
            TransitPortType = ReadString(reader, 24),
            TransitPortName = ReadString(reader, 25),
            TransitCountryName = ReadString(reader, 26)
        };
    }

    private static async Task<List<DisplayPermissionVm>> ReadPermissionsAsync(SqlConnection connection, long requestId, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT permission.ID,
                   CONVERT(nvarchar(100), permission.ImPermission_Number),
                   item.Name_Ar,
                   permissionItems.GrossWeight
            FROM dbo.Im_PermissionRequest permission
            INNER JOIN dbo.Im_PermissionItems permissionItems ON permissionItems.Im_PermissionRequest_ID = permission.ID
            INNER JOIN dbo.Im_Initiator initiator ON initiator.ID = permissionItems.Im_Initiator_ID
            INNER JOIN dbo.Item_ShortName shortName ON shortName.ID = initiator.Item_ShortName_ID
            INNER JOIN dbo.Item item ON item.ID = shortName.Item_ID
            WHERE permission.Im_CheckRequest_ID = @RequestId
            ORDER BY permission.ID;
            """;

        var list = new List<DisplayPermissionVm>();
        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 30 };
        command.Parameters.Add("@RequestId", SqlDbType.BigInt).Value = requestId;
        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            list.Add(new DisplayPermissionVm
            {
                ID = Convert.ToInt64(reader.GetValue(0)),
                ImPermission_Number = ReadString(reader, 1),
                Item_Name = ReadString(reader, 2),
                GrossWeight = ReadNullableDecimal(reader, 3)
            });
        }
        return list;
    }

    private static async Task<List<DisplayItemVm>> ReadItemsAsync(SqlConnection connection, long requestId, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT DISTINCT i.ID,
                   i.Im_Initiator_ID,
                   i.Item_ShortName_ID,
                   i.GrossWeight,
                   i.Net_Weight,
                   plant.Name_Ar,
                   COALESCE(plant.Scientific_Name, N''),
                   COALESCE(shortName.ShortName_Ar, N''),
                   COALESCE(status.Ar_Name, N''),
                   COALESCE(purpose.Ar_Name, N''),
                   COALESCE(shortName.HSCode, N''),
                   COALESCE([group].Name_Ar, N''),
                   COALESCE(secondary.Name_Ar, N''),
                   COALESCE(main.Name_Ar, N''),
                   COALESCE(itemType.Name_Ar, N''),
                   COALESCE(family.Name_Ar, N''),
                   COALESCE([order].Name_Ar, N''),
                   COALESCE(phylum.Name_Ar, N''),
                   COALESCE(kingdom.Name_Ar, N''),
                   COALESCE(country.Ar_Name, N'')
            FROM dbo.Im_CheckRequest_Items i
            INNER JOIN dbo.Im_CheckRequset_Shipping_Method shipping ON shipping.ID = i.Im_CheckRequset_Shipping_Method_ID
            INNER JOIN dbo.Item_ShortName shortName ON shortName.ID = i.Item_ShortName_ID
            INNER JOIN dbo.Item plant ON plant.ID = shortName.Item_ID
            LEFT JOIN dbo.Item_Status status ON status.ID = shortName.Item_Status_ID
            LEFT JOIN dbo.Item_Purpose purpose ON purpose.ID = shortName.Item_Purpose_ID
            LEFT JOIN dbo.[Group] [group] ON [group].ID = plant.Group_ID
            LEFT JOIN dbo.SecondaryClassification secondary ON secondary.ID = [group].SecClass_ID
            LEFT JOIN dbo.MainCalssification main ON main.ID = secondary.MainClass_ID
            LEFT JOIN dbo.Item_Type itemType ON itemType.Id = main.Item_Type_ID
            LEFT JOIN dbo.Family family ON family.ID = plant.Family_ID
            LEFT JOIN dbo.[Order] [order] ON [order].ID = family.Order_ID
            LEFT JOIN dbo.PhylumSubphylum phylum ON phylum.ID = [order].Phylum_ID
            LEFT JOIN dbo.Kingdom kingdom ON kingdom.ID = phylum.Kingdom_ID
            LEFT JOIN dbo.Im_Initiator initiator ON initiator.ID = i.Im_Initiator_ID
            LEFT JOIN dbo.Country country ON country.ID = initiator.Country_Id
            WHERE shipping.Im_CheckRequest_ID = @RequestId
            ORDER BY i.ID;
            """;

        var items = new List<DisplayItemVm>();
        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 60 };
        command.Parameters.Add("@RequestId", SqlDbType.BigInt).Value = requestId;
        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            items.Add(new DisplayItemVm
            {
                ID = Convert.ToInt64(reader.GetValue(0)),
                Im_Initiator_ID = ReadNullableInt64(reader, 1),
                Item_ShortName_ID = ReadNullableInt64(reader, 2),
                GrossWeight = ReadNullableDecimal(reader, 3),
                Net_Weight = ReadNullableDecimal(reader, 4),
                Item_Name = ReadString(reader, 5),
                Scientific_Name = ReadString(reader, 6),
                Short_Name = ReadString(reader, 7),
                Item_Status = ReadString(reader, 8),
                Purpose = ReadString(reader, 9),
                HSCode = ReadString(reader, 10),
                GroupName = ReadString(reader, 11),
                SecondaryClassificationName = ReadString(reader, 12),
                MainClassificationName = ReadString(reader, 13),
                ItemTypeName = ReadString(reader, 14),
                FamilyName = ReadString(reader, 15),
                OrderName = ReadString(reader, 16),
                PhylumSubphylumName = ReadString(reader, 17),
                KingdomName = ReadString(reader, 18),
                Im_InitiatorCounteryName = ReadString(reader, 19)
            });
        }
        await reader.DisposeAsync().ConfigureAwait(false);

        foreach (var item in items)
        {
            item.Plant_Part = await ReadPlantPartAsync(connection, item.Item_ShortName_ID, cancellationToken).ConfigureAwait(false);
            item.List_Lots = await ReadLotsAsync(connection, item.ID, cancellationToken).ConfigureAwait(false);
        }

        return items;
    }

    private static async Task<string> ReadPlantPartAsync(SqlConnection connection, long? itemShortNameId, CancellationToken cancellationToken)
    {
        if (!itemShortNameId.HasValue)
            return string.Empty;

        const string itemIdSql = """
            SELECT Item_ID
            FROM dbo.Item_ShortName
            WHERE ID = @ShortNameId;
            """;
        await using var itemCommand = new SqlCommand(itemIdSql, connection) { CommandTimeout = 30 };
        itemCommand.Parameters.Add("@ShortNameId", SqlDbType.BigInt).Value = itemShortNameId.Value;
        var value = await itemCommand.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        if (value is null or DBNull)
            return string.Empty;
        var itemId = Convert.ToInt64(value);

        const string sql = """
            SELECT TOP (1) sp.Name_Ar
            FROM dbo.ItemPart ip
            INNER JOIN dbo.SubPart sp ON sp.ID = ip.SubPart_ID
            WHERE ip.Item_ID = @ItemId
            ORDER BY ip.ID;
            """;
        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 30 };
        command.Parameters.Add("@ItemId", SqlDbType.BigInt).Value = itemId;
        var part = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        return part is null or DBNull ? string.Empty : Convert.ToString(part) ?? string.Empty;
    }

    private static async Task<List<DisplayLotVm>> ReadLotsAsync(SqlConnection connection, long itemId, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT l.ID,
                   COALESCE(l.Lot_Number, N''),
                   COALESCE(category.Name_Ar, N''),
                   COALESCE(packageType.Ar_Name, N''),
                   COALESCE(packageMaterial.Ar_Name, N''),
                   COALESCE(l.DistinctiveMark, N''),
                   COALESCE(l.PackagesCount, N''),
                   l.GrossWeight,
                   l.Net_Weight,
                   l.Based_Weight,
                   l.Package_Weight,
                   l.Package_Based_Weight,
                   l.Package_Net_Weight,
                   l.Units_Number,
                   COALESCE(l.Waybill, N''),
                   COALESCE(l.ShipName, N''),
                   COALESCE(l.Order_Text, N''),
                   l.containers_type_ID,
                   l.TripDate
            FROM dbo.Im_CheckRequest_Items_Lot_Category l
            LEFT JOIN dbo.ItemCategories category ON category.ID = l.ItemCategory_ID
            LEFT JOIN dbo.Package_Type packageType ON packageType.ID = l.Package_Type_ID
            LEFT JOIN dbo.Package_Material packageMaterial ON packageMaterial.ID = l.Package_Material_ID
            WHERE l.Im_CheckRequest_Items_ID = @ItemId
            ORDER BY l.ID;
            """;

        var lots = new List<DisplayLotVm>();
        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 30 };
        command.Parameters.Add("@ItemId", SqlDbType.BigInt).Value = itemId;
        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            lots.Add(new DisplayLotVm
            {
                ID = Convert.ToInt64(reader.GetValue(0)),
                Lot_Number = ReadString(reader, 1),
                categoryName = ReadString(reader, 2),
                packageType = ReadString(reader, 3),
                packageMaterialName = ReadString(reader, 4),
                DistinctiveMark = ReadString(reader, 5),
                PackagesCount = ReadString(reader, 6),
                GrossWeight = ReadNullableDecimal(reader, 7),
                Net_Weight = ReadNullableDecimal(reader, 8),
                Based_Weight = ReadNullableDecimal(reader, 9),
                Package_Weight = ReadNullableDecimal(reader, 10),
                Package_Based_Weight = ReadNullableDecimal(reader, 11),
                Package_Net_Weight = ReadNullableDecimal(reader, 12),
                Units_Number = ReadNullableInt32(reader, 13),
                Waybill = ReadString(reader, 14),
                ShipName = ReadString(reader, 15),
                Order_Text = ReadString(reader, 16),
                containers_type_ID = ReadNullableInt32(reader, 17),
                TripDate = ReadNullableDateTime(reader, 18)
            });
        }
        return lots;
    }

    private static async Task<List<DisplayAttachmentVm>> ReadAttachmentsAsync(SqlConnection connection, long requestId, CancellationToken cancellationToken)
    {
        const string sql = """
            SELECT COALESCE(a.AttachmentPath, N''),
                   COALESCE(att.Ar_Name, N''),
                   COALESCE(a.Attachment_Number, N'')
            FROM dbo.A_AttachmentData a
            LEFT JOIN dbo.A_AttachmentTableType att ON att.ID = a.A_AttachmentTableType_ID
            WHERE a.RowId = @RequestId
              AND a.A_AttachmentTableNameId = 8
            ORDER BY a.ID;
            """;

        var list = new List<DisplayAttachmentVm>();
        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 30 };
        command.Parameters.Add("@RequestId", SqlDbType.BigInt).Value = requestId;
        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            list.Add(new DisplayAttachmentVm
            {
                AttachmentPath = ReadString(reader, 0),
                Attachment_Name = ReadString(reader, 1),
                Attachment_Number = ReadString(reader, 2)
            });
        }
        return list;
    }

    public async Task AcceptRequestAsync(long requestId, bool accepted, CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await using var transaction = (SqlTransaction)await connection.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            const string readSql = """
                SELECT IsAccepted, IsPaid, Completion_Status
                FROM dbo.Im_CheckRequest
                WHERE ID = @RequestId;
                """;
            await using var readCommand = new SqlCommand(readSql, connection, transaction) { CommandTimeout = 30 };
            readCommand.Parameters.Add("@RequestId", SqlDbType.BigInt).Value = requestId;
            await using var reader = await readCommand.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
            if (!await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                throw new InvalidOperationException("السجل غير موجود.");
            var completionStatus = reader.IsDBNull(2) ? (byte?)null : reader.GetByte(2);
            await reader.DisposeAsync().ConfigureAwait(false);

            const string updateSql = """
                UPDATE dbo.Im_CheckRequest
                SET IsAccepted = @Accepted,
                    IsAccepted_Date = GETDATE(),
                    IsActive = @Accepted,
                    Completion_Notes = CASE WHEN @Accepted = 1 THEN NULL ELSE Completion_Notes END,
                    Completion_Status = CASE WHEN Completion_Status = 2 THEN 3 ELSE Completion_Status END
                WHERE ID = @RequestId;
                """;
            await using var updateCommand = new SqlCommand(updateSql, connection, transaction) { CommandTimeout = 30 };
            updateCommand.Parameters.Add("@RequestId", SqlDbType.BigInt).Value = requestId;
            updateCommand.Parameters.Add("@Accepted", SqlDbType.Bit).Value = accepted;
            await updateCommand.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);

            if (accepted)
            {
                const string shippingSql = """
                    UPDATE dbo.Im_CheckRequset_Shipping_Method
                    SET containers_type_ID = 120,
                        containers_ID = 120
                    WHERE Im_CheckRequest_ID = @RequestId;
                    """;
                await using var shippingCommand = new SqlCommand(shippingSql, connection, transaction) { CommandTimeout = 30 };
                shippingCommand.Parameters.Add("@RequestId", SqlDbType.BigInt).Value = requestId;
                await shippingCommand.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
            }

            _ = completionStatus;
            await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
            throw;
        }
    }

    public async Task ContinueRequestAsync(long requestId, string notes, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(notes))
            throw new InvalidOperationException("يرجى كتابة ملاحظات الاستكمال.");

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

        const string sql = """
            UPDATE dbo.Im_CheckRequest
            SET Completion_Notes = @Notes,
                Completion_Status = 1,
                Completion_Request_Date = GETDATE(),
                Completion_Response_Date = NULL,
                Completion_Response_User_Id = NULL
            WHERE ID = @RequestId;
            """;
        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 30 };
        command.Parameters.Add("@RequestId", SqlDbType.BigInt).Value = requestId;
        command.Parameters.Add("@Notes", SqlDbType.NVarChar, -1).Value = notes.Trim();
        if (await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false) != 1)
            throw new InvalidOperationException("السجل غير موجود ولم يتم استكمال الطلب.");
    }

    public async Task AppendHSCodeAsync(long requestItemId, string addition, CancellationToken cancellationToken = default)
    {
        var value = addition?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidOperationException("يرجى كتابة القيمة الجديدة.");
        if (value.Length > 200)
            throw new InvalidOperationException("قيمة HS Code يجب ألا تتجاوز 200 حرف.");

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

        const string sql = """
            UPDATE dbo.Item_ShortName
            SET HSCode = @Addition
            WHERE ID = (
                SELECT Item_ShortName_ID
                FROM dbo.Im_CheckRequest_Items
                WHERE ID = @RequestItemId
            );
            """;
        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 30 };
        command.Parameters.Add("@RequestItemId", SqlDbType.BigInt).Value = requestItemId;
        command.Parameters.Add("@Addition", SqlDbType.NVarChar, -1).Value = value;
        if (await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false) != 1)
            throw new InvalidOperationException("الصنف غير موجود.");
    }

    public async Task MarkCompletionRespondedAsync(long requestId, short userId, CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

        const string sql = """
            UPDATE dbo.Im_CheckRequest
            SET Completion_Status = 2,
                Completion_Response_Date = GETDATE(),
                Completion_Response_User_Id = @UserId,
                User_Updation_Date = GETDATE(),
                User_Updation_Id = @UserId
            WHERE ID = @RequestId
              AND Completion_Status = 1;
            """;
        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 30 };
        command.Parameters.Add("@RequestId", SqlDbType.BigInt).Value = requestId;
        command.Parameters.Add("@UserId", SqlDbType.SmallInt).Value = userId;
        if (await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false) != 1)
            throw new InvalidOperationException("الطلب ليس في انتظار استكمال.");
    }

    private static string ReadString(SqlDataReader reader, int ordinal) =>
        reader.IsDBNull(ordinal) ? string.Empty : Convert.ToString(reader.GetValue(ordinal)) ?? string.Empty;

    private static byte? ReadNullableByte(SqlDataReader reader, int ordinal) =>
        reader.IsDBNull(ordinal) ? null : reader.GetByte(ordinal);

    private static DateTime? ReadNullableDateTime(SqlDataReader reader, int ordinal) =>
        reader.IsDBNull(ordinal) ? null : reader.GetDateTime(ordinal);

    private static bool? ReadNullableBoolean(SqlDataReader reader, int ordinal) =>
        reader.IsDBNull(ordinal) ? null : reader.GetBoolean(ordinal);

    private static long? ReadNullableInt64(SqlDataReader reader, int ordinal) =>
        reader.IsDBNull(ordinal) ? null : Convert.ToInt64(reader.GetValue(ordinal));

    private static int? ReadNullableInt32(SqlDataReader reader, int ordinal) =>
        reader.IsDBNull(ordinal) ? null : Convert.ToInt32(reader.GetValue(ordinal));

    private static decimal? ReadNullableDecimal(SqlDataReader reader, int ordinal) =>
        reader.IsDBNull(ordinal) ? null : Convert.ToDecimal(reader.GetValue(ordinal));
}
