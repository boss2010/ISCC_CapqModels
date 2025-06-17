using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Farm_Request_ItemCategory
{
    public long ID { get; set; }

    public long? Farm_Request_ID { get; set; }

    public long? Farm_ItemCategories_ID { get; set; }

    public bool? IsActive { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public DateTime? Admin_Date { get; set; }

    /// <summary>
    /// ادمن الحجر
    /// </summary>
    public short? Admin_User { get; set; }

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

    /// <summary>
    /// مساحة العميل
    /// </summary>
    public double? Area_Acres { get; set; }

    /// <summary>
    /// الكمية للطن
    /// </summary>
    public double? Quantity_Ton { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public virtual ICollection<Farm_Committee_Examination> Farm_Committee_Examinations { get; set; } = new List<Farm_Committee_Examination>();

    public virtual Farm_ItemCategory? Farm_ItemCategories { get; set; }

    public virtual Farm_Request? Farm_Request { get; set; }

    public virtual ICollection<Farm_SampleDatum> Farm_SampleData { get; set; } = new List<Farm_SampleDatum>();

    public virtual ICollection<Farm_SampleData_Item> Farm_SampleData_Items { get; set; } = new List<Farm_SampleData_Item>();
}
