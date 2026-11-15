namespace ViewModels;

public sealed class UserPrivilegePageVm
{
    public List<UserPrivilegeOptionVm> Outlets { get; set; } = new();
    public List<UserPrivilegeOptionVm> Groups { get; set; } = new();
}

public sealed class UserPrivilegeOptionVm
{
    public long Id { get; set; }
    public string Text { get; set; } = string.Empty;
}

public sealed class UserPrivilegeEmployeeVm
{
    public short Id { get; set; }
    public long EmployeeId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string LoginName { get; set; } = string.Empty;
}

public sealed class UserPrivilegeMenuVm
{
    public int MenuId { get; set; }
    public string MenuTitle { get; set; } = string.Empty;
    public string MenuUrl { get; set; } = string.Empty;
    public bool CanView { get; set; }
    public bool CanAdd { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
}

public sealed class UserPrivilegeSaveVm
{
    public short UserId { get; set; }
    public int GroupId { get; set; }
    public int ModuleId { get; set; }
    public List<UserPrivilegeSaveRowVm> Menus { get; set; } = new();
}

public sealed class UserPrivilegeSaveRowVm
{
    public int MenuId { get; set; }
    public bool CanView { get; set; }
    public bool CanAdd { get; set; }
    public bool CanEdit { get; set; }
    public bool CanDelete { get; set; }
}
