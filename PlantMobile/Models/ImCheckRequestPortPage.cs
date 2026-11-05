using System;
using System.Collections.Generic;

namespace PlantMobile.Models;

public sealed class ImCheckRequestPortRowVm
{
    public long CheckRequestId { get; set; }
    public string CheckRequestNumber { get; set; } = string.Empty;
    public int ReqPortTypeId { get; set; }
    public string PortTypeName { get; set; } = string.Empty;
    public long CheckRequestPortId { get; set; }
    public int? PortId { get; set; }
    public int? GovernId { get; set; }
    public string GovernName { get; set; } = string.Empty;
    public string PortNameAr { get; set; } = string.Empty;
    public string CountryArName { get; set; } = string.Empty;
    public bool CommitteeFormed { get; set; }
    public bool? CommitteeStatus { get; set; }
}

public sealed class PortOptionVm
{
    public string DisplayText { get; set; } = string.Empty;
    public long? Value { get; set; }
}

public sealed class UpdatePortItemVm
{
    public long Im_CheckRequest_Port_ID { get; set; }
    public long Port_ID { get; set; }
    public byte status { get; set; } = 1;
}

public sealed class UpdatePortsRequestVm
{
    public List<UpdatePortItemVm> list_of_values { get; set; } = new();
    public string CheckNumber { get; set; } = string.Empty;
}
