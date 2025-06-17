using System;
using System.Collections.Generic;

namespace EF.Models;

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

    public virtual ICollection<Center> Centers { get; set; } = new List<Center>();

    public virtual ICollection<Ex_CheckRequest> Ex_CheckRequests { get; set; } = new List<Ex_CheckRequest>();

    public virtual ICollection<FumigationUnit> FumigationUnits { get; set; } = new List<FumigationUnit>();

    public virtual General_Admin? GrAdmin { get; set; }

    public virtual ICollection<Im_CheckRequest> Im_CheckRequests { get; set; } = new List<Im_CheckRequest>();

    public virtual A_SystemCode IsExportNavigation { get; set; } = null!;

    public virtual ICollection<Outlet_Employee> Outlet_Employees { get; set; } = new List<Outlet_Employee>();

    public virtual PortNational? PortNational { get; set; }

    public virtual ICollection<pos_information> pos_informations { get; set; } = new List<pos_information>();
}
