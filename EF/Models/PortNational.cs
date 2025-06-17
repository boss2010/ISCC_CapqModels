using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// ميناء محلى
/// </summary>
public partial class PortNational
{
    /// <summary>
    /// الميناء
    /// </summary>
    public int ID { get; set; }

    /// <summary>
    /// الاسم بالعربية
    /// </summary>
    public string? Name_Ar { get; set; }

    /// <summary>
    /// الاسم بالانجليزية
    /// </summary>
    public string? Name_En { get; set; }

    /// <summary>
    /// المحافظة
    /// </summary>
    public short Govern_ID { get; set; }

    /// <summary>
    /// هيئات المواني
    /// </summary>
    public int PortOrgainzation_ID { get; set; }

    /// <summary>
    /// نوع الميناء
    /// </summary>
    public byte PortTypeID { get; set; }

    public string? Phone { get; set; }

    public string? Fax { get; set; }

    public string? Email { get; set; }

    public bool IsActive { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public virtual ICollection<Ex_CheckRequest_Place> Ex_CheckRequest_Places { get; set; } = new List<Ex_CheckRequest_Place>();

    public virtual Governate Govern { get; set; } = null!;

    public virtual ICollection<Im_CountryConstrain_ArrivalPort> Im_CountryConstrain_ArrivalPorts { get; set; } = new List<Im_CountryConstrain_ArrivalPort>();

    public virtual ICollection<Im_ScientificResearch> Im_ScientificResearches { get; set; } = new List<Im_ScientificResearch>();

    public virtual ICollection<Outlet> Outlets { get; set; } = new List<Outlet>();

    public virtual PortOrganization PortOrgainzation { get; set; } = null!;

    public virtual Port_Type PortType { get; set; } = null!;
}
