using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Farm_Request_Refuse_Reason
{
    public long ID { get; set; }

    public long Farm_Request_ID { get; set; }

    public short? Refuse_Reason_ID { get; set; }

    public string? Nots { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public virtual Farm_Request Farm_Request { get; set; } = null!;

    public virtual Refuse_Reason? Refuse_Reason { get; set; }
}
