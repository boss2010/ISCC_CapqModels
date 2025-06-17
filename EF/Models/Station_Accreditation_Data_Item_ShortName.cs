using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// المسمى المختصر للاعتماد
/// </summary>
public partial class Station_Accreditation_Data_Item_ShortName
{
    public long ID { get; set; }

    public long Station_Accreditation_Data_ID { get; set; }

    public long Item_ShortName_ID { get; set; }

    public bool IsActive { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public virtual Item_ShortName Item_ShortName { get; set; } = null!;

    public virtual Station_Accreditation_Datum Station_Accreditation_Data { get; set; } = null!;
}
