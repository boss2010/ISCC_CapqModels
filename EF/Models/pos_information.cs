using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// ماكينات الدفع
/// </summary>
public partial class pos_information
{
    public long Id { get; set; }

    public string? pos_number { get; set; }

    public long? user_id { get; set; }

    public string? place { get; set; }

    public long? Outlet_ID { get; set; }

    public int? InstitutionalCode { get; set; }

    public string? BankAccount { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Fees_Transactions_Payment_Detile> Fees_Transactions_Payment_Detiles { get; set; } = new List<Fees_Transactions_Payment_Detile>();

    public virtual Outlet? Outlet { get; set; }
}
