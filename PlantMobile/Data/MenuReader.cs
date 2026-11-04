using System.Data;
using Microsoft.Data.SqlClient;

namespace PlantMobile.Data;

/// <summary>Reads every menu row for one user in a single database round trip.</summary>
public sealed class MenuReader
{
    private readonly string _connectionString;
    private readonly int _applicationCategoryId;
    private readonly bool _respectActiveFlags;
    private readonly bool _onlyEnabledForAndroid;

    public MenuReader(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("PrivilegeDBConnection")
            ?? throw new InvalidOperationException("PrivilegeDBConnection is not configured.");
        _applicationCategoryId = configuration.GetValue("Menu:ApplicationCategoryId", 1);
        _respectActiveFlags = configuration.GetValue("Menu:RespectActiveFlags", false);
        _onlyEnabledForAndroid = configuration.GetValue("Menu:OnlyEnabledForAndroid", true);
    }

    public async Task<IReadOnlyList<MenuRow>> ReadAsync(int userId, bool isArabic, CancellationToken cancellationToken = default)
    {
        await using var connection = new SqlConnection(_connectionString);
        await connection.OpenAsync(cancellationToken).ConfigureAwait(false);

        await using var command = new SqlCommand(MenuSql.Build(_respectActiveFlags, _onlyEnabledForAndroid), connection)
        {
            CommandTimeout = 30
        };
        command.Parameters.Add("@UserId", SqlDbType.SmallInt).Value = userId;
        command.Parameters.Add("@IsArabic", SqlDbType.Bit).Value = isArabic;
        command.Parameters.Add("@ApplicationCategoryId", SqlDbType.Int).Value = _applicationCategoryId;

        await using var reader = await command.ExecuteReaderAsync(cancellationToken).ConfigureAwait(false);
        var rows = new List<MenuRow>();
        while (await reader.ReadAsync(cancellationToken).ConfigureAwait(false))
        {
            rows.Add(new MenuRow(
                GroupId: reader.GetInt32(0),
                GroupName: reader.GetString(1),
                ModuleId: reader.GetInt32(2),
                ModuleName: reader.GetString(3),
                MenuId: reader.GetInt32(4),
                MenuTitle: reader.GetString(5),
                MenuUrl: reader.IsDBNull(6) ? null : reader.GetString(6),
                CanView: ReadFlag(reader, 7),
                CanAdd: ReadFlag(reader, 8),
                CanEdit: ReadFlag(reader, 9),
                CanDelete: ReadFlag(reader, 10)));
        }

        return rows;
    }

    private static bool ReadFlag(SqlDataReader reader, int ordinal) =>
        !reader.IsDBNull(ordinal) && reader.GetBoolean(ordinal);
}

/// <summary>One flat row of the menu query, before it is grouped into a tree.</summary>
public sealed record MenuRow(
    int GroupId,
    string GroupName,
    int ModuleId,
    string ModuleName,
    int MenuId,
    string MenuTitle,
    string? MenuUrl,
    bool CanView,
    bool CanAdd,
    bool CanEdit,
    bool CanDelete);
