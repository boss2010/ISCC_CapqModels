using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class LiableItems_ShortName
{
    public long ID { get; set; }

    public int? LiableItem_ID { get; set; }

    public int? PlantPurpose_ID { get; set; }

    public int? LiableItems_Status_ID { get; set; }

    public int? Biological_Phase_ID { get; set; }

    public string? ScientficName_En { get; set; }

    public string? ScientficName_Ar { get; set; }

    public string? ShortName_Ar { get; set; }

    public string? ShortName_En { get; set; }

    public string? HSCODE { get; set; }

    /// <summary>
    /// الموقف من التصدير
    /// </summary>
    public bool ExportStatus { get; set; }

    /// <summary>
    /// الموقف من الاستيراد
    /// </summary>
    public bool ImportStatus { get; set; }

    public string? Reason { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public virtual Biological_Phase? Biological_Phase { get; set; }

    public virtual LiableItem? LiableItem { get; set; }

    public virtual LiableItems_Status? LiableItems_Status { get; set; }

    public virtual Item_Purpose? PlantPurpose { get; set; }
}
