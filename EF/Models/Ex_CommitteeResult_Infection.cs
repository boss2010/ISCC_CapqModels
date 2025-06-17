using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Ex_CommitteeResult_Infection
{
    public long ID { get; set; }

    public long Ex_CommitteeResult_ID { get; set; }

    public long Item_ID { get; set; }

    public short? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public virtual Ex_CommitteeResult Ex_CommitteeResult { get; set; } = null!;

    public virtual Item Item { get; set; } = null!;
}
