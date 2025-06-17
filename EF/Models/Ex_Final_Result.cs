using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Ex_Final_Result
{
    public int ID { get; set; }

    public string? Ar_Name { get; set; }

    public string? En_Name { get; set; }

    public short? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public bool? IsActive { get; set; }

    public bool? Status { get; set; }

    public virtual ICollection<Ex_CheckRequest_Final_Result> Ex_CheckRequest_Final_Results { get; set; } = new List<Ex_CheckRequest_Final_Result>();
}
