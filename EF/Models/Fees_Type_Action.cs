using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// انواع الاجراءات
/// </summary>
public partial class Fees_Type_Action
{
    public byte ID { get; set; }

    public byte? Fees_process_ID { get; set; }

    public string? Name_Ar { get; set; }

    public string? Name_En { get; set; }

    public string? TableName { get; set; }

    public bool IsActive { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public virtual ICollection<Fees_Action> Fees_Actions { get; set; } = new List<Fees_Action>();

    public virtual Fees_process? Fees_process { get; set; }
}
