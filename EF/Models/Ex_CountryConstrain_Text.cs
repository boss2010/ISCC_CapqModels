using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// نصوص الاشتراطات
/// </summary>
public partial class Ex_CountryConstrain_Text
{
    public long ID { get; set; }

    public long CountryConstrain_ID { get; set; }

    public long? EX_Constrain_Text_ID { get; set; }

    public bool? IsAcceppted { get; set; }

    public bool IsActive { get; set; }

    public long User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public long? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public long? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public long? Parent_ID { get; set; }

    public virtual Ex_CountryConstrain CountryConstrain { get; set; } = null!;

    public virtual EX_Constrain_Text? EX_Constrain_Text { get; set; }
}
