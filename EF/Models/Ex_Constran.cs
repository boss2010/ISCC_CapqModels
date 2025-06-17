using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Ex_Constran
{
    public short? Import_Country_ID { get; set; }

    public short? TransportCountry_ID { get; set; }

    public long Item_ShortName_id { get; set; }

    public long? ItemCategories_ID { get; set; }

    public long ID { get; set; }

    public bool IsActive { get; set; }

    public bool? IsStationAccreditation { get; set; }

    public bool? IsFarmAccreditation { get; set; }

    public bool? IsCompanyAccreditation { get; set; }

    public string? ConstrainText_Ar { get; set; }

    public string? ConstrainText_En { get; set; }

    public string? InSide_Certificate_Ar { get; set; }

    public string? InSide_Certificate_En { get; set; }

    public bool IsCertificate_Addtion { get; set; }

    public string? Ar_Name { get; set; }

    public string? En_Name { get; set; }

    public string? Expr1 { get; set; }

    public string? Expr2 { get; set; }

    public string? ShortName_Ar { get; set; }

    public string? ShortName_En { get; set; }
}
