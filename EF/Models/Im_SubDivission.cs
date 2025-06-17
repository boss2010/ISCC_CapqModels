using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// تقسيم الرسالة
/// </summary>
public partial class Im_SubDivission
{
    public long ID { get; set; }

    public long Im_Item_ID { get; set; }

    public decimal? Quantity { get; set; }

    /// <summary>
    /// رقم بوليصة الشحن
    /// </summary>
    public string? ShipmentPolicy_Number { get; set; }

    public string? Customs_Certificate { get; set; }

    public string FilePath { get; set; } = null!;

    public virtual Im_PermissionItem Im_Item { get; set; } = null!;
}
