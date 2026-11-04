namespace PlantMobile.Models.Menu;

/// <summary>A top-level menu group (for example: بيانات أساسية).</summary>
public sealed class MenuGroup
{
    public MenuGroup(int id, string title)
    {
        Id = id;
        Title = title;
    }

    public int Id { get; }

    public string Title { get; }

    public List<MenuModule> Modules { get; } = [];
}

/// <summary>A module inside a group; it holds the clickable screens.</summary>
public sealed class MenuModule
{
    public MenuModule(int id, string title)
    {
        Id = id;
        Title = title;
    }

    public int Id { get; }

    public string Title { get; }

    public List<MenuItem> Items { get; } = [];
}

/// <summary>One clickable screen in the navigation menu.</summary>
public sealed class MenuItem
{
    public required int Id { get; init; }

    public required string Title { get; init; }

    /// <summary>The raw MenuURL value, exactly as stored in PR_Menu.</summary>
    public string Url { get; init; } = string.Empty;

    public bool CanView { get; init; }

    public bool CanAdd { get; init; }

    public bool CanEdit { get; init; }

    public bool CanDelete { get; init; }

    /// <summary>True for heading rows only (no URL, or an anchor).</summary>
    public bool IsContainer => string.IsNullOrWhiteSpace(Url) || Url.StartsWith('#');
}
