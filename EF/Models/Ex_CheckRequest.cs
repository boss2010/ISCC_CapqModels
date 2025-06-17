using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Ex_CheckRequest
{
    public long ID { get; set; }

    public long? Outlet_ID { get; set; }

    public string CheckRequest_Number { get; set; } = null!;

    public string? ExportCompany { get; set; }

    public string? ExportCompanyAddress { get; set; }

    public bool? IsPaid { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsAccepted { get; set; }

    /// <summary>
    /// from A_SystemCode =3
    /// </summary>
    public int? User_Type_ID { get; set; }

    public long? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public long? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public long? User_Updation_Id { get; set; }

    public short? IsStatus { get; set; }

    public DateTime? IsAccepted_Date { get; set; }

    public decimal? Amount { get; set; }

    public string? NationalIDCompanyOwner { get; set; }

    public byte? Ex_OpertaionType_ID { get; set; }

    /// <summary>
    /// اسباب الرفض
    /// </summary>
    public string? Notes_Reject { get; set; }

    public virtual ICollection<EX_Choose_SampleDatum> EX_Choose_SampleData { get; set; } = new List<EX_Choose_SampleDatum>();

    public virtual ICollection<EX_Choose_Treatment> EX_Choose_Treatments { get; set; } = new List<EX_Choose_Treatment>();

    public virtual ICollection<Ex_CertificatesNewCountry> Ex_CertificatesNewCountries { get; set; } = new List<Ex_CertificatesNewCountry>();

    public virtual ICollection<Ex_CertificatesRequest> Ex_CertificatesRequests { get; set; } = new List<Ex_CertificatesRequest>();

    public virtual ICollection<Ex_CheckRequest_Customs_Message> Ex_CheckRequest_Customs_Messages { get; set; } = new List<Ex_CheckRequest_Customs_Message>();

    public virtual ICollection<Ex_CheckRequest_Datum> Ex_CheckRequest_Data { get; set; } = new List<Ex_CheckRequest_Datum>();

    public virtual ICollection<Ex_CheckRequest_Fee> Ex_CheckRequest_Fees { get; set; } = new List<Ex_CheckRequest_Fee>();

    public virtual ICollection<Ex_CheckRequest_Final_Result> Ex_CheckRequest_Final_Results { get; set; } = new List<Ex_CheckRequest_Final_Result>();

    public virtual ICollection<Ex_CheckRequest_Item> Ex_CheckRequest_Items { get; set; } = new List<Ex_CheckRequest_Item>();

    public virtual ICollection<Ex_CheckRequest_Organization_Distribution_Detial> Ex_CheckRequest_Organization_Distribution_Detials { get; set; } = new List<Ex_CheckRequest_Organization_Distribution_Detial>();

    public virtual ICollection<Ex_CheckRequest_Organization_Distribution> Ex_CheckRequest_Organization_Distributions { get; set; } = new List<Ex_CheckRequest_Organization_Distribution>();

    public virtual ICollection<Ex_CheckRequest_Place> Ex_CheckRequest_Places { get; set; } = new List<Ex_CheckRequest_Place>();

    public virtual ICollection<Ex_CheckRequest_RefuseReason> Ex_CheckRequest_RefuseReasons { get; set; } = new List<Ex_CheckRequest_RefuseReason>();

    public virtual ICollection<Ex_CheckRequest_Visa> Ex_CheckRequest_Visas { get; set; } = new List<Ex_CheckRequest_Visa>();

    public virtual ICollection<Ex_RequestCommittee> Ex_RequestCommittees { get; set; } = new List<Ex_RequestCommittee>();

    public virtual Outlet? Outlet { get; set; }

    public virtual ICollection<Pallet_Data_Ex_CheckRequest_Distribution> Pallet_Data_Ex_CheckRequest_Distributions { get; set; } = new List<Pallet_Data_Ex_CheckRequest_Distribution>();

    public virtual ICollection<Pallet_Data_Organization__Distribution> Pallet_Data_Organization__Distributions { get; set; } = new List<Pallet_Data_Organization__Distribution>();
}
