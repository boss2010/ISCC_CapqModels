using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class CompanyActivity1
{
    public long ID { get; set; }

    public long? Company_ID { get; set; }

    public byte? CompActivityType_ID { get; set; }

    public int MainActivityType { get; set; }

    public string? Enrollment_Name { get; set; }

    public decimal? Enrollment_Number { get; set; }

    public DateOnly? Enrollment_Start { get; set; }

    public DateOnly? Enrollment_End { get; set; }

    public bool IsActive { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public byte? Enrollment_type_ID { get; set; }

    public virtual Company_National1? Company { get; set; }
}
