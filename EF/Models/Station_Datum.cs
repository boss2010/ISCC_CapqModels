using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Station_Datum
{
    public long Station_ID { get; set; }

    public string? Ar_Name { get; set; }

    public string? StationCode { get; set; }

    public long? Company_ID { get; set; }

    public short? Gov_Id { get; set; }

    public short? Center_Id { get; set; }

    public short? Village_Id { get; set; }

    public string? Gov_Ar_Name { get; set; }

    public string? Center_Ar_Name { get; set; }

    public string? Village_Ar_Name { get; set; }

    public bool? station_IsActive { get; set; }

    public bool? station_IsAccepted { get; set; }

    public short? Station_User_Deletion_Id { get; set; }

    public string? Company_Name { get; set; }
}
