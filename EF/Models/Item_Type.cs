using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// نوع الصنف نبات, منتج, بند حي...
/// </summary>
public partial class Item_Type
{
    public byte Id { get; set; }

    public string Name_Ar { get; set; } = null!;

    public string Name_En { get; set; } = null!;

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public string? Coler { get; set; }

    public virtual ICollection<MainCalssification> MainCalssifications { get; set; } = new List<MainCalssification>();
}
