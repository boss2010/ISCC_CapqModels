using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Ex_List2
{
    public long? Outlet_User_ID { get; set; }

    public string? Outlet_User_Name { get; set; }

    public short? Center_ID { get; set; }

    public long Ex_CheckRequest_ID { get; set; }

    public string ImCheckRequest_Number { get; set; } = null!;

    public DateTime? Creation_Date { get; set; }

    public bool? IsAccepted { get; set; }

    public bool? IsActive { get; set; }

    public short ExportCountryID { get; set; }

    public string? ExportCountryName { get; set; }

    public long Importer_ID { get; set; }

    public int ImporterType_Id { get; set; }

    public bool? IsPaid { get; set; }

    public long? Outlet_ID { get; set; }

    public string? ImporterTypeName { get; set; }

    public string? ImporterName { get; set; }

    public short? f { get; set; }

    public long g { get; set; }

    public long? Outlet_Examination_ID { get; set; }

    public string? Outlet_Examination_Name { get; set; }

    public long? Station_Examination_ID { get; set; }

    public long? Outlet_Genshi_ID { get; set; }

    public string? Outlet_Genshi_Name { get; set; }

    public long? Station_Genshi_ID { get; set; }

    public int? Closed_Request { get; set; }

    public int Final_Result_ID { get; set; }

    public string Final_Result_Name { get; set; } = null!;

    public string? Station_Examination_Name { get; set; }

    public string? Station_Genshi_Name { get; set; }
}
