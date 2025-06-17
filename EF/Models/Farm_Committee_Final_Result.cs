using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Farm_Committee_Final_Result
{
    public long ID { get; set; }

    /// <summary>
    /// لجنة المعالجة
    /// </summary>
    public long FarmCommittee_ID { get; set; }

    public long EmployeeId { get; set; }

    /// <summary>
    /// is admin for the current committee
    /// </summary>
    public bool? ISAdmin { get; set; }

    public string? Notes_CheckList { get; set; }

    public string? Notes_Examination { get; set; }

    public string? Notes_SampleData { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    /// <summary>
    /// Employee ID
    /// </summary>
    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public virtual Farm_Committee FarmCommittee { get; set; } = null!;
}
