using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Station_Emp
{
    public long Id { get; set; }

    public long? Station_Id { get; set; }

    public long? Emp_Id { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateOnly? Date_From { get; set; }

    public DateOnly? Date_To { get; set; }

    public virtual Station? Station { get; set; }
}
