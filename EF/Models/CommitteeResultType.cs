using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// مرفوض/ مقبول ..........
/// </summary>
public partial class CommitteeResultType
{
    public byte ID { get; set; }

    public string? Name_Ar { get; set; }

    public string? Name_En { get; set; }

    public bool IsActive { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public virtual ICollection<Ex_CommitteeResult> Ex_CommitteeResults { get; set; } = new List<Ex_CommitteeResult>();

    public virtual ICollection<Im_CommitteeResult> Im_CommitteeResults { get; set; } = new List<Im_CommitteeResult>();
}
