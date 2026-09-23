using System;
using System.Collections.Generic;

namespace EF.Imports.Entities;

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

    public virtual ICollection<Im_CommitteeResult> Im_CommitteeResult { get; set; } = new List<Im_CommitteeResult>();
}
