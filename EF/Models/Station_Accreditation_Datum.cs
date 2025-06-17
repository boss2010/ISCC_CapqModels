using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// مسمى الاعتماد
/// </summary>
public partial class Station_Accreditation_Datum
{
    public long ID { get; set; }

    /// <summary>
    /// نوع النشاط
    /// </summary>
    public byte? StationActivityType_ID { get; set; }

    /// <summary>
    /// A_SystemCode id =21
    /// </summary>
    public int? Accreditation_Type_ID { get; set; }

    /// <summary>
    /// الاسم بالعربية
    /// </summary>
    public string? Name_AR { get; set; }

    /// <summary>
    /// الاسم بالانجليزية
    /// </summary>
    public string? Name_En { get; set; }

    /// <summary>
    /// العنوان بالعربية
    /// </summary>
    public string? Description_Ar { get; set; }

    /// <summary>
    /// العنوان بالانجليزية
    /// </summary>
    public string? Description_En { get; set; }

    public string? DescriptionMore_AR { get; set; }

    public string? DescriptionMore_EN { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public virtual A_SystemCode? Accreditation_Type { get; set; }

    public virtual StationActivityType? StationActivityType { get; set; }

    public virtual ICollection<Station_Accreditation_CheckList> Station_Accreditation_CheckLists { get; set; } = new List<Station_Accreditation_CheckList>();

    public virtual ICollection<Station_Accreditation_Data_Country> Station_Accreditation_Data_Countries { get; set; } = new List<Station_Accreditation_Data_Country>();

    public virtual ICollection<Station_Accreditation_Data_Item_ShortName> Station_Accreditation_Data_Item_ShortNames { get; set; } = new List<Station_Accreditation_Data_Item_ShortName>();

    public virtual ICollection<Station_Accreditation_Request> Station_Accreditation_Requests { get; set; } = new List<Station_Accreditation_Request>();

    public virtual ICollection<Station_Accreditation> Station_Accreditations { get; set; } = new List<Station_Accreditation>();
}
