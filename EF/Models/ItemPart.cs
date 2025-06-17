using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// الجزء النباتى للكائنات
/// </summary>
public partial class ItemPart
{
    public long ID { get; set; }

    public long Item_ID { get; set; }

    public int SubPart_ID { get; set; }

    public bool IsAllowed { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public virtual Item Item { get; set; } = null!;

    public virtual SubPart SubPart { get; set; } = null!;
}
