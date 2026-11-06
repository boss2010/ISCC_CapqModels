using System.Data;
using Microsoft.Data.SqlClient;
using PlantMobile.Models;

namespace PlantMobile.Data;

/// <summary>
/// Data access for the two permission screens that used to call the legacy Web API:
///   Im_Permissions/IM_Permission_Active_Print  (re-enable / reprint a permission)
///   Im_Permissions/Stopping_Permission         (stop a permission)
/// The queries and updates mirror Stopping_PermissionBLL and List_ImPermissions_NewBLL
/// of the legacy web application.
/// </summary>
public sealed class PermissionScreenReader
{
    private readonly string _connectionString;

    public PermissionScreenReader(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DBConnection")
            ?? throw new InvalidOperationException("DBConnection is not configured.");
    }

    /// <summary>Loads one permission row by its printed permission number (legacy: GetImPermissionsList_filter_ActivePrint).</summary>
    public async Task<PermissionScreenRow?> ReadAsync(decimal permissionNumber, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT TOP (1)
                   cc.ID,
                   CONVERT(nvarchar(100), cc.ImPermission_Number),
                   cc.Arrival_Date,
                   cc.IS_Print_Ar,
                   cc.IS_Print_EN,
                   cc.IsPaid,
                   cc.IsAcceppted,
                   cc.Im_CheckRequest_ID,
                   COALESCE(operation.Name_Ar, N''),
                   COALESCE(country.Ar_Name, N''),
                   COALESCE(systemCode.ValueName, N''),
                   CASE rr.ImporterType_Id
                        WHEN 6 THEN COALESCE(company.Name_Ar, N'')
                        WHEN 7 THEN COALESCE(organization.Name_Ar, N'')
                        WHEN 8 THEN COALESCE(person.Name, N'')
                        ELSE N''
                   END,
                   COALESCE(item.shortName, N'')
            FROM dbo.Im_PermissionRequest cc
            INNER JOIN dbo.Im_RequestData rr ON rr.Im_PermissionRequest_ID = cc.ID
            LEFT JOIN dbo.Im_OpertaionType operation ON operation.ID = rr.Im_OperationType
            LEFT JOIN dbo.Country country ON country.ID = rr.ExportCountry_Id
            LEFT JOIN dbo.A_SystemCode systemCode ON systemCode.Id = rr.ImporterType_Id
            LEFT JOIN dbo.Company_National company ON company.ID = rr.Importer_ID AND rr.ImporterType_Id = 6
            LEFT JOIN dbo.Public_Organization organization ON organization.ID = rr.Importer_ID AND rr.ImporterType_Id = 7
            LEFT JOIN dbo.Person person ON person.ID = rr.Importer_ID AND rr.ImporterType_Id = 8
            OUTER APPLY (
                SELECT TOP (1)
                       COALESCE(NULLIF(shortName.ShortName_Ar, N''), groupName.Name_Ar, N'') AS shortName
                FROM dbo.Im_PermissionItems pi
                LEFT JOIN dbo.Im_Initiator initiator ON initiator.ID = pi.Im_Initiator_ID
                LEFT JOIN dbo.Item_ShortName shortName ON shortName.ID = initiator.Item_ShortName_ID
                LEFT JOIN dbo.QualitativeGroup groupName ON groupName.Id = initiator.QualitativeGroup_Id
                WHERE pi.Im_PermissionRequest_ID = cc.ID
                ORDER BY pi.ID
            ) item
            WHERE cc.ImPermission_Number = @PermissionNumber
            ORDER BY cc.ID DESC;
            """;

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 30 };
        command.Parameters.Add("@PermissionNumber", SqlDbType.Decimal).Value = permissionNumber;

        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (!await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            return null;

        return new PermissionScreenRow
        {
            Im_PermissionRequest_ID = Convert.ToInt64(reader.GetValue(0)),
            ImPermission_Number = ReadString(reader, 1),
            Arrival_Date = reader.IsDBNull(2) ? null : Convert.ToDateTime(reader.GetValue(2)),
            IS_Print_Ar = ReadNullableBoolean(reader, 3),
            IS_Print_EN = ReadNullableBoolean(reader, 4),
            IsPaid = ReadNullableBoolean(reader, 5),
            IsAcceppted = ReadNullableBoolean(reader, 6),
            Im_CheckRequest_ID = reader.IsDBNull(7) ? null : Convert.ToInt64(reader.GetValue(7)),
            OperationTypeName = ReadString(reader, 8),
            ExportCountryName = ReadString(reader, 9),
            ImporterTypeName = ReadString(reader, 10),
            ImporterName = ReadString(reader, 11),
            ShortName = ReadString(reader, 12)
        };
    }

    /// <summary>
    /// Sets the Arabic/English reprint flags and writes the two audit rows (actions 45 and 46),
    /// the same behavior as IM_Permission_Active_PrintController.UpdateUserDatal in the legacy app.
    /// </summary>
    public async Task UpdatePrintFlagsAsync(long permissionId, bool printArabic, bool printEnglish, short userId,
        string notesArabic, string notesEnglish, CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await using var transaction = (SqlTransaction)await connection.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);
        try
        {
            const string updateSql = """
                UPDATE dbo.Im_PermissionRequest
                SET IS_Print_Ar = @PrintArabic,
                    IS_Print_EN = @PrintEnglish
                WHERE ID = @PermissionId;
                """;
            await using var updateCommand = new SqlCommand(updateSql, connection, transaction) { CommandTimeout = 30 };
            updateCommand.Parameters.Add("@PrintArabic", SqlDbType.Bit).Value = printArabic;
            updateCommand.Parameters.Add("@PrintEnglish", SqlDbType.Bit).Value = printEnglish;
            updateCommand.Parameters.Add("@PermissionId", SqlDbType.BigInt).Value = permissionId;
            if (await updateCommand.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false) != 1)
                throw new InvalidOperationException("تعذر تحديث بيانات الإذن.");

            const string logSql = """
                INSERT INTO dbo.Table_Action_Log
                    (ID, ID_Table_Action, NOTS, ID_TableActionValue, Im_PermissionRequest_ID,
                     User_Creation_Id, User_Creation_Date, User_Type_ID, Type_log_ID)
                VALUES
                    (NEXT VALUE FOR dbo.Table_Action_Log_seq, @ActionId, @Note, @PermissionId, @PermissionId,
                     @UserId, GETDATE(), 127, 135);
                """;
            foreach (var (actionId, note) in new[] { ((short)45, notesArabic), ((short)46, notesEnglish) })
            {
                await using var logCommand = new SqlCommand(logSql, connection, transaction) { CommandTimeout = 30 };
                logCommand.Parameters.Add("@ActionId", SqlDbType.SmallInt).Value = actionId;
                logCommand.Parameters.Add("@Note", SqlDbType.NVarChar, 200).Value = string.IsNullOrWhiteSpace(note) ? "اعادة طباعة الاذن" : note;
                logCommand.Parameters.Add("@PermissionId", SqlDbType.BigInt).Value = permissionId;
                logCommand.Parameters.Add("@UserId", SqlDbType.BigInt).Value = userId;
                await logCommand.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
            }

            await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
            throw;
        }
    }

    /// <summary>
    /// Stops a permission by clearing Im_CheckRequest_ID (legacy Stopping_PermissionBLL.Update).
    /// Returns the number of affected rows.
    /// </summary>
    public async Task<int> StopAsync(decimal permissionNumber, CancellationToken cancellationToken = default)
    {
        const string sql = """
            UPDATE dbo.Im_PermissionRequest
            SET Im_CheckRequest_ID = 0
            WHERE ImPermission_Number = @PermissionNumber;
            """;

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 30 };
        command.Parameters.Add("@PermissionNumber", SqlDbType.Decimal).Value = permissionNumber;
        return await command.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
    }

    private static string ReadString(SqlDataReader reader, int ordinal) =>
        reader.IsDBNull(ordinal) ? string.Empty : Convert.ToString(reader.GetValue(ordinal)) ?? string.Empty;

    private static bool? ReadNullableBoolean(SqlDataReader reader, int ordinal) =>
        reader.IsDBNull(ordinal) ? null : Convert.ToBoolean(reader.GetValue(ordinal));
}
