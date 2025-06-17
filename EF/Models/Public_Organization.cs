using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// الهيئات العامة
/// </summary>
public partial class Public_Organization
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

    public int? PublicOrgType_ID { get; set; }

    public bool IsNational { get; set; }

    public bool? IsActive { get; set; }

    /// <summary>
    /// from web/system
    /// 1-&gt;online
    /// 0-&gt;offline
    /// </summary>
    public bool? IS_OnlineOffline { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short? User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public short? Center_ID { get; set; }

    public short? Village_ID { get; set; }

    public short? Person_Responsible_Country_ID { get; set; }

    public string? Person_Responsible_Name { get; set; }

    public string? Person_Responsible_Job { get; set; }

    public string? Person_Responsible_Address { get; set; }

    public int? Person_IDType { get; set; }

    public string? PersonResponsible_IDNumber { get; set; }

    public short? User_Activation_Id { get; set; }

    public DateTime? User_Activation_Date { get; set; }

    public virtual PublicOrganization_Type? PublicOrgType { get; set; }
}
