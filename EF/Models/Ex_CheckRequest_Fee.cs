using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Ex_CheckRequest_Fee
{
    public long ID { get; set; }

    public long Ex_CheckRequest_ID { get; set; }

    /// <summary>
    /// الوزن الاجمالى للطلب
    /// </summary>
    public decimal? Total_GrossWeight { get; set; }

    /// <summary>
    /// قيمة الرسوم
    /// </summary>
    public decimal? Fee_Value { get; set; }

    /// <summary>
    /// اجمالى الرسوم
    /// </summary>
    public decimal? Total_Amount { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public short? User_Creation_Id { get; set; }

    /// <summary>
    /// تقريب الرسوم
    /// </summary>
    public bool? is_Fee_Rounding { get; set; }

    public virtual Ex_CheckRequest Ex_CheckRequest { get; set; } = null!;
}
