using System.Data;
using Microsoft.Data.SqlClient;
using PlantMobile.Models;

namespace PlantMobile.Data;

/// <summary>Reads and updates the check-request outlet assignment page.</summary>
public sealed class CheckRequestOutletReader
{
    private readonly string _connectionString;
    private readonly string _privilegeConnectionString;

    public CheckRequestOutletReader(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DBConnection")
            ?? throw new InvalidOperationException("DBConnection is not configured.");
        _privilegeConnectionString = configuration.GetConnectionString("PrivilegeDBConnection")
            ?? throw new InvalidOperationException("PrivilegeDBConnection is not configured.");
    }

    public record OutletsForUserResult(long? PortNationalId, List<CheckRequestOutletRowVm> Requests, List<PortOptionVm> OutletOptions);

    public async Task<OutletsForUserResult> ReadForUserAsync(short userId, CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

        await using var privilegeConnection = new SqlConnection(_privilegeConnectionString);
        await privilegeConnection.OpenAsync(cancellationToken).ConfigureAwait(false);

        const string userSql = """
            SELECT TOP (1) Outlet_ID
            FROM dbo.PR_User
            WHERE Id = @UserId;
            """;
        long? outletId = null;
        await using (var userCommand = new SqlCommand(userSql, privilegeConnection) { CommandTimeout = 30 })
        {
            userCommand.Parameters.Add("@UserId", SqlDbType.SmallInt).Value = userId;
            var value = await userCommand.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
            if (value is not null and not DBNull)
                outletId = Convert.ToInt64(value);
        }

        if (!outletId.HasValue)
            return new OutletsForUserResult(null, new List<CheckRequestOutletRowVm>(), new List<PortOptionVm>());

        const string outletSql = """
            SELECT TOP (1) PortNational_ID
            FROM dbo.Outlet
            WHERE ID_HR = @OutletId
              AND PortNational_ID IS NOT NULL;
            """;
        long? portNationalId = null;
        await using (var outletCommand = new SqlCommand(outletSql, connection) { CommandTimeout = 30 })
        {
            outletCommand.Parameters.Add("@OutletId", SqlDbType.BigInt).Value = outletId.Value;
            var value = await outletCommand.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
            if (value is not null and not DBNull)
                portNationalId = Convert.ToInt64(value);
        }

        if (!portNationalId.HasValue)
            return new OutletsForUserResult(null, new List<CheckRequestOutletRowVm>(), new List<PortOptionVm>());

        const string requestSql = """
            SELECT DISTINCT cr.ID,
                   cr.CheckRequest_Number,
                   pn.ID,
                   COALESCE(pn.Name_Ar, N''),
                   pn.Govern_ID,
                   COALESCE(gov.Ar_Name, N'')
            FROM dbo.Im_CheckRequest cr
            INNER JOIN dbo.Im_CheckRequest_Data data ON data.Im_CheckRequest_ID = cr.ID
            INNER JOIN dbo.Im_CheckRequest_Port p ON p.Im_CheckRequest_Data_ID = data.ID
            INNER JOIN dbo.PortNational pn ON pn.ID = p.Port_ID
            INNER JOIN dbo.Governate gov ON gov.ID = pn.Govern_ID
            WHERE cr.Outlet_ID IS NULL
              AND p.ReqPortType_ID = 10
              AND pn.ID = @PortNationalId
              AND p.Port_ID IS NOT NULL
            ORDER BY cr.ID;
            """;
        var requests = new List<CheckRequestOutletRowVm>();
        await using (var requestCommand = new SqlCommand(requestSql, connection) { CommandTimeout = 60 })
        {
            requestCommand.Parameters.Add("@PortNationalId", SqlDbType.Int).Value = (int)portNationalId.Value;
            await using var reader = await requestCommand.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                requests.Add(new CheckRequestOutletRowVm
                {
                    ID = Convert.ToInt64(reader.GetValue(0)),
                    ImCheckRequest_Number = ReadString(reader, 1),
                    Port_ID = reader.IsDBNull(2) ? null : Convert.ToInt32(reader.GetValue(2)),
                    Port_Name = ReadString(reader, 3),
                    Gov_ID = reader.IsDBNull(4) ? 0 : Convert.ToInt32(reader.GetValue(4)),
                    Gov_Name = ReadString(reader, 5)
                });
            }
        }

        const string outletListSql = """
            SELECT DISTINCT o.ID, COALESCE(o.Ar_Name, N'')
            FROM dbo.Outlet o
            WHERE o.PortNational_ID = @PortNationalId
              AND o.User_Deletion_Id IS NULL
              AND COALESCE(o.IsExport, 0) <> 80
            ORDER BY COALESCE(o.Ar_Name, N'');
            """;
        var options = new List<PortOptionVm> { new() { DisplayText = "----------", Value = null } };
        await using (var optionCommand = new SqlCommand(outletListSql, connection) { CommandTimeout = 30 })
        {
            optionCommand.Parameters.Add("@PortNationalId", SqlDbType.Int).Value = (int)portNationalId.Value;
            await using var reader = await optionCommand.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
            while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            {
                options.Add(new PortOptionVm
                {
                    Value = Convert.ToInt64(reader.GetValue(0)),
                    DisplayText = ReadString(reader, 1)
                });
            }
        }

        return new OutletsForUserResult(portNationalId, requests, options);
    }

    public async Task UpdateOutletAssignmentsAsync(List<UpdateOutletItemVm> items, short userId, CancellationToken cancellationToken = default)
    {
        if (items.Count == 0)
            return;

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await using var transaction = (SqlTransaction)await connection.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            foreach (var item in items)
            {
                if (item.ID <= 0 || item.Outlet_ID <= 0)
                    throw new InvalidOperationException("بيانات المنفذ غير صحيحة.");

                const string readSql = """
                    SELECT TOP (1) Outlet_ID
                    FROM dbo.Im_CheckRequest
                    WHERE ID = @RequestId;
                    """;
                await using (var readCommand = new SqlCommand(readSql, connection, transaction) { CommandTimeout = 30 })
                {
                    readCommand.Parameters.Add("@RequestId", SqlDbType.BigInt).Value = item.ID;
                    var value = await readCommand.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
                    if (value is null or DBNull)
                        throw new InvalidOperationException("طلب الفحص غير موجود.");
                }

                const string updateSql = """
                    UPDATE dbo.Im_CheckRequest
                    SET Outlet_ID = @OutletId,
                        User_Updation_Id = @UserId,
                        User_Updation_Date = GETDATE()
                    WHERE ID = @RequestId;
                    """;
                await using (var updateCommand = new SqlCommand(updateSql, connection, transaction) { CommandTimeout = 30 })
                {
                    updateCommand.Parameters.Add("@RequestId", SqlDbType.BigInt).Value = item.ID;
                    updateCommand.Parameters.Add("@OutletId", SqlDbType.BigInt).Value = item.Outlet_ID;
                    updateCommand.Parameters.Add("@UserId", SqlDbType.SmallInt).Value = userId;
                    if (await updateCommand.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false) != 1)
                        throw new InvalidOperationException("تعذر تحديث المنفذ.");
                }

                const string logSql = """
                    INSERT INTO dbo.Table_Action_Log_CheckRequest
                        (ID, ID_Table_Action, ID_TableActionValue, Im_CheckRequest_ID,
                         User_Creation_Id, User_Creation_Date, NOTS, User_Type_ID, Type_log_ID)
                    VALUES
                        (NEXT VALUE FOR dbo.Table_Action_Log_CheckRequest_SEQ, 11, @RequestId, @RequestId,
                         @UserId, GETDATE(), N'تحويل لمنفذ', 127, 135);
                    """;
                await using (var logCommand = new SqlCommand(logSql, connection, transaction) { CommandTimeout = 30 })
                {
                    logCommand.Parameters.Add("@RequestId", SqlDbType.BigInt).Value = item.ID;
                    logCommand.Parameters.Add("@UserId", SqlDbType.BigInt).Value = userId;
                    if (await logCommand.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false) != 1)
                        throw new InvalidOperationException("تعذر تسجيل تحويل المنفذ.");
                }
            }

            await transaction.CommitAsync(cancellationToken).ConfigureAwait(false);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken).ConfigureAwait(false);
            throw;
        }
    }

    private static string ReadString(SqlDataReader reader, int ordinal) =>
        reader.IsDBNull(ordinal) ? string.Empty : Convert.ToString(reader.GetValue(ordinal)) ?? string.Empty;
}
