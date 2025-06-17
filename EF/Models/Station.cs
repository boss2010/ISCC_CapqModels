using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// محطة
/// </summary>
public partial class Station
{
    public long ID { get; set; }

    /// <summary>
    /// الاسم بالعربية
    /// </summary>
    public string? Ar_Name { get; set; }

    /// <summary>
    /// الاسم بالانجليزية
    /// </summary>
    public string? En_Name { get; set; }

    /// <summary>
    /// العنوان بالعربية
    /// </summary>
    public string? Address_Ar { get; set; }

    /// <summary>
    /// العنوان بالانجليزية
    /// </summary>
    public string? Address_En { get; set; }

    public string? StationCode { get; set; }

    /// <summary>
    /// السجل الضريبي
    /// </summary>
    public string? TaxesRecord { get; set; }

    /// <summary>
    /// السجل التجاري
    /// </summary>
    public string? CommertialRecord { get; set; }

    /// <summary>
    /// رقم الترخيص الصناعي
    /// </summary>
    public string? Industrial_License_Num { get; set; }

    public string? FileUpload { get; set; }

    /// <summary>
    /// لو معتمدة 1
    /// </summary>
    public bool? IsApproved { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public short? Gov_Id { get; set; }

    public short? Center_Id { get; set; }

    public short? Village_Id { get; set; }

    /// <summary>
    /// from systemcode table 3
    /// </summary>
    public int? User_Type_Id { get; set; }

    public long? Company_ID { get; set; }

    /// <summary>
    /// تاريخ النهاية
    /// </summary>
    public DateOnly? EndDate_Industrial_License_Num { get; set; }

    /// <summary>
    /// تاريخ البداية
    /// </summary>
    public DateOnly? StartDate_Industrial_License_Num { get; set; }

    /// <summary>
    /// عدد ساعات العمل
    /// </summary>
    public int? working_hours { get; set; }

    /// <summary>
    /// متوسط عدد العمال
    /// </summary>
    public int? Average_number_workers { get; set; }

    /// <summary>
    /// عدد الورديات
    /// </summary>
    public int? Number_Shifts { get; set; }

    /// <summary>
    /// عدد ايام العمل
    /// </summary>
    public int? Number_working_Days { get; set; }

    /// <summary>
    /// موسمي 0 سنوي 1
    /// </summary>
    public byte? Seasonal_Annual { get; set; }

    public int? Year_Creation { get; set; }

    /// <summary>
    /// موافقة ورفض الطلب
    /// 
    /// </summary>
    public bool? IsAccepted { get; set; }

    public decimal? Facility_area { get; set; }

    public decimal? The_num_of_storage_refrigerators { get; set; }

    public decimal? Storage_fridge_capacity { get; set; }

    public decimal? Fast_cooling_refrigerators { get; set; }

    public decimal? The_num_of_production_lines { get; set; }

    public decimal? Production_capacity { get; set; }

    /// <summary>
    /// اسباب الرفض
    /// </summary>
    public string? Notes_Reject { get; set; }

    public virtual ICollection<Ex_CheckRequest_Place> Ex_CheckRequest_PlaceStation_Examinations { get; set; } = new List<Ex_CheckRequest_Place>();

    public virtual ICollection<Ex_CheckRequest_Place> Ex_CheckRequest_PlaceStation_Genshis { get; set; } = new List<Ex_CheckRequest_Place>();

    public virtual ICollection<Im_CustodyPlace_CheckRequest> Im_CustodyPlace_CheckRequests { get; set; } = new List<Im_CustodyPlace_CheckRequest>();

    public virtual ICollection<StationContact> StationContacts { get; set; } = new List<StationContact>();

    public virtual ICollection<Station_Accreditation_Request> Station_Accreditation_Requests { get; set; } = new List<Station_Accreditation_Request>();

    public virtual ICollection<Station_Accreditation> Station_Accreditations { get; set; } = new List<Station_Accreditation>();

    public virtual ICollection<Station_Emp> Station_Emps { get; set; } = new List<Station_Emp>();

    public virtual ICollection<Station_Managing_Director> Station_Managing_Directors { get; set; } = new List<Station_Managing_Director>();
}
