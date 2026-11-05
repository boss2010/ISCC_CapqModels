using System.Data;
using Microsoft.Data.SqlClient;

namespace PlantMobile.Data;

/// <summary>Reads the effective CanView/CanPrint flags for the permission list menu.</summary>
public sealed class PermissionMenuAccessReader
{
    private readonly string _connectionString;
    private readonly int _applicationCategoryId;

    public PermissionMenuAccessReader(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("PrivilegeDBConnection")
            ?? throw new InvalidOperationException("PrivilegeDBConnection is not configured.");
        _applicationCategoryId = configuration.GetValue("Menu:ApplicationCategoryId", 1);
    }

    public async Task<(bool? CanView, bool? CanPrint)> ReadListPermissionsAsync(int userId, CancellationToken cancellationToken = default)
    {
        const string sql = """
            WITH UserMenus AS (
                SELECT prv.PR_GroupId, prv.PR_ModuleId, prv.PR_MenuId,
                       CAST(MAX(CAST(prv.CanView AS tinyint)) AS bit) AS CanView,
                       CAST(MAX(CAST(prv.CanPrint AS tinyint)) AS bit) AS CanPrint
                FROM dbo.PR_GroupModuleMenuPrivilage prv
                WHERE prv.PR_User_id = @UserId
                  AND prv.IS_Active = 1
                GROUP BY prv.PR_GroupId, prv.PR_ModuleId, prv.PR_MenuId
            )
            SELECT MAX(CAST(um.CanView AS tinyint)),
                   MAX(CAST(um.CanPrint AS tinyint))
            FROM UserMenus um
            INNER JOIN dbo.PR_Group g ON g.Id = um.PR_GroupId
            INNER JOIN dbo.PR_Application app ON app.Id = g.PR_ApplicationId
            INNER JOIN dbo.PR_Menu mn ON mn.Id = um.PR_MenuId
            WHERE g.Active = 1
              AND app.PR_ApplicationCategoryId = @ApplicationCategoryId
              AND REPLACE(REPLACE(REPLACE(LOWER(LTRIM(RTRIM(COALESCE(mn.MenuURL, N'')))), N'~', N''), N'\\', N'/'), N' ', N'')
                  LIKE N'%/Im_Permissions/ListIm_Permission/Index%';
            """;

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 30 };
        command.Parameters.Add("@UserId", SqlDbType.SmallInt).Value = (short)userId;
        command.Parameters.Add("@ApplicationCategoryId", SqlDbType.Int).Value = _applicationCategoryId;

        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        if (!await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
            return (null, null);

        return (
            CanView: reader.IsDBNull(0) ? null : Convert.ToInt32(reader.GetValue(0)) == 1,
            CanPrint: reader.IsDBNull(1) ? null : Convert.ToInt32(reader.GetValue(1)) == 1);
    }
}
