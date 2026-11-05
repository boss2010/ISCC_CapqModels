using System;
using System.Collections.Generic;

namespace PlantMobile.Models;

public sealed class CheckRequestOutletRowVm
{
    public long ID { get; set; }
    public string ImCheckRequest_Number { get; set; } = string.Empty;
    public int? Port_ID { get; set; }
    public string Port_Name { get; set; } = string.Empty;
    public int Gov_ID { get; set; }
    public string Gov_Name { get; set; } = string.Empty;
}

public sealed class UpdateOutletItemVm
{
    public long ID { get; set; }
    public long Outlet_ID { get; set; }
}

public sealed class UpdateOutletRequestVm
{
    public List<UpdateOutletItemVm> model { get; set; } = new();
}
