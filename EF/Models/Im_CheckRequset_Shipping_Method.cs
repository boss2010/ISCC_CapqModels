using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// اساليب الشحن لطلب الفحص الوارد
/// </summary>
public partial class Im_CheckRequset_Shipping_Method
{
    public long ID { get; set; }

    public long? Im_CheckRequest_ID { get; set; }

    /// <summary>
    /// حاوية او صب
    /// </summary>
    public int? containers_ID { get; set; }

    /// <summary>
    /// عبوات او بدون
    /// </summary>
    public int? containers_type_ID { get; set; }

    /// <summary>
    /// رقم عنبر السفينة
    /// </summary>
    public string? ShipholdNumber { get; set; }

    /// <summary>
    /// رقم الحاوية
    /// </summary>
    public string? ContainerNumber { get; set; }

    /// <summary>
    /// رقم السيل الملاحي
    /// </summary>
    public string? NavigationalNumber { get; set; }

    public decimal? Total_Weight { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public virtual Im_CheckRequest? Im_CheckRequest { get; set; }

    public virtual ICollection<Im_CheckRequest_Item> Im_CheckRequest_Items { get; set; } = new List<Im_CheckRequest_Item>();
}
