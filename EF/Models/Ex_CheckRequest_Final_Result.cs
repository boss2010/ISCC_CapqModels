using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Ex_CheckRequest_Final_Result
{
    public long ID { get; set; }

    public long? Ex_CheckRequest_ID { get; set; }

    public int? Ex_Final_Result_ID { get; set; }

    public DateOnly? Date { get; set; }

    public short? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public virtual Ex_CheckRequest? Ex_CheckRequest { get; set; }

    public virtual Ex_Final_Result? Ex_Final_Result { get; set; }
}
