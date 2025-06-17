using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// انواع عمليات الرسوم - صادر وارد مزارع محطات
/// </summary>
public partial class Fees_process
{
    public byte ID { get; set; }

    public string? Name_En { get; set; }

    public string? Name_Ar { get; set; }

    public virtual ICollection<Fees_Type_Action> Fees_Type_Actions { get; set; } = new List<Fees_Type_Action>();
}
