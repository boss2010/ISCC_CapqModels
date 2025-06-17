using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Station_Accreditation_Committee_Final_Result
{
    public long ID { get; set; }

    /// <summary>
    /// لجنة المعالجة
    /// </summary>
    public long Station_Accreditation_Committee_ID { get; set; }

    public long EmployeeId { get; set; }

    /// <summary>
    /// is admin for the current committee
    /// </summary>
    public bool ISAdmin { get; set; }

    public string? Notes_CheckList { get; set; }

    public string? Notes_final { get; set; }

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
    /// is admin for the current committee
    /// </summary>
    public bool? IsAccepted { get; set; }

    public virtual Station_Accreditation_Committee Station_Accreditation_Committee { get; set; } = null!;
}
