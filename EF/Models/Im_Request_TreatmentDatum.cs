using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Im_Request_TreatmentDatum
{
    public long ID { get; set; }

    /// <summary>
    /// لجنة المعالجة
    /// </summary>
    public long Im_RequestCommittee_ID { get; set; }

    public long Im_Request_Item_Id { get; set; }

    public long? Im_Request_LotData_ID { get; set; }

    public byte? TreatmentType_ID { get; set; }

    /// <summary>
    /// شركة المعالجة
    /// </summary>
    public long? Company_ID { get; set; }

    /// <summary>
    /// مكان المعالجة(محطة معتمدة)
    /// </summary>
    public long? Station_ID { get; set; }

    public string? Station_Place { get; set; }

    /// <summary>
    /// طريقة المعالجة
    /// </summary>
    public byte TreatmentMethod_ID { get; set; }

    /// <summary>
    /// مادة المعالجة
    /// </summary>
    public byte? TreatmentMat_ID { get; set; }

    /// <summary>
    /// حجم الرسالة (متر مكعب / سم مكعب)
    /// </summary>
    public decimal? Size { get; set; }

    /// <summary>
    /// كمية المادة المستخدمة في المعالجة
    /// </summary>
    public decimal? TreatmentMat_Amount { get; set; }

    /// <summary>
    /// الجرعة
    /// </summary>
    public decimal? TheDose { get; set; }

    public int? Exposure_Minute { get; set; }

    public int? Exposure_Hour { get; set; }

    public int? Exposure_Day { get; set; }

    /// <summary>
    /// درجة الحرارة
    /// </summary>
    public decimal? Temperature { get; set; }

    public string? Note { get; set; }

    /// <summary>
    /// رقم الختم الحراري
    /// </summary>
    public decimal? ThermalSealNumber { get; set; }

    public long? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public long? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public long User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    /// <summary>
    /// الاسم المختصر 
    /// </summary>
    public long? Item_ShortName_ID { get; set; }

    /// <summary>
    /// في حاله الفحص لو كلي واتحول الي جزئي
    /// </summary>
    public bool? IS_Total_Android { get; set; }

    /// <summary>
    /// مين رمي row (system or android)
    /// </summary>
    public bool? IS_From_Android { get; set; }

    /// <summary>
    /// (0) in the case of all,(1) in the case of the part في حاله الجزئي او الكلي
    /// </summary>
    public bool? IS_Total { get; set; }

    public string? Procedures { get; set; }

    /// <summary>
    /// المبلغ
    /// </summary>
    public decimal? Amount { get; set; }

    public decimal? Fees_Actual { get; set; }

    public bool? IsPaid { get; set; }

    public virtual Im_RequestCommittee Im_RequestCommittee { get; set; } = null!;

    public virtual ICollection<Im_Request_TreatmentData_Confirm> Im_Request_TreatmentData_Confirms { get; set; } = new List<Im_Request_TreatmentData_Confirm>();

    public virtual TreatmentMaterial? TreatmentMat { get; set; }
}
