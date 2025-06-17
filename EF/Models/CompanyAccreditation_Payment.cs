using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class CompanyAccreditation_Payment
{
    public long ID { get; set; }

    public long Company_Committee_ID { get; set; }

    /// <summary>
    /// المبلغ
    /// </summary>
    public decimal Amount { get; set; }

    public DateTime User_Creation_Date { get; set; }

    /// <summary>
    /// from web/system
    /// 1-&gt;online
    /// 0-&gt;offline
    /// </summary>
    public bool IS_OnlineOffline { get; set; }

    public short User_Creation_Id { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public virtual CompanyAccreditation_Committee Company_Committee { get; set; } = null!;
}
