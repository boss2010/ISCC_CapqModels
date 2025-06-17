using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// ثابت - معالجة - نبات - نوباتجية - سحب
/// 
/// </summary>
public partial class FeesType
{
    public byte ID { get; set; }

    public string? Name_Ar { get; set; }

    public string? Name_En { get; set; }

    public bool IsActive { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public virtual ICollection<FeesAmount_Fixed> FeesAmount_Fixeds { get; set; } = new List<FeesAmount_Fixed>();

    public virtual ICollection<Fees_Action> Fees_Actions { get; set; } = new List<Fees_Action>();
}
