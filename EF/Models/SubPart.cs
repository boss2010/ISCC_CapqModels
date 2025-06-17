using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// الجزء النباتي والطور الحيوى
/// </summary>
public partial class SubPart
{
    public int ID { get; set; }

    public byte? Item_Type_ID { get; set; }

    public string? Name_Ar { get; set; }

    public string? Name_En { get; set; }

    /// <summary>
    /// وصف أو تنويه
    /// </summary>
    public string? Descreption_Ar { get; set; }

    /// <summary>
    /// وصف أو تنويه
    /// </summary>
    public string? Descreption_En { get; set; }

    /// <summary>
    /// مسموح/غير مسموح
    /// </summary>
    public bool IsActive { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public int? SubPart_Type_ID { get; set; }

    public virtual ICollection<ItemPart> ItemParts { get; set; } = new List<ItemPart>();

    public virtual ICollection<Item_ShortName> Item_ShortNames { get; set; } = new List<Item_ShortName>();

    public virtual SubPart_Type? SubPart_Type { get; set; }
}
