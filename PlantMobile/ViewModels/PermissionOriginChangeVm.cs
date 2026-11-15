namespace ViewModels;

public sealed class PermissionOriginChangePageVm
{
    public long PermissionRequestId { get; set; }
    public string PermissionNumber { get; set; } = string.Empty;
    public List<PermissionOriginChangeItemVm> Items { get; set; } = new();
    public string ReturnUrl { get; set; } = string.Empty;
}

public sealed class PermissionOriginChangeItemVm
{
    public long PermissionItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string ScientificName { get; set; } = string.Empty;
    public string CurrentCountry { get; set; } = string.Empty;
    public long? SelectedInitiatorId { get; set; }
    public List<PermissionOriginOptionVm> Origins { get; set; } = new();
}

public sealed class PermissionOriginOptionVm
{
    public long InitiatorId { get; set; }
    public short CountryId { get; set; }
    public string CountryName { get; set; } = string.Empty;
}

public sealed class PermissionOriginChangeSaveVm
{
    public long PermissionRequestId { get; set; }
    public string PermissionNumber { get; set; } = string.Empty;
    public string ReturnUrl { get; set; } = string.Empty;
    public List<PermissionOriginChangeSaveItemVm> Items { get; set; } = new();
}

public sealed class PermissionOriginChangeSaveItemVm
{
    public long PermissionItemId { get; set; }
    public long InitiatorId { get; set; }
}
