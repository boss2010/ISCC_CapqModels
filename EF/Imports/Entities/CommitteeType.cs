using System;
using System.Collections.Generic;

namespace EF.Imports.Entities;

/// <summary>
/// الغرض من اللجنة
/// </summary>
public partial class CommitteeType
{
    public byte ID { get; set; }

    public string? Name_Ar { get; set; }

    public string? Name_En { get; set; }

    public int? type_Id { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public virtual ICollection<Im_RequestCommittee> Im_RequestCommittee { get; set; } = new List<Im_RequestCommittee>();
}
