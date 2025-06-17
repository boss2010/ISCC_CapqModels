using System;
using System.Collections.Generic;

namespace EF.Models;

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

    public virtual ICollection<EX_Choose_SampleDatum> EX_Choose_SampleData { get; set; } = new List<EX_Choose_SampleDatum>();

    public virtual ICollection<Ex_CheckRequest_SampleDatum> Ex_CheckRequest_SampleData { get; set; } = new List<Ex_CheckRequest_SampleDatum>();

    public virtual ICollection<Farm_SampleDatum> Farm_SampleData { get; set; } = new List<Farm_SampleDatum>();

    public virtual ICollection<Farm_SampleData_Item> Farm_SampleData_Items { get; set; } = new List<Farm_SampleData_Item>();

    public virtual ICollection<Im_CheckRequest_SampleDatum> Im_CheckRequest_SampleData { get; set; } = new List<Im_CheckRequest_SampleDatum>();
}
