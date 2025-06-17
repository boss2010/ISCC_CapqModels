using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Ex_CommitteeResult
{
    public long ID { get; set; }

    public long Committee_ID { get; set; }

    public long Ex_Request_Item_Id { get; set; }

    public long? LotData_ID { get; set; }

    public long? EmployeeId { get; set; }

    public byte? CommitteeResultType_ID { get; set; }

    public DateTime? Date { get; set; }

    public bool? IsAdminResult { get; set; }

    public string? AdminFinalResult_Note { get; set; }

    public double? QuantitySize { get; set; }

    public double? Weight { get; set; }

    public string? Notes { get; set; }

    public bool? IS_Total { get; set; }

    public long? Item_ShortName_ID { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public bool? IS_Total_Android { get; set; }

    public double? WeightOld { get; set; }

    public virtual ICollection<A_AttachmentData_Ex_CommitteeResult_Infection> A_AttachmentData_Ex_CommitteeResult_Infections { get; set; } = new List<A_AttachmentData_Ex_CommitteeResult_Infection>();

    public virtual Ex_RequestCommittee Committee { get; set; } = null!;

    public virtual CommitteeResultType? CommitteeResultType { get; set; }

    public virtual ICollection<Ex_CommitteeResult_Confirm> Ex_CommitteeResult_Confirms { get; set; } = new List<Ex_CommitteeResult_Confirm>();

    public virtual ICollection<Ex_CommitteeResult_Infection> Ex_CommitteeResult_Infections { get; set; } = new List<Ex_CommitteeResult_Infection>();

    public virtual Item_ShortName? Item_ShortName { get; set; }
}
