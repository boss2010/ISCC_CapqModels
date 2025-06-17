using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// معالجات الاشتراطات
/// </summary>
public partial class Ex_CountryConstrain_Treatment
{
    public long ID { get; set; }

    public long CountryConstrain_ID { get; set; }

    /// <summary>
    /// الجرعة
    /// </summary>
    public decimal? TheDose { get; set; }

    public int? Exposure_Day { get; set; }

    public int? Exposure_Minute { get; set; }

    public int? Exposure_Hour { get; set; }

    /// <summary>
    /// 1 لو مفعل انه يظهر في الشهادة الزراعية
    /// </summary>
    public bool IsAcive { get; set; }

    public long User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public long? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public long? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public long? Parent_ID { get; set; }

    /// <summary>
    /// التحليل اختيارى =0 
    /// التحليل اجباري =1
    /// </summary>
    public bool IS_Optional { get; set; }

    public byte TreatmentMethods_ID { get; set; }

    public virtual Ex_CountryConstrain CountryConstrain { get; set; } = null!;

    public virtual ICollection<EX_Choose_Treatment> EX_Choose_Treatments { get; set; } = new List<EX_Choose_Treatment>();

    public virtual TreatmentMethod TreatmentMethods { get; set; } = null!;
}
