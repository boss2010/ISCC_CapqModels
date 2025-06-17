using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Im_CheckRequest_Distribution
{
    public long ID { get; set; }

    public long? Im_CheckRequest_ID { get; set; }

    public long? Item_ID { get; set; }

    public long? Item_ShortName_ID { get; set; }

    public long? Im_Initiator_ID { get; set; }

    public long? ItemCategory_ID { get; set; }

    public int ImporterType_Id { get; set; }

    public long Importer_ID { get; set; }

    public DateTime? Date_Distribution { get; set; }

    public byte? Num_Distribution { get; set; }

    public decimal? GrossWeight { get; set; }

    /// <summary>
    /// الوزن الصافي
    /// </summary>
    public decimal? Net_Weight { get; set; }

    public bool? IsActive { get; set; }

    public short? User_Updation_Id { get; set; }

    public short? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public virtual Im_CheckRequest? Im_CheckRequest { get; set; }
}
