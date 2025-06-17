using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Im_Constrain_Initiator_Text
{
    public long ID { get; set; }

    public long? ConstrainText_ID { get; set; }

    public long? Im_Initiator_ID { get; set; }

    public bool IsActive { get; set; }

    public bool? IsAcceppted { get; set; }

    public bool IsActive1 { get; set; }

    public long User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public long? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public long? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public byte? Group_ID { get; set; }

    public virtual Im_CountryConstrain_Text? ConstrainText { get; set; }

    public virtual Im_Initiator? Im_Initiator { get; set; }

    public virtual ICollection<Im_choose_Constrain_> Im_choose_Constrain_s { get; set; } = new List<Im_choose_Constrain_>();
}
