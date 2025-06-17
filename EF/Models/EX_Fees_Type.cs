using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class EX_Fees_Type
{
    public int ID { get; set; }

    public string? Name { get; set; }

    public decimal? Value { get; set; }

    /// <summary>
    /// نوع الحساب من system code رقم 33
    /// </summary>
    public int? Account_Type { get; set; }

    /// <summary>
    /// صادر وارد ورية مهندس
    /// رقم 20
    /// فى system code
    /// 
    /// </summary>
    public int? Fees_Type { get; set; }

    /// <summary>
    /// نوع الوردية وقيمتها
    /// </summary>
    public long Fees_Action_ID { get; set; }

    public virtual ICollection<Ex_RequestCommittee_Fees_ENG> Ex_RequestCommittee_Fees_ENGs { get; set; } = new List<Ex_RequestCommittee_Fees_ENG>();
}
