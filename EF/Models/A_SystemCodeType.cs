using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class A_SystemCodeType
{
    public int Id { get; set; }

    public string? TableName { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<A_SystemCode> A_SystemCodes { get; set; } = new List<A_SystemCode>();
}
