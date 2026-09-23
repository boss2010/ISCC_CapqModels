using System;
using System.Collections.Generic;

namespace EF.Imports.Entities;

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

    public string? HSCode { get; set; }

    public virtual ICollection<Im_CommitteeResult> Im_CommitteeResult { get; set; } = new List<Im_CommitteeResult>();

    public virtual Item? Item { get; set; }
}
