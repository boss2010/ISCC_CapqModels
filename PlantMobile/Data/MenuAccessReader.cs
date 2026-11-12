using System.Data;
using Microsoft.Data.SqlClient;

namespace PlantMobile.Data;

public sealed class MenuAccessReader
{
    public const string ShippingDateMenuUrl = "Im_CheckRequests/ShippingDate/Index";

    private readonly string _connectionString;
    private readonly int _applicationCategoryId;

    public MenuAccessReader(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("PrivilegeDBConnection")
            ?? throw new InvalidOperationException("PrivilegeDBConnection is not configured.");
        _applicationCategoryId = configuration.GetValue("Menu:ApplicationCategoryId", 1);
    }

    public async Task<bool> CanViewAsync(
        short userId,
        string menuUrl,
        CancellationToken cancellationToken = default)
    {
        const string sql = """
            SELECT CASE WHEN EXISTS
            (
                SELECT 1
                FROM dbo.PR_GroupModuleMenuPrivilage privilege
                INNER JOIN dbo.PR_Group [group] ON [group].Id = privilege.PR_GroupId
                INNER JOIN dbo.PR_Application application ON application.Id = [group].PR_ApplicationId
                INNER JOIN dbo.PR_Menu menu ON menu.Id = privilege.PR_MenuId
                WHERE privilege.PR_User_id = @UserId
                  AND privilege.IS_Active = 1
                  AND privilege.CanView = 1
                  AND [group].Active = 1
                  AND application.PR_ApplicationCategoryId = @ApplicationCategoryId
                  AND LOWER(REPLACE(REPLACE(LTRIM(RTRIM(menu.MenuURL)), N'~', N''), N'\\', N'/'))
                      = LOWER(REPLACE(REPLACE(LTRIM(RTRIM(@MenuUrl)), N'~', N''), N'\\', N'/'))
            ) THEN 1 ELSE 0 END;
            """;

        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);
        await using var command = new SqlCommand(sql, connection) { CommandTimeout = 30 };
        command.Parameters.Add("@UserId", SqlDbType.SmallInt).Value = userId;
        command.Parameters.Add("@ApplicationCategoryId", SqlDbType.Int).Value = _applicationCategoryId;
        command.Parameters.Add("@MenuUrl", SqlDbType.NVarChar, 500).Value = menuUrl;
        return Convert.ToInt32(await command.ExecuteScalarAsync(cancellationToken).ConfigureAwait(false)) == 1;
    }
}
