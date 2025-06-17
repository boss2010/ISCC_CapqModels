using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Farm_Request
{
    public long ID { get; set; }

    public long? FarmsData_ID { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsAcceppted { get; set; }

    /// <summary>
    /// null لم يتم انتهاء العمل على الطلب
    /// 0 تم رفض الطلب
    /// 1 تم قبول الطلب
    /// 
    /// </summary>
    public bool? IsStatus { get; set; }

    public DateOnly? Start_Date { get; set; }

    /// <summary>
    /// تم الانتهاء من الدفع
    /// </summary>
    public bool IsPaid { get; set; }

    public DateOnly? End_Date { get; set; }

    /// <summary>
    /// from web/system
    /// 1-&gt;online
    /// 0-&gt;offline
    /// </summary>
    public bool? IS_OnlineOffline { get; set; }

    public byte Farm_Request_Type_ID { get; set; }

    public decimal Fees { get; set; }

    public decimal Fees_Actual { get; set; }

    public DateOnly? End_Date_Request { get; set; }

    public DateOnly? Start_Date_Request { get; set; }

    /// <summary>
    /// الموقف النهائي للطلب
    /// null لم يتم العمل على الطلب
    /// 0 يتم العمل على الطلب
    /// 1 تم الانتهاء من العمل على الطلب
    /// </summary>
    public bool? Is_Final_requst { get; set; }

    public string? Print_Text { get; set; }

    public virtual ICollection<Farm_Committee> Farm_Committees { get; set; } = new List<Farm_Committee>();

    public virtual ICollection<Farm_Country> Farm_Countries { get; set; } = new List<Farm_Country>();

    public virtual ICollection<Farm_Request_ItemCategory> Farm_Request_ItemCategories { get; set; } = new List<Farm_Request_ItemCategory>();

    public virtual ICollection<Farm_Request_Refuse_Reason> Farm_Request_Refuse_Reasons { get; set; } = new List<Farm_Request_Refuse_Reason>();

    public virtual Farm_Request_Type Farm_Request_Type { get; set; } = null!;

    public virtual FarmsDatum? FarmsData { get; set; }
}
