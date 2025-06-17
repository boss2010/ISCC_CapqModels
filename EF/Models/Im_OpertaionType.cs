using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// أنواع عمليات الوارد
/// </summary>
public partial class Im_OpertaionType
{
    public byte ID { get; set; }

    public string? Name_Ar { get; set; }

    public string? Name_En { get; set; }

    /// <summary>
    /// 1 = permission request / 2 = check request / 3 = both permission and check request
    /// </summary>
    public byte? withPermission { get; set; }
}
