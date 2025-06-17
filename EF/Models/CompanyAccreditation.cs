using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// اعتمادات الشركة
/// </summary>
public partial class CompanyAccreditation
{
    public long ID { get; set; }

    /// <summary>
    /// الشركة
    /// </summary>
    public long? Company_ID { get; set; }

    /// <summary>
    /// الدولة
    /// </summary>
    public short? Country_ID { get; set; }

    /// <summary>
    /// تاريخ بداية الاعتماد
    /// </summary>
    public DateOnly? StartDate { get; set; }

    /// <summary>
    /// تاريخ نهاية الاعتماد
    /// </summary>
    public DateOnly? EndDate { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    /// <summary>
    /// null-&gt;ask for accredation
    /// 0-&gt;not accepted
    /// 1-&gt;Accepted
    /// </summary>
    public bool? IsApproved { get; set; }

    public short? User_Updation_Id { get; set; }

    /// <summary>
    /// product or plant ID manual no relation
    /// </summary>
    public long? Item_ShortName_id { get; set; }

    public virtual Company_National? Company { get; set; }

    public virtual ICollection<CompanyAccreditation_Committee> CompanyAccreditation_Committees { get; set; } = new List<CompanyAccreditation_Committee>();

    public virtual Country? Country { get; set; }

    public virtual Item_ShortName? Item_ShortName { get; set; }
}
