using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// اصناف المزرعة
/// </summary>
public partial class Farm_ItemCategory
{
    public long ID { get; set; }

    public long? Farm_ID { get; set; }

    public long? ItemCategories_ID { get; set; }

    /// <summary>
    /// مساحة العميل
    /// </summary>
    public double? Area_Acres { get; set; }

    public double? Quantity_Ton { get; set; }

    public DateOnly? Date { get; set; }

    public bool? IsActive { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    /// <summary>
    /// المساحة النهائية للحجر
    /// </summary>
    public double? Area_Acres_Quarant { get; set; }

    /// <summary>
    /// الكمية للفدان بالطن للحجر
    /// </summary>
    public double? Quantity_Ton__Quarant { get; set; }

    /// <summary>
    /// الكمية الاجمالية الصالحة للتصدير
    /// </summary>
    public double? Quantity_Ton__Export { get; set; }

    public bool? IsAcceppted { get; set; }

    public virtual FarmsDatum? Farm { get; set; }

    public virtual ICollection<Farm_Request_ItemCategory> Farm_Request_ItemCategories { get; set; } = new List<Farm_Request_ItemCategory>();

    public virtual ICollection<Farms_Organization_Distribution_Master> Farms_Organization_Distribution_Masters { get; set; } = new List<Farms_Organization_Distribution_Master>();

    public virtual ItemCategory? ItemCategories { get; set; }
}
