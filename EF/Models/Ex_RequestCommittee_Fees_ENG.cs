using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Ex_RequestCommittee_Fees_ENG
{
    public long ID { get; set; }

    public long Ex_RequestCommittee_ID { get; set; }

    public int Ex_Fees_Type_ID { get; set; }

    public decimal? Value { get; set; }

    public bool? IsActive { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    /// <summary>
    /// تم الانتهاء من الدفع
    /// </summary>
    public bool? IsPaid { get; set; }

    public int? Num_Eng { get; set; }

    public virtual EX_Fees_Type Ex_Fees_Type { get; set; } = null!;

    public virtual Ex_RequestCommittee Ex_RequestCommittee { get; set; } = null!;
}
