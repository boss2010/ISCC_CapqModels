using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// شركة محلية
/// </summary>
public partial class Company_National
{
    public long ID { get; set; }

    /// <summary>
    /// الاسم بالعربية
    /// </summary>
    public string? Name_Ar { get; set; }

    /// <summary>
    /// الاسم بالانجليزية
    /// </summary>
    public string? Name_En { get; set; }

    /// <summary>
    /// العنوان بالعربية
    /// </summary>
    public string? Address_Ar { get; set; }

    /// <summary>
    /// العنوان بالانجليزية
    /// </summary>
    public string? Address_En { get; set; }

    /// <summary>
    /// السجل الضريبي
    /// </summary>
    public string? TaxesRecord { get; set; }

    /// <summary>
    /// السجل التجاري
    /// </summary>
    public string? CommertialRecord { get; set; }

    /// <summary>
    /// it was deleted and i returned it again fz 8-9-2019
    /// </summary>
    public bool IsTreatment { get; set; }

    public bool? IsActive { get; set; }

    /// <summary>
    /// from web/system
    /// 1-&gt;online
    /// 0-&gt;offline
    /// </summary>
    public bool? IS_OnlineOffline { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short? User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    /// <summary>
    /// الاسم بالعربية
    /// </summary>
    public string Owner_Ar { get; set; } = null!;

    /// <summary>
    /// null-&gt;Company request 
    /// 0-&gt;not accepted
    /// 1-&gt;Aceecpted
    ///  
    /// </summary>
    public bool? IsApproved { get; set; }

    /// <summary>
    /// الاسم بالانجليزية
    /// </summary>
    public string? Owner_En { get; set; }

    public short? Center_ID { get; set; }

    public short? Village_ID { get; set; }

    public short? User_Activation_Id { get; set; }

    public DateTime? User_Activation_Date { get; set; }

    public virtual Center? Center { get; set; }

    public virtual ICollection<CompanyAccreditation> CompanyAccreditations { get; set; } = new List<CompanyAccreditation>();

    public virtual ICollection<CompanyActivity> CompanyActivities { get; set; } = new List<CompanyActivity>();

    public virtual ICollection<Gas_ImportCompany> Gas_ImportCompanies { get; set; } = new List<Gas_ImportCompany>();

    public virtual ICollection<ItemCategory> ItemCategories { get; set; } = new List<ItemCategory>();

    public virtual ICollection<SteamingCompany> SteamingCompanies { get; set; } = new List<SteamingCompany>();

    public virtual Village? Village { get; set; }
}
