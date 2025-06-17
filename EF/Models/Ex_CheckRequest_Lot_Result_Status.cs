using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Ex_CheckRequest_Lot_Result_Status
{
    public int ID { get; set; }

    public string? Name_AR { get; set; }

    public string? Name_En { get; set; }

    public bool? IsActive { get; set; }

    /// <summary>
    /// 0 عدم استكمال الاعمال
    /// لا يمكن استكمال الاعمال 1
    /// </summary>
    public bool? Is_Continue { get; set; }

    public byte? CommitteeType_ID { get; set; }

    public virtual ICollection<Ex_CheckRequest_Items_Lot_Result> Ex_CheckRequest_Items_Lot_Results { get; set; } = new List<Ex_CheckRequest_Items_Lot_Result>();
}
