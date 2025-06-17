using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// تحاليل الاشتراطات
/// </summary>
public partial class Ex_CountryConstrain_AnalysisLabType
{
    public long ID { get; set; }

    public long CountryConstrain_ID { get; set; }

    /// <summary>
    /// 1 لو مفعل انه يظهر في الشهادة الزراعية
    /// </summary>
    public bool IsAcive { get; set; }

    public long User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public long? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public long? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public long? Parent_ID { get; set; }

    public int AnalysisTypeID { get; set; }

    public virtual AnalysisType AnalysisType { get; set; } = null!;

    public virtual Ex_CountryConstrain CountryConstrain { get; set; } = null!;

    public virtual ICollection<EX_Choose_SampleDatum> EX_Choose_SampleData { get; set; } = new List<EX_Choose_SampleDatum>();
}
