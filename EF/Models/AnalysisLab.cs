using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// معامل التحاليل
/// </summary>
public partial class AnalysisLab
{
    public int ID { get; set; }

    public string? Name_Ar { get; set; }

    public string? Name_En { get; set; }

    public string? Addrees_Ar { get; set; }

    public string? Addrees_En { get; set; }

    public decimal? Phone { get; set; }

    public decimal? Fax { get; set; }

    public string? Email { get; set; }

    public bool IsActive { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public virtual ICollection<AnalysisLabType> AnalysisLabTypes { get; set; } = new List<AnalysisLabType>();
}
