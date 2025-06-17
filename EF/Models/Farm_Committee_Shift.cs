using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Farm_Committee_Shift
{
    public long ID { get; set; }

    public long Farm_Committee_ID { get; set; }

    public byte ShiftTiming_ID { get; set; }

    /// <summary>
    /// المبلغ
    /// </summary>
    public decimal? Amount { get; set; }

    public byte? Count { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public virtual Farm_Committee Farm_Committee { get; set; } = null!;

    public virtual ShiftTiming ShiftTiming { get; set; } = null!;
}
