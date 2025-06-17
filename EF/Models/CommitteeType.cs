using System;
using System.Collections.Generic;

namespace EF.Models;

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

    public virtual ICollection<CompanyAccreditation_Committee> CompanyAccreditation_Committees { get; set; } = new List<CompanyAccreditation_Committee>();

    public virtual ICollection<Ex_RequestCommittee> Ex_RequestCommittees { get; set; } = new List<Ex_RequestCommittee>();

    public virtual ICollection<Farm_Committee> Farm_Committees { get; set; } = new List<Farm_Committee>();

    public virtual ICollection<Im_RequestCommittee> Im_RequestCommittees { get; set; } = new List<Im_RequestCommittee>();

    public virtual ICollection<Station_Accreditation_Committee> Station_Accreditation_Committees { get; set; } = new List<Station_Accreditation_Committee>();

    public virtual ICollection<Station_Accreditation_Request> Station_Accreditation_Requests { get; set; } = new List<Station_Accreditation_Request>();

    public virtual A_SystemCode? type { get; set; }
}
