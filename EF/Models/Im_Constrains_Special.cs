using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Im_Constrains_Special
{
    public long ID { get; set; }

    public long? Im_PermissionRequest_ID { get; set; }

    public string? ConstrainText_En { get; set; }

    public string? ConstrainText_Ar { get; set; }

    public bool IsActive { get; set; }

    public long User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public long? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public long? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public virtual ICollection<Im_choose_Constrain_> Im_choose_Constrain_s { get; set; } = new List<Im_choose_Constrain_>();
}
