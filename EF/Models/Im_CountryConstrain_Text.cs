using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Im_CountryConstrain_Text
{
    public long ID { get; set; }

    public string? ConstrainText_Ar { get; set; }

    public string? ConstrainText_En { get; set; }

    public string? InSide_Certificate_Ar { get; set; }

    public string? InSide_Certificate_En { get; set; }

    public bool? IsAcceppted { get; set; }

    public bool IsActive { get; set; }

    public long User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public long? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public long? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public byte? Im_Constrain_Type_ID { get; set; }

    public virtual ICollection<Im_Constrain_Initiator_Text> Im_Constrain_Initiator_Texts { get; set; } = new List<Im_Constrain_Initiator_Text>();

    public virtual Im_Constrain_Type? Im_Constrain_Type { get; set; }
}
