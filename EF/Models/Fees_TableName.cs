using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Fees_TableName
{
    public short Id { get; set; }

    public string? TableName { get; set; }

    public string? Description { get; set; }

    public virtual ICollection<Fees_Transaction> Fees_Transactions { get; set; } = new List<Fees_Transaction>();
}
