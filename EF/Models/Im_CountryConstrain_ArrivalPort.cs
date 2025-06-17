using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// موانى تحديد ميناء وصول معين
/// </summary>
public partial class Im_CountryConstrain_ArrivalPort
{
    public long Id { get; set; }

    public int? Port_National_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public long User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public long? User_Deletion_Id { get; set; }

    public bool IsActive { get; set; }

    public long? Item_ShortName_ID { get; set; }

    public byte Port_Type_ID { get; set; }

    public short? Id_QualitativeGroup { get; set; }

    public virtual QualitativeGroup? Id_QualitativeGroupNavigation { get; set; }

    public virtual Item_ShortName? Item_ShortName { get; set; }

    public virtual PortNational? Port_National { get; set; }
}
