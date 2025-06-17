using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Im_CheckRequest_Manafest
{
    public long ID { get; set; }

    public long Im_CheckRequest_Id { get; set; }

    public long Im_Manafest { get; set; }

    public virtual Im_CheckRequest Im_CheckRequest { get; set; } = null!;

    public virtual Im_Manafest Im_ManafestNavigation { get; set; } = null!;
}
