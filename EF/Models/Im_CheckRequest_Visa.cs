using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// عدد التاشيرات علي طلب الفحص
/// </summary>
public partial class Im_CheckRequest_Visa
{
    public long ID { get; set; }

    /// <summary>
    /// جدول التاشيره
    /// </summary>
    public long? Im_Visa_ID { get; set; }

    /// <summary>
    /// جدول طلب الفحص الوارد
    /// </summary>
    public long? Im_CheckRequest_ID { get; set; }

    public DateOnly? Date { get; set; }

    public short? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public virtual Im_CheckRequest? Im_CheckRequest { get; set; }

    public virtual Im_Visa? Im_Visa { get; set; }
}
