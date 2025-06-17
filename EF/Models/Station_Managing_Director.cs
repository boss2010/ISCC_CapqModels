using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// المدير المسئول
/// </summary>
public partial class Station_Managing_Director
{
    /// <summary>
    /// المدير المسئول
    /// </summary>
    public long ID { get; set; }

    public long StationID { get; set; }

    /// <summary>
    /// الاسم بالعربية
    /// </summary>
    public string? Ar_Name { get; set; }

    /// <summary>
    /// الاسم بالانجليزية
    /// </summary>
    public string? En_Name { get; set; }

    public string? Managing_Director_NID { get; set; }

    /// <summary>
    /// العنوان بالعربية
    /// </summary>
    public string? Address_Ar { get; set; }

    /// <summary>
    /// العنوان بالانجليزية
    /// </summary>
    public string? Address_En { get; set; }

    public string? Mobile { get; set; }

    public virtual Station Station { get; set; } = null!;
}
