namespace PlantMobile.Models.Menu;

/// <summary>
/// The menu tree prepared for one request: every row knows whether it is the page being
/// displayed, so the view stays free of URL comparison logic.
/// </summary>
public sealed class MenuViewModel
{
    private MenuViewModel(IReadOnlyList<MenuGroupView> groups) => Groups = groups;

    public IReadOnlyList<MenuGroupView> Groups { get; }

    public bool IsEmpty => Groups.Count == 0;

    public static MenuViewModel Create(IReadOnlyList<MenuGroup> groups, HttpRequest request)
    {
        var path = (request.Path.Value ?? string.Empty).TrimEnd('/');
        var query = request.QueryString.Value ?? string.Empty;
        var pathBase = request.PathBase.Value ?? string.Empty;

        var groupViews = new List<MenuGroupView>(groups.Count);

        foreach (var group in groups)
        {
            var moduleViews = new List<MenuModuleView>(group.Modules.Count);

            foreach (var module in group.Modules)
            {
                var itemViews = new List<MenuItemView>(module.Items.Count);

                foreach (var item in module.Items)
                {
                    var (itemPath, itemQuery) = SplitUrl(item.Url);
                    var isActive = itemPath.Length > 0
                        && string.Equals(path, "/" + itemPath, StringComparison.OrdinalIgnoreCase)
                        && (itemQuery.Length == 0 || string.Equals(query, itemQuery, StringComparison.OrdinalIgnoreCase));

                    var href = item.IsContainer ? "#" : pathBase + "/" + itemPath + itemQuery;
                    itemViews.Add(new MenuItemView(item, isActive, href));
                }

                moduleViews.Add(new MenuModuleView(module, itemViews.Any(i => i.IsActive), itemViews));
            }

            groupViews.Add(new MenuGroupView(group, moduleViews.Any(m => m.IsActive), moduleViews));
        }

        return new MenuViewModel(groupViews);
    }

    /// <summary>Splits a MenuURL such as "Screen/Index?Code=3" into path and query.</summary>
    private static (string Path, string Query) SplitUrl(string url)
    {
        var value = (url ?? string.Empty).Trim();
        if (value.Length == 0 || value.StartsWith('#'))
        {
            return (string.Empty, string.Empty);
        }

        var queryStart = value.IndexOf('?');
        var path = (queryStart < 0 ? value : value[..queryStart]).TrimStart('~', '/').TrimEnd('/');
        var query = queryStart < 0 ? string.Empty : value[queryStart..];
        return (path, query);
    }
}

public sealed class MenuGroupView(MenuGroup group, bool isActive, IReadOnlyList<MenuModuleView> modules)
{
    public MenuGroup Group { get; } = group;

    public bool IsActive { get; } = isActive;

    public IReadOnlyList<MenuModuleView> Modules { get; } = modules;
}

public sealed class MenuModuleView(MenuModule module, bool isActive, IReadOnlyList<MenuItemView> items)
{
    public MenuModule Module { get; } = module;

    public bool IsActive { get; } = isActive;

    public IReadOnlyList<MenuItemView> Items { get; } = items;
}

public sealed class MenuItemView(MenuItem item, bool isActive, string href)
{
    public MenuItem Item { get; } = item;

    public bool IsActive { get; } = isActive;

    public string Href { get; } = href;
}
