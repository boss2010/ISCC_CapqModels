using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Farm_Committee_CheckList
{
    public long ID { get; set; }

    /// <summary>
    /// لجنة المعالجة
    /// </summary>
    public long FarmCommittee_ID { get; set; }

    public long Farm_Country_CheckList_ID { get; set; }

    public long? EmployeeId { get; set; }

    public string? Notes_Ar { get; set; }

    public string? Notes_En { get; set; }

    /// <summary>
    /// 0 if rejected else 1 
    /// lab will set the result
    /// </summary>
    public bool? IsAccepted { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    /// <summary>
    /// Employee ID
    /// </summary>
    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    /// <summary>
    /// 0 if rejected else 1 
    /// lab will set the result
    /// </summary>
    public bool? IsAccepted_Quarantine { get; set; }

    public long? EmployeeId_Quarantine { get; set; }

    public virtual Farm_Committee FarmCommittee { get; set; } = null!;

    public virtual ICollection<Farm_Committee_CheckList_Confirm> Farm_Committee_CheckList_Confirms { get; set; } = new List<Farm_Committee_CheckList_Confirm>();

    public virtual Farm_Country_CheckList Farm_Country_CheckList { get; set; } = null!;
}
