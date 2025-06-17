using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// التحاليل التي تتم على العينة
/// </summary>
public partial class AnalysisType
{
    public int ID { get; set; }

    public string? Name_Ar { get; set; }

    public string? Name_En { get; set; }

    public bool IsActive { get; set; }

    /// <summary>
    /// هل يرفض الطلب كله في حالة وجود إصابة
    /// </summary>
    public bool IsRejectedAll { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public virtual ICollection<AnalysisLabType> AnalysisLabTypes { get; set; } = new List<AnalysisLabType>();

    public virtual ICollection<Ex_CountryConstrain_AnalysisLabType> Ex_CountryConstrain_AnalysisLabTypes { get; set; } = new List<Ex_CountryConstrain_AnalysisLabType>();

    public virtual ICollection<Farm_Constrain> Farm_Constrains { get; set; } = new List<Farm_Constrain>();
}
