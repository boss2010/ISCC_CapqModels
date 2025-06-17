using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Farm_CheckList
{
    public long ID { get; set; }

    public string? ConstrainText_Ar { get; set; }

    public string? ConstrainText_En { get; set; }

    public string? Description_Ar { get; set; }

    public string? Description_En { get; set; }

    public bool IsActive { get; set; }

    public long User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public long? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public long? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public virtual ICollection<Farm_Country_CheckList> Farm_Country_CheckLists { get; set; } = new List<Farm_Country_CheckList>();
}
