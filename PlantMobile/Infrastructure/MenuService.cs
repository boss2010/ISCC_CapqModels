using Microsoft.Extensions.Caching.Memory;
using PlantMobile.Data;
using PlantMobile.Models.Menu;

namespace PlantMobile.Infrastructure;

/// <summary>Provides the navigation menu tree for one user.</summary>
public interface IMenuService
{
    Task<IReadOnlyList<MenuGroup>> GetAsync(int userId, bool isArabic, CancellationToken cancellationToken = default);

    /// <summary>Drops the cached menu of one user after a privilege change.</summary>
    void Invalidate(int userId);
}

/// <summary>
/// Builds the menu tree from one flat query and caches it per user and language.
/// Replaces the three nested child actions used by the legacy web application.
/// </summary>
public sealed class MenuService : IMenuService
{
    private static readonly TimeSpan CacheDuration = TimeSpan.FromMinutes(15);

    private static readonly string[] DefaultExcludedUrls =
    [
        "Im_CheckRequests/Im_Fumigation/ReleaseSupervisorTasks"
    ];

    private readonly MenuReader _reader;
    private readonly IMemoryCache _cache;
    private readonly IReadOnlyCollection<string> _excludedUrls;

    public MenuService(MenuReader reader, IMemoryCache cache, IConfiguration configuration)
    {
        _reader = reader;
        _cache = cache;
        _excludedUrls = configuration.GetSection("Menu:ExcludedUrls").Get<string[]>() ?? DefaultExcludedUrls;
    }

    public async Task<IReadOnlyList<MenuGroup>> GetAsync(int userId, bool isArabic, CancellationToken cancellationToken = default)
    {
        var cacheKey = BuildCacheKey(userId, isArabic);
        if (_cache.TryGetValue(cacheKey, out IReadOnlyList<MenuGroup>? cached) && cached is not null)
        {
            return cached;
        }

        var rows = await _reader.ReadAsync(userId, isArabic, cancellationToken).ConfigureAwait(false);
        var menu = Build(rows, _excludedUrls);
        _cache.Set(cacheKey, menu, CacheDuration);
        return menu;
    }

    public void Invalidate(int userId)
    {
        _cache.Remove(BuildCacheKey(userId, isArabic: true));
        _cache.Remove(BuildCacheKey(userId, isArabic: false));
    }

    /// <summary>Groups the flat rows into Group -> Module -> Items, preserving query order.</summary>
    internal static IReadOnlyList<MenuGroup> Build(IReadOnlyList<MenuRow> rows, IReadOnlyCollection<string> excludedUrls)
    {
        var groups = new List<MenuGroup>();
        var groupsById = new Dictionary<int, MenuGroup>();
        var modulesById = new Dictionary<(int GroupId, int ModuleId), MenuModule>();
        var seenItems = new HashSet<(int GroupId, int ModuleId, int MenuId)>();

        foreach (var row in rows)
        {
            if (IsExcluded(row.MenuUrl, excludedUrls) || !seenItems.Add((row.GroupId, row.ModuleId, row.MenuId)))
            {
                continue;
            }

            if (!groupsById.TryGetValue(row.GroupId, out var group))
            {
                group = new MenuGroup(row.GroupId, row.GroupName);
                groupsById.Add(row.GroupId, group);
                groups.Add(group);
            }

            if (!modulesById.TryGetValue((row.GroupId, row.ModuleId), out var module))
            {
                module = new MenuModule(row.ModuleId, row.ModuleName);
                modulesById.Add((row.GroupId, row.ModuleId), module);
                group.Modules.Add(module);
            }

            module.Items.Add(new MenuItem
            {
                Id = row.MenuId,
                Title = row.MenuTitle,
                Url = (row.MenuUrl ?? string.Empty).Trim(),
                CanView = row.CanView,
                CanAdd = row.CanAdd,
                CanEdit = row.CanEdit,
                CanDelete = row.CanDelete
            });
        }

        return groups;
    }

    private static bool IsExcluded(string? url, IReadOnlyCollection<string> excludedUrls)
    {
        if (string.IsNullOrWhiteSpace(url) || excludedUrls.Count == 0)
        {
            return false;
        }

        var normalized = NormalizeUrl(url);
        foreach (var excluded in excludedUrls)
        {
            if (string.Equals(normalized, NormalizeUrl(excluded), StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    private static string NormalizeUrl(string url) => url.Trim().TrimStart('~', '/').TrimEnd('/');

    private static string BuildCacheKey(int userId, bool isArabic) => $"menu:{userId}:{(isArabic ? "ar" : "en")}";
}
