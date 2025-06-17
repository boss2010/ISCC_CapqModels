using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// الجهة المقدمة للطلب
/// </summary>
public partial class Im_ScientificResearch_Organization
{
    public long ID { get; set; }

    public string? Name_Ar { get; set; }

    public string? Name_En { get; set; }

    public string? Email { get; set; }

    public string? Address_En { get; set; }

    public string? Address_AR { get; set; }

    public string? Phone_No { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public virtual ICollection<Im_ScientificResearch> Im_ScientificResearches { get; set; } = new List<Im_ScientificResearch>();
}
