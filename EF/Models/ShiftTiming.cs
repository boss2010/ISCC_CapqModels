using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class ShiftTiming
{
    public byte ID { get; set; }

    public string? Name_Ar { get; set; }

    public string? Name_En { get; set; }

    public TimeOnly? ShiftTiming_From { get; set; }

    public TimeOnly? ShiftTiming_To { get; set; }

    public bool IsActive { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    /// <summary>
    /// 0 ايام عطلات
    /// 1 ايام عادية
    /// 
    /// </summary>
    public byte? Day_Type { get; set; }

    public double? count { get; set; }

    public virtual ICollection<Ex_RequestCommittee_Shift> Ex_RequestCommittee_Shifts { get; set; } = new List<Ex_RequestCommittee_Shift>();

    public virtual ICollection<Farm_Committee_Shift> Farm_Committee_Shifts { get; set; } = new List<Farm_Committee_Shift>();

    public virtual ICollection<Im_RequestCommittee_Shift> Im_RequestCommittee_Shifts { get; set; } = new List<Im_RequestCommittee_Shift>();

    public virtual ICollection<Station_Accreditation_Committee_Shift> Station_Accreditation_Committee_Shifts { get; set; } = new List<Station_Accreditation_Committee_Shift>();
}
