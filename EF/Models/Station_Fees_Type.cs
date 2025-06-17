using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Station_Fees_Type
{
    public int ID { get; set; }

    public string? Name { get; set; }

    public decimal? Value { get; set; }

    /// <summary>
    /// نوع الحساب من  system code رقم 33
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

    public virtual ICollection<Station_Accreditation_Request_Fee> Station_Accreditation_Request_Fees { get; set; } = new List<Station_Accreditation_Request_Fee>();

    public virtual ICollection<Station_Accreditation_Request_Fees_ENG> Station_Accreditation_Request_Fees_ENGs { get; set; } = new List<Station_Accreditation_Request_Fees_ENG>();
}
