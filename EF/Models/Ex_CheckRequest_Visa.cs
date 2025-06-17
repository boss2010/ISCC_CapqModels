using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Ex_CheckRequest_Visa
{
    public long ID { get; set; }

    public long? Ex_Visa_ID { get; set; }

    public long? Ex_CheckRequest_ID { get; set; }

    public DateOnly? Date { get; set; }

    public short? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public virtual Ex_CheckRequest? Ex_CheckRequest { get; set; }

    public virtual Ex_Visa? Ex_Visa { get; set; }

    public virtual Im_Visa? Ex_VisaNavigation { get; set; }
}
