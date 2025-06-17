using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// المزرعة معتمدة
/// </summary>
public partial class FarmsDatum
{
    public long ID { get; set; }

    public long? Item_ID { get; set; }

    public string? FarmCode_14 { get; set; }

    /// <summary>
    /// المراكز
    /// </summary>
    public short? Village_ID { get; set; }

    public string? Name_Ar { get; set; }

    public string? Name_En { get; set; }

    public string? Address_Ar { get; set; }

    public string? Address_En { get; set; }

    /// <summary>
    /// الحوض أو البيفوت
    /// </summary>
    public string? ThePivot { get; set; }

    /// <summary>
    /// قراءة GPS
    /// </summary>
    public string? GPSRead { get; set; }

    /// <summary>
    /// لو معتمدة 1
    /// </summary>
    public bool? IsApproved { get; set; }

    public bool? IsActive { get; set; }

    /// <summary>
    /// is null for default 0 is stopped for a time 1 is stopped permantely
    /// </summary>
    public bool? Status { get; set; }

    /// <summary>
    /// from web/system
    /// 1-&gt;online
    /// 0-&gt;offline
    /// </summary>
    public bool? IS_OnlineOffline { get; set; }

    public string? FileUpload { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short? Center_Id { get; set; }

    /// <summary>
    /// المحافظة
    /// </summary>
    public short? Govern_ID { get; set; }

    public virtual Center? Center { get; set; }

    public virtual ICollection<Ex_CheckRequest_Item> Ex_CheckRequest_Items { get; set; } = new List<Ex_CheckRequest_Item>();

    public virtual ICollection<Farm_Company> Farm_Companies { get; set; } = new List<Farm_Company>();

    public virtual ICollection<Farm_ItemCategory> Farm_ItemCategories { get; set; } = new List<Farm_ItemCategory>();

    public virtual ICollection<Farm_Request> Farm_Requests { get; set; } = new List<Farm_Request>();

    public virtual Governate? Govern { get; set; }

    public virtual Item? Item { get; set; }

    public virtual Village? Village { get; set; }
}
