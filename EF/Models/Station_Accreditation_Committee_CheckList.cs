using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// نتائج لجنة اعتماد المحطات
/// 
/// </summary>
public partial class Station_Accreditation_Committee_CheckList
{
    public long ID { get; set; }

    /// <summary>
    /// لجنة سحب العينة
    /// </summary>
    public long Committee_ID { get; set; }

    public long Station_Accreditation_CheckList_ID { get; set; }

    public long? EmployeeId { get; set; }

    /// <summary>
    /// 0 if rejected else 1 
    /// lab will set the result
    /// </summary>
    public bool? IsAccepted { get; set; }

    public string? Notes_Ar { get; set; }

    public string? Notes_En { get; set; }

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
    /// موقف الحجر
    /// 
    /// </summary>
    public bool? IsAccepted_Quarantine { get; set; }

    /// <summary>
    /// ملاحظات الحجر
    /// </summary>
    public string? Notes_Quarantine { get; set; }

    public virtual Station_Accreditation_Committee Committee { get; set; } = null!;

    public virtual Station_Accreditation_CheckList Station_Accreditation_CheckList { get; set; } = null!;

    public virtual ICollection<Station_Accreditation_Committee_CheckList_Confirm> Station_Accreditation_Committee_CheckList_Confirms { get; set; } = new List<Station_Accreditation_Committee_CheckList_Confirm>();
}
