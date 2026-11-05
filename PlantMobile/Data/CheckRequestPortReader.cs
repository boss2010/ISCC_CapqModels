using System.Data;
using Microsoft.Data.SqlClient;
using PlantMobile.Models;

namespace PlantMobile.Data;

/// <summary>Reads and updates the check request ports page.</summary>
public sealed class CheckRequestPortReader
{
    private readonly string _connectionString;

    public CheckRequestPortReader(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DBConnection")
            ?? throw new InvalidOperationException("DBConnection is not configured.");
    }

    public async Task<List<ImCheckRequestPortRowVm>> ReadByCheckNumberAsync(string checkNumber, CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT cr.ID,
                   cr.CheckRequest_Number,
                   p.ReqPortType_ID,
                   COALESCE(sc.ValueName, N''),
                   p.ID,
                   p.Port_ID,
                   CASE WHEN p.ReqPortType_ID = 10 THEN pn.Govern_ID ELSE inp.Country_ID END,
                   CASE WHEN p.ReqPortType_ID = 10 THEN COALESCE(gov.Ar_Name, N'') ELSE COALESCE(country.Ar_Name, N'') END,
                   CASE WHEN p.ReqPortType_ID = 10 THEN COALESCE(pn.Name_Ar, N'') ELSE COALESCE(inp.Name_Ar, N'') END,
                   COALESCE(country.Ar_Name, N''),
                   MAX(CASE WHEN c.Status IS NULL THEN 0 ELSE 1 END),
                   MAX(CASE WHEN c.Status IS NULL THEN 0 ELSE CAST(c.Status AS int) END)
            FROM dbo.Im_CheckRequest cr
            INNER JOIN dbo.Im_CheckRequest_Data data ON data.Im_CheckRequest_ID = cr.ID
            INNER JOIN dbo.Im_CheckRequest_Port p ON p.Im_CheckRequest_Data_ID = data.ID
            LEFT JOIN dbo.A_SystemCode sc ON sc.Id = p.ReqPortType_ID
            LEFT JOIN dbo.PortNational pn ON pn.ID = p.Port_ID AND p.ReqPortType_ID = 10
            LEFT JOIN dbo.Governate gov ON gov.ID = pn.Govern_ID
            LEFT JOIN dbo.Port_International inp ON inp.ID = p.Port_ID AND p.ReqPortType_ID IN (9, 11)
            LEFT JOIN dbo.Country country ON country.ID = inp.Country_ID
            LEFT JOIN dbo.Im_RequestCommittee c ON c.ImCheckRequest_ID = cr.ID
            WHERE cr.CheckRequest_Number = @CheckNumber
              AND p.Port_ID IS NOT NULL
            GROUP BY cr.ID,
                     cr.CheckRequest_Number,
                     p.ReqPortType_ID,
                     sc.ValueName,
                     p.ID,
                     p.Port_ID,
                     CASE WHEN p.ReqPortType_ID = 10 THEN pn.Govern_ID ELSE inp.Country_ID END,
                     CASE WHEN p.ReqPortType_ID = 10 THEN COALESCE(gov.Ar_Name, N'') ELSE COALESCE(country.Ar_Name, N'') END,
                     CASE WHEN p.ReqPortType_ID = 10 THEN COALESCE(pn.Name_Ar, N'') ELSE COALESCE(inp.Name_Ar, N'') END,
                     country.Ar_Name
            ORDER BY CASE WHEN p.ReqPortType_ID = 11 THEN 1 WHEN p.ReqPortType_ID = 10 THEN 2 WHEN p.ReqPortType_ID = 9 THEN 3 ELSE 4 END;
            """;

        var rows = new List<ImCheckRequestPortRowVm>();
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 60 };
        command.Parameters.Add("@CheckNumber", SqlDbType.VarChar, 50).Value = checkNumber?.Trim() ?? string.Empty;

        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            rows.Add(new ImCheckRequestPortRowVm
            {
                CheckRequestId = Convert.ToInt64(reader.GetValue(0)),
                CheckRequestNumber = Convert.ToString(reader.GetValue(1)) ?? string.Empty,
                ReqPortTypeId = reader.IsDBNull(2) ? 0 : Convert.ToInt32(reader.GetValue(2)),
                PortTypeName = ReadString(reader, 3),
                CheckRequestPortId = Convert.ToInt64(reader.GetValue(4)),
                PortId = reader.IsDBNull(5) ? null : Convert.ToInt32(reader.GetValue(5)),
                GovernId = reader.IsDBNull(6) ? null : Convert.ToInt32(reader.GetValue(6)),
                GovernName = ReadString(reader, 7),
                PortNameAr = ReadString(reader, 8),
                CountryArName = ReadString(reader, 9),
                CommitteeFormed = !reader.IsDBNull(10) && Convert.ToBoolean(reader.GetValue(10)),
                CommitteeStatus = reader.IsDBNull(11) ? null : Convert.ToInt32(reader.GetValue(11)) == 1
            });
        }
        return rows;
    }

    public async Task<List<PortOptionVm>> GetCountryOptionsAsync(CancellationToken cancellationToken = default) =>
        await ReadOptionListAsync(
            """
            SELECT ID, Ar_Name
            FROM dbo.Country
            WHERE IsActive = 1
              AND User_Deletion_Id IS NULL
              AND User_Deletion_Date IS NULL
            ORDER BY Ar_Name;
            """,
            "----------",
            cancellationToken).ConfigureAwait(false);

    public async Task<List<PortOptionVm>> GetGovernmentOptionsAsync(CancellationToken cancellationToken = default) =>
        await ReadOptionListAsync(
            """
            SELECT ID, Ar_Name
            FROM dbo.Governate
            WHERE IsActive = 1
              AND User_Deletion_Id IS NULL
              AND User_Deletion_Date IS NULL
            ORDER BY Ar_Name;
            """,
            "----------",
            cancellationToken).ConfigureAwait(false);

    public async Task<List<PortOptionVm>> GetNationalPortsAsync(int governmentId, CancellationToken cancellationToken = default) =>
        await ReadOptionListAsync(
            """
            SELECT ID, Name_Ar
            FROM dbo.PortNational
            WHERE Govern_ID = @GovernmentId
            ORDER BY Name_Ar;
            """,
            "----------",
            cancellationToken,
            ("@GovernmentId", governmentId, SqlDbType.Int)).ConfigureAwait(false);

    public async Task<List<PortOptionVm>> GetInternationalPortsAsync(int countryId, CancellationToken cancellationToken = default) =>
        await ReadOptionListAsync(
            """
            SELECT ID, Name_Ar
            FROM dbo.Port_International
            WHERE Country_ID = @CountryId
            ORDER BY Name_Ar;
            """,
            "----------",
            cancellationToken,
            ("@CountryId", countryId, SqlDbType.SmallInt)).ConfigureAwait(false);

    public async Task UpdatePortsAsync(UpdatePortsRequestVm request, short userId, CancellationToken cancellationToken = default)
    {
        if (request.list_of_values.Count == 0)
            return;

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await using var transaction = (SqlTransaction)await connection.BeginTransactionAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            foreach (var item in request.list_of_values)
            {
                if (item.Im_CheckRequest_Port_ID <= 0 || item.Port_ID <= 0)
                    throw new InvalidOperationException("بيانات الميناء غير صحيحة.");

                const string readSql = """
                    SELECT cp.ID, cp.ReqPortType_ID, cr.ID, cr.Outlet_ID
                    FROM dbo.Im_CheckRequest_Port cp
                    INNER JOIN dbo.Im_CheckRequest_Data data ON data.ID = cp.Im_CheckRequest_Data_ID
                    INNER JOIN dbo.Im_CheckRequest cr ON cr.ID = data.Im_CheckRequest_ID
                    WHERE cp.ID = @PortRowId;
                    """;
                short portTypeId;
                long checkRequestId;
                await using (var readCommand = new SqlCommand(readSql, connection, transaction) { CommandTimeout = 30 })
                {
                    readCommand.Parameters.Add("@PortRowId", SqlDbType.Int).Value = item.Im_CheckRequest_Port_ID;
                    await using var reader = await readCommand.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
                    if (!await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
                        throw new InvalidOperationException("سجل الميناء غير موجود.");
                    portTypeId = Convert.ToInt16(reader.GetValue(1));
                    checkRequestId = Convert.ToInt64(reader.GetValue(2));
                }

                long portRowId = item.Im_CheckRequest_Port_ID;
                long newPortId = item.Port_ID;

                const string updateRequestSql = """
                    UPDATE dbo.Im_CheckRequest
                    SET Outlet_ID = NULL
                    WHERE ID = @RequestId;
                    """;
                await using (var updateRequest = new SqlCommand(updateRequestSql, connection, transaction) { CommandTimeout = 30 })
                {
                    updateRequest.Parameters.Add("@RequestId", SqlDbType.BigInt).Value = checkRequestId;
                    await updateRequest.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
                }

                byte portType;
                string note;
                if (portTypeId == 10)
                {
                    const string typeSql = "SELECT PortTypeID FROM dbo.PortNational WHERE ID = @PortId;";
                    portType = await ReadPortTypeAsync(connection, transaction, typeSql, newPortId, cancellationToken).ConfigureAwait(false);
                    note = " تم تعديل ميناءالوصول ";
                }
                else if (portTypeId == 9)
                {
                    const string typeSql = "SELECT PortTypeID FROM dbo.Port_International WHERE ID = @PortId;";
                    portType = await ReadPortTypeAsync(connection, transaction, typeSql, newPortId, cancellationToken).ConfigureAwait(false);
                    note = " تم تعديل ميناءالشحن ";
                }
                else if (portTypeId == 11)
                {
                    const string typeSql = "SELECT PortTypeID FROM dbo.Port_International WHERE ID = @PortId;";
                    portType = await ReadPortTypeAsync(connection, transaction, typeSql, newPortId, cancellationToken).ConfigureAwait(false);
                    note = " تم تعديل ميناءالعبور ";
                }
                else
                {
                    throw new InvalidOperationException("نوع الميناء غير معروف.");
                }

                const string updatePortSql = """
                    UPDATE dbo.Im_CheckRequest_Port
                    SET Port_ID = @PortId,
                        Port_Type_ID = @PortTypeId,
                        User_Updation_Id = @UserId,
                        User_Updation_Date = GETDATE()
                    WHERE ID = @PortRowId;
                    """;
                await using (var updatePort = new SqlCommand(updatePortSql, connection, transaction) { CommandTimeout = 30 })
                {
                    updatePort.Parameters.Add("@PortId", SqlDbType.Int).Value = (int)newPortId;
                    updatePort.Parameters.Add("@PortTypeId", SqlDbType.TinyInt).Value = portType;
                    updatePort.Parameters.Add("@UserId", SqlDbType.SmallInt).Value = userId;
                    updatePort.Parameters.Add("@PortRowId", SqlDbType.Int).Value = (int)portRowId;
                    if (await updatePort.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false) != 1)
                        throw new InvalidOperationException("تعذر تحديث الميناء.");
                }

                const string logSql = """
                    INSERT INTO dbo.Table_Action_Log_CheckRequest
                        (ID, ID_Table_Action, ID_TableActionValue, Im_CheckRequest_ID,
                         User_Creation_Id, User_Creation_Date, NOTS, User_Type_ID, Type_log_ID)
                    VALUES
                        (NEXT VALUE FOR dbo.Table_Action_Log_CheckRequest_SEQ, 41, @PortRowId, @RequestId,
                         @UserId, GETDATE(), @Note, 127, 135);
                    """;
                await using (var logCommand = new SqlCommand(logSql, connection, transaction) { CommandTimeout = 30 })
                {
                    logCommand.Parameters.Add("@PortRowId", SqlDbType.BigInt).Value = portRowId;
                    logCommand.Parameters.Add("@RequestId", SqlDbType.BigInt).Value = checkRequestId;
                    logCommand.Parameters.Add("@UserId", SqlDbType.BigInt).Value = userId;
                    logCommand.Parameters.Add("@Note", SqlDbType.NVarChar, -1).Value = note;
                    await logCommand.ExecuteNonQueryAsync(cancellationToken).ConfigureAwait(false);
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

    private async Task<List<PortOptionVm>> ReadOptionListAsync(
        string sql,
        string separatorText,
        CancellationToken cancellationToken,
        params (string Name, object Value, SqlDbType Type)[] parameters)
    {
        var list = new List<PortOptionVm> { new() { DisplayText = separatorText, Value = null } };

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 30 };
        foreach (var parameter in parameters)
            command.Parameters.Add(parameter.Name, parameter.Type).Value = parameter.Value;

        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            list.Add(new PortOptionVm
            {
                Value = Convert.ToInt64(reader.GetValue(0)),
                DisplayText = ReadString(reader, 1)
            });
        }
        return list;
    }

    private static async Task<byte> ReadPortTypeAsync(SqlConnection connection, SqlTransaction transaction, string sql, long portId, CancellationToken cancellationToken)
    {
        await using var command = new SqlCommand(sql, connection, transaction) { CommandTimeout = 30 };
        command.Parameters.Add("@PortId", SqlDbType.Int).Value = (int)portId;
        var value = await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false);
        if (value is null or DBNull)
            throw new InvalidOperationException("الميناء المحددة غير موجودة.");
        return Convert.ToByte(value);
    }

    private static string ReadString(SqlDataReader reader, int ordinal) =>
        reader.IsDBNull(ordinal) ? string.Empty : Convert.ToString(reader.GetValue(ordinal)) ?? string.Empty;
}
