using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class EX_Constrain_Text
{
    public long ID { get; set; }

    public byte EX_Constrain_Country_Item_ID { get; set; }

    public string? ConstrainText_Ar { get; set; }

    public string? ConstrainText_En { get; set; }

    public string? InSide_Certificate_Ar { get; set; }

    public string? InSide_Certificate_En { get; set; }

    /// <summary>
    /// شهادة الصحة النباتية
    /// </summary>
    public bool IsCertificate_Addtion { get; set; }

    public bool? IsAcceppted { get; set; }

    public bool IsActive { get; set; }

    public long User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public long? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public long? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public virtual EX_Constrain_Country_Item EX_Constrain_Country_Item { get; set; } = null!;

    public virtual ICollection<Ex_CountryConstrain_Text> Ex_CountryConstrain_Texts { get; set; } = new List<Ex_CountryConstrain_Text>();
}
