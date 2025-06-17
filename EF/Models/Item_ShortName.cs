using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// المسمى المختصر
/// </summary>
public partial class Item_ShortName
{
    public long ID { get; set; }

    /// <summary>
    /// الصنف
    /// </summary>
    public long? Item_ID { get; set; }

    /// <summary>
    /// الجزء النباتى او الطور الحيوى
    /// </summary>
    public int? SubPart_ID { get; set; }

    /// <summary>
    /// الحالة
    /// </summary>
    public int? Item_Status_ID { get; set; }

    /// <summary>
    /// الغرض
    /// </summary>
    public int? Item_Purpose_ID { get; set; }

    /// <summary>
    /// الاسم العربى
    /// </summary>
    public string? ShortName_Ar { get; set; }

    /// <summary>
    /// الاسم الاجنبى
    /// </summary>
    public string? ShortName_En { get; set; }

    /// <summary>
    /// الموقف من التصدير
    /// </summary>
    public bool ExportStatus { get; set; }

    /// <summary>
    /// الموقف من الاستيراد
    /// </summary>
    public bool ImportStatus { get; set; }

    /// <summary>
    /// سبب الايقاف
    /// </summary>
    public string? Reason { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    /// <summary>
    /// له اسم مختصر ام لا
    /// </summary>
    public bool? IS_ShortName { get; set; }

    /// <summary>
    /// مجموعة نوعية
    /// </summary>
    public short? QualitativeGroup_Id { get; set; }

    public byte? Item_Type_ID { get; set; }

    /// <summary>
    /// معفي من اذن الاستيراد
    /// </summary>
    public bool? Is_ImportTaxFree { get; set; }

    /// <summary>
    /// المنتج
    /// </summary>
    public long? Product_ID { get; set; }

    public long? ItemCategories_Group_ID { get; set; }

    public virtual ICollection<CompanyAccreditation> CompanyAccreditations { get; set; } = new List<CompanyAccreditation>();

    public virtual ICollection<EX_Choose_SampleDatum> EX_Choose_SampleData { get; set; } = new List<EX_Choose_SampleDatum>();

    public virtual ICollection<Ex_CheckRequest_Item> Ex_CheckRequest_Items { get; set; } = new List<Ex_CheckRequest_Item>();

    public virtual ICollection<Ex_CheckRequest_Organization_Distribution_Master> Ex_CheckRequest_Organization_Distribution_Masters { get; set; } = new List<Ex_CheckRequest_Organization_Distribution_Master>();

    public virtual ICollection<Ex_CommitteeResult> Ex_CommitteeResults { get; set; } = new List<Ex_CommitteeResult>();

    public virtual ICollection<Ex_CountryConstrain> Ex_CountryConstrains { get; set; } = new List<Ex_CountryConstrain>();

    public virtual ICollection<Im_CommitteeResult> Im_CommitteeResults { get; set; } = new List<Im_CommitteeResult>();

    public virtual ICollection<Im_CountryConstrain_ArrivalPort> Im_CountryConstrain_ArrivalPorts { get; set; } = new List<Im_CountryConstrain_ArrivalPort>();

    public virtual ICollection<Im_Initiator> Im_Initiators { get; set; } = new List<Im_Initiator>();

    public virtual Item? Item { get; set; }

    public virtual Item_Purpose? Item_Purpose { get; set; }

    public virtual Item_Status? Item_Status { get; set; }

    public virtual QualitativeGroup? QualitativeGroup { get; set; }

    public virtual ICollection<Station_Accreditation_Data_Item_ShortName> Station_Accreditation_Data_Item_ShortNames { get; set; } = new List<Station_Accreditation_Data_Item_ShortName>();

    public virtual SubPart? SubPart { get; set; }
}
