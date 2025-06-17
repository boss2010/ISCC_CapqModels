using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// الموقف النهائي لطلب الفحص
/// </summary>
public partial class Im_Final_Result
{
    public int ID { get; set; }

    /// <summary>
    /// الاسم بالعربية
    /// </summary>
    public string? Ar_Name { get; set; }

    /// <summary>
    /// الاسم بالانجليزية
    /// </summary>
    public string? En_Name { get; set; }

    public short? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    /// <summary>
    /// الطلب فعال ام لا
    /// </summary>
    public bool? IsActive { get; set; }

    /// <summary>
    /// تم ايقاف الطلب ام لا
    /// 0 = مرفوض
    /// 1 = مقبول
    /// </summary>
    public bool? Status { get; set; }

    public virtual ICollection<Im_CheckRequest_Final_Result> Im_CheckRequest_Final_Results { get; set; } = new List<Im_CheckRequest_Final_Result>();
}
