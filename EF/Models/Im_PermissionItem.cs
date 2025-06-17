using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// النباتات والمنتجات للوارد
/// </summary>
public partial class Im_PermissionItem
{
    public long ID { get; set; }

    public long? Im_PermissionRequest_ID { get; set; }

    public long? Im_CheckRequest_ID { get; set; }

    /// <summary>
    /// دوله المنشا
    /// </summary>
    public long? Im_Initiator_ID { get; set; }

    /// <summary>
    /// جزء نباتى
    /// </summary>
    public int? SubPart_id { get; set; }

    /// <summary>
    /// رقم اذن الاستيراد يأخذ رقم عند الموافقة
    /// </summary>
    public string? Item_Permission_Number { get; set; }

    /// <summary>
    /// مادة العبوة
    /// </summary>
    public short? Package_Material_ID { get; set; }

    /// <summary>
    /// نوع العبوة
    /// </summary>
    public short? Package_Type_ID { get; set; }

    /// <summary>
    /// عدد العبوات
    /// </summary>
    public int? Package_Count { get; set; }

    /// <summary>
    /// وزن العبوة
    /// </summary>
    public decimal? Package_Weight { get; set; }

    /// <summary>
    /// عدد الوحدات
    /// </summary>
    public int? Units_Number { get; set; }

    /// <summary>
    /// 1 if divided lots /0 if Sub
    /// </summary>
    public bool? Is_LotDivision { get; set; }

    /// <summary>
    /// حجم الرسالة
    /// </summary>
    public double? Size { get; set; }

    /// <summary>
    /// الرتبة
    /// </summary>
    public string? Order_Text { get; set; }

    /// <summary>
    /// الوزن الاجمالى
    /// </summary>
    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// هل تم الموافقة على الصنف
    /// </summary>
    public bool IsAccepted { get; set; }

    public DateOnly? Accept_Date { get; set; }

    public DateTime? Accept_User_Updation_Date { get; set; }

    /// <summary>
    /// الموظف الذى وافق على الصنف وهذا ليس له علاقة بمن ادخل الصنف( يقرا من الاذن نفسه)
    /// </summary>
    public short? Accept_User_Creation_Id { get; set; }

    public DateTime? Accept_User_Creation_Date { get; set; }

    public short? Accept_User_Updation_Id { get; set; }

    /// <summary>
    /// ConstrainOwner(UnionId/CountryId/ or 0 if Local-Egypt)
    /// </summary>
    public short? Country_ID { get; set; }

    public long? Item_ShortName_ID { get; set; }

    public short? QualitativeGroup_Id { get; set; }

    public virtual Im_Initiator? Im_Initiator { get; set; }

    public virtual ICollection<Im_ItemsLotDivision> Im_ItemsLotDivisions { get; set; } = new List<Im_ItemsLotDivision>();

    public virtual ICollection<Im_PermissionItems_Category> Im_PermissionItems_Categories { get; set; } = new List<Im_PermissionItems_Category>();

    public virtual ICollection<Im_SubDivission> Im_SubDivissions { get; set; } = new List<Im_SubDivission>();
}
