using System;
using System.Collections.Generic;

namespace EF.Imports.Entities;

/// <summary>
/// المنفذ
/// </summary>
public partial class Outlet
{
    public long ID { get; set; }

    /// <summary>
    /// الادارة العامة
    /// </summary>
    public byte? GrAdmin_ID { get; set; }

    /// <summary>
    /// الاسم بالعربية
    /// </summary>
    public string? Ar_Name { get; set; }

    /// <summary>
    /// الاسم بالانجليزية
    /// </summary>
    public string? En_Name { get; set; }

    /// <summary>
    /// العنوان بالعربية
    /// </summary>
    public string? Address_Ar { get; set; }

    /// <summary>
    /// العنوان بالانجليزية
    /// </summary>
    public string? Address_En { get; set; }

    /// <summary>
    /// مشرف الفرع
    /// From HR employee table
    /// </summary>
    public int? Supervisor_ID { get; set; }

    public bool IsActive { get; set; }

    /// <summary>
    /// from system code 21
    /// صادر	/وارد	/صادر+ وارد
    /// </summary>
    public int IsExport { get; set; }

    public long? User_Updation_Id { get; set; }

    public DateOnly? User_Updation_Date { get; set; }

    public long? User_Deletion_Id { get; set; }

    public DateOnly? User_Deletion_Date { get; set; }

    public long User_Creation_Id { get; set; }

    public DateOnly User_Creation_Date { get; set; }

    /// <summary>
    /// رقم المنفذ بالنسبة لل HR
    /// </summary>
    public long? ID_HR { get; set; }

    public int? PortNational_ID { get; set; }

    /// <summary>
    /// from system code 21
    /// صادر	/وارد	/صادر+ وارد
    /// </summary>
    public byte IsDisplay { get; set; }

    public bool CanAcceptPayment { get; set; }

    public virtual ICollection<Im_CheckRequest> Im_CheckRequest { get; set; } = new List<Im_CheckRequest>();
}
