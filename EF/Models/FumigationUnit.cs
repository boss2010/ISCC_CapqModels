using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// وحدات التبخير التابعة للحجر
/// </summary>
public partial class FumigationUnit
{
    public long ID { get; set; }

    public byte? UnitType_ID { get; set; }

    public long? Outlet_ID { get; set; }

    /// <summary>
    /// السعة
    /// </summary>
    public decimal? Capacity { get; set; }

    /// <summary>
    /// 0 if work else 1
    /// </summary>
    public bool Status { get; set; }

    /// <summary>
    /// حركة الصيانة مرتبط مع الشئون المالية
    /// </summary>
    public int? Maintenance { get; set; }

    public virtual Outlet? Outlet { get; set; }

    public virtual UnitType? UnitType { get; set; }
}
