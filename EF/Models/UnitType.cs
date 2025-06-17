using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// تبع الوارد
/// </summary>
public partial class UnitType
{
    public byte ID { get; set; }

    public string? Name_Ar { get; set; }

    public string? Name_En { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public virtual ICollection<FumigationUnit> FumigationUnits { get; set; } = new List<FumigationUnit>();
}
