using System;
using System.Collections.Generic;

namespace EF.Imports.Entities;

/// <summary>
/// جدول ربط التحاليل بالمعامل
/// </summary>
public partial class AnalysisLabType
{
    public int ID { get; set; }

    public int AnalysisLabID { get; set; }

    public int AnalysisTypeID { get; set; }

    public short? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public virtual AnalysisLab AnalysisLab { get; set; } = null!;

    public virtual AnalysisType AnalysisType { get; set; } = null!;

    public virtual ICollection<Im_CheckRequest_SampleData> Im_CheckRequest_SampleData { get; set; } = new List<Im_CheckRequest_SampleData>();
}
