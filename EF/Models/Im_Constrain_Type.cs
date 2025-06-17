using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// انواع الاشتراطات
/// </summary>
public partial class Im_Constrain_Type
{
    public byte ID { get; set; }

    /// <summary>
    /// الاسم بالعربية
    /// </summary>
    public string? Ar_Name { get; set; }

    /// <summary>
    /// الاسم بالانجليزية
    /// </summary>
    public string? En_Name { get; set; }

    /// <summary>
    /// مفعل
    /// </summary>
    public bool IsActive { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public virtual ICollection<Im_CountryConstrain_Text> Im_CountryConstrain_Texts { get; set; } = new List<Im_CountryConstrain_Text>();

    public virtual ICollection<Im_choose_Constrain_> Im_choose_Constrain_s { get; set; } = new List<Im_choose_Constrain_>();
}
