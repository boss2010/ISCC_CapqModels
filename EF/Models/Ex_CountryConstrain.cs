using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// الاشترطات الدوليه
/// </summary>
public partial class Ex_CountryConstrain
{
    public long ID { get; set; }

    /// <summary>
    /// الدولة المستوردة
    /// </summary>
    public short? Import_Country_ID { get; set; }

    /// <summary>
    /// دولة عبور
    /// </summary>
    public short? TransportCountry_ID { get; set; }

    /// <summary>
    /// product or plant ID manual no relation
    /// </summary>
    public long Item_ShortName_id { get; set; }

    public long? ItemCategories_ID { get; set; }

    /// <summary>
    /// هل محطة معتمدة
    /// </summary>
    public bool? IsStationAccreditation { get; set; }

    /// <summary>
    /// هل مزرعة معتمدة
    /// </summary>
    public bool? IsFarmAccreditation { get; set; }

    /// <summary>
    /// هل شركة معتمدة
    /// </summary>
    public bool? IsCompanyAccreditation { get; set; }

    public bool IsActive { get; set; }

    public long User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public long? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public long? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public virtual ICollection<Ex_CountryConstrain_AnalysisLabType> Ex_CountryConstrain_AnalysisLabTypes { get; set; } = new List<Ex_CountryConstrain_AnalysisLabType>();

    public virtual ICollection<Ex_CountryConstrain_ArrivalPort> Ex_CountryConstrain_ArrivalPorts { get; set; } = new List<Ex_CountryConstrain_ArrivalPort>();

    public virtual ICollection<Ex_CountryConstrain_Text> Ex_CountryConstrain_Texts { get; set; } = new List<Ex_CountryConstrain_Text>();

    public virtual ICollection<Ex_CountryConstrain_Treatment> Ex_CountryConstrain_Treatments { get; set; } = new List<Ex_CountryConstrain_Treatment>();

    public virtual Item_ShortName Item_ShortName { get; set; } = null!;
}
