using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// اشترطات المزارع
/// </summary>
public partial class Farm_Constrain
{
    public long ID { get; set; }

    public long? Item_ID { get; set; }

    public short? Country_Id { get; set; }

    public long? Farm_Constrain_Text_ID { get; set; }

    /// <summary>
    /// المعاينة
    /// </summary>
    public bool? Is_Preview { get; set; }

    public int? AnalysisType_ID { get; set; }

    public bool? IsActive { get; set; }

    public short? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public byte? Count_Visit { get; set; }

    public virtual AnalysisType? AnalysisType { get; set; }

    public virtual Country? Country { get; set; }

    public virtual ICollection<Farm_Committee_Constrain> Farm_Committee_Constrains { get; set; } = new List<Farm_Committee_Constrain>();

    public virtual Farm_Constrain_Text? Farm_Constrain_Text { get; set; }

    public virtual Item? Item { get; set; }
}
