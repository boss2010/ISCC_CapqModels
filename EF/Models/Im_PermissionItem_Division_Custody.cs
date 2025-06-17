using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// تقسيم/ نقل الى مكان التحفظ
/// </summary>
public partial class Im_PermissionItem_Division_Custody
{
    public long ID { get; set; }

    /// <summary>
    /// كود اماكن التحفظ
    /// </summary>
    public long Im_CustodyPlace_Id { get; set; }

    public long Im_CheckRequest_Item_Id { get; set; }

    /// <summary>
    /// الوزن الاجمالي
    /// </summary>
    public decimal GrossWeight { get; set; }

    /// <summary>
    /// كود وسائل النقل
    /// </summary>
    public byte Transport_Mean_Id { get; set; }

    public string Transport_Mean_Number { get; set; } = null!;

    /// <summary>
    /// اسم السائق
    /// </summary>
    public string Driver_Name { get; set; } = null!;

    /// <summary>
    /// رقم تليفون السائق
    /// </summary>
    public string Driver_Phone { get; set; } = null!;

    public string Driver_National_Id { get; set; } = null!;

    /// <summary>
    /// هل تم الموافقة على التقسيم
    /// حالة الطلب
    /// </summary>
    public bool? IsAccepted { get; set; }

    /// <summary>
    /// تاريخ القبول
    /// </summary>
    public DateOnly? Accept_Date { get; set; }

    public DateTime? Accept_User_Updation_Date { get; set; }

    /// <summary>
    /// الموظف الذى وافق على الصنف وهذا ليس له علاقة بمن ادخل الصنف( يقرا من الاذن نفسه)
    /// </summary>
    public short? Accept_User_Creation_Id { get; set; }

    public DateTime? Accept_User_Creation_Date { get; set; }

    public short? Accept_User_Updation_Id { get; set; }

    public virtual Im_CheckRequest_Item Im_CheckRequest_Item { get; set; } = null!;

    public virtual Im_CustodyPlace_CheckRequest Im_CustodyPlace { get; set; } = null!;

    public virtual ICollection<Im_PermissionItem_Division_Custody_DismissCommittee> Im_PermissionItem_Division_Custody_DismissCommittees { get; set; } = new List<Im_PermissionItem_Division_Custody_DismissCommittee>();

    public virtual Transport_Mean Transport_Mean { get; set; } = null!;
}
