using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Company_National1
{
    public long ID { get; set; }

    public string? Name_Ar { get; set; }

    public string? Name_En { get; set; }

    public string? Address_Ar { get; set; }

    public string? Address_En { get; set; }

    public string? TaxesRecord { get; set; }

    public string? CommertialRecord { get; set; }

    public bool IsTreatment { get; set; }

    public bool? IsActive { get; set; }

    public bool? IS_OnlineOffline { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short? User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public string Owner_Ar { get; set; } = null!;

    public bool? IsApproved { get; set; }

    public string? Owner_En { get; set; }

    public short? Center_ID { get; set; }

    public short? Village_ID { get; set; }

    public short? User_Activation_Id { get; set; }

    public DateTime? User_Activation_Date { get; set; }

    public virtual ICollection<CompanyActivity1> CompanyActivity1s { get; set; } = new List<CompanyActivity1>();
}
