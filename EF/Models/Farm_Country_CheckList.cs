using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Farm_Country_CheckList
{
    public long ID { get; set; }

    public long Farm_CheckList_ID { get; set; }

    public long? Item_ID { get; set; }

    public short? Country_ID { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public bool IsActive { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public virtual Country? Country { get; set; }

    public virtual Farm_CheckList Farm_CheckList { get; set; } = null!;

    public virtual ICollection<Farm_Committee_CheckList> Farm_Committee_CheckLists { get; set; } = new List<Farm_Committee_CheckList>();

    public virtual Item? Item { get; set; }
}
