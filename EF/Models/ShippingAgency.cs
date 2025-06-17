using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class ShippingAgency
{
    public long ID { get; set; }

    /// <summary>
    /// الاسم بالعربية
    /// </summary>
    public string? Name_Ar { get; set; }

    /// <summary>
    /// الاسم بالانجليزية
    /// </summary>
    public string? Name_En { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public bool IsActive { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public virtual ICollection<Ex_CheckRequest_Customs_Message> Ex_CheckRequest_Customs_Messages { get; set; } = new List<Ex_CheckRequest_Customs_Message>();

    public virtual ICollection<Im_CheckRequest_Customs_Message> Im_CheckRequest_Customs_Messages { get; set; } = new List<Im_CheckRequest_Customs_Message>();
}
