using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Ex_Request_TreatmentDatum
{
    public long ID { get; set; }

    public long Ex_RequestCommittee_ID { get; set; }

    public long Ex_Request_Item_Id { get; set; }

    public long? Ex_Request_LotData_ID { get; set; }

    public byte? TreatmentType_ID { get; set; }

    public long? Company_ID { get; set; }

    public long? Station_ID { get; set; }

    public string? Station_Place { get; set; }

    public byte TreatmentMethod_ID { get; set; }

    public byte? TreatmentMat_ID { get; set; }

    public decimal? Size { get; set; }

    public decimal? TreatmentMat_Amount { get; set; }

    public decimal? TheDose { get; set; }

    public int? Exposure_Minute { get; set; }

    public int? Exposure_Hour { get; set; }

    public int? Exposure_Day { get; set; }

    public decimal? Temperature { get; set; }

    public string? Note { get; set; }

    public decimal? ThermalSealNumber { get; set; }

    public long? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public long? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public long User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public long? Item_ShortName_ID { get; set; }

    public bool? IS_Total_Android { get; set; }

    public bool? IS_From_Android { get; set; }

    public bool? IS_Total { get; set; }

    public string? Procedures { get; set; }

    /// <summary>
    /// المبلغ
    /// </summary>
    public decimal? Amount { get; set; }

    public decimal? Fees_Actual { get; set; }

    public bool? IsPaid { get; set; }

    public virtual Ex_RequestCommittee Ex_RequestCommittee { get; set; } = null!;

    public virtual ICollection<Ex_Request_TreatmentData_Confirm> Ex_Request_TreatmentData_Confirms { get; set; } = new List<Ex_Request_TreatmentData_Confirm>();

    public virtual TreatmentMaterial? TreatmentMat { get; set; }
}
