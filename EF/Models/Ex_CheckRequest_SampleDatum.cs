using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Ex_CheckRequest_SampleDatum
{
    public long ID { get; set; }

    public int AnalysisLabType_ID { get; set; }

    public long Ex_RequestCommittee_ID { get; set; }

    public long Ex_Request_Item_Id { get; set; }

    public long? LotData_ID { get; set; }

    public DateOnly? WithdrawDate { get; set; }

    public string? Sample_BarCode { get; set; }

    public double? SampleSize { get; set; }

    public double? SampleRatio { get; set; }

    public bool? IsAccepted { get; set; }

    public string? Notes_Ar { get; set; }

    public string? RejectReason_Ar { get; set; }

    public string? RejectReason_En { get; set; }

    public string? Notes_En { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public bool? Admin_Confirmation { get; set; }

    public short? Admin_User { get; set; }

    public DateTime? Admin_Date { get; set; }

    public bool? IsPrint { get; set; }

    public bool? IS_Total { get; set; }

    public long? Item_ShortName_ID { get; set; }

    public bool? IS_Total_Android { get; set; }

    public bool? IS_From_Android { get; set; }

    public string? Syl_ALkhatima_Number { get; set; }

    public decimal? Amount { get; set; }

    public decimal? Fees_Actual { get; set; }

    public bool? IsPaid { get; set; }

    public int? Count_Sample { get; set; }

    public virtual AnalysisLabType AnalysisLabType { get; set; } = null!;

    public virtual ICollection<Ex_CheckRequest_SampleData_Confirm> Ex_CheckRequest_SampleData_Confirms { get; set; } = new List<Ex_CheckRequest_SampleData_Confirm>();

    public virtual Ex_RequestCommittee Ex_RequestCommittee { get; set; } = null!;
}
