using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class SubPart_Type
{
    public int ID { get; set; }

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

    public virtual ICollection<SubPart> SubParts { get; set; } = new List<SubPart>();
}
