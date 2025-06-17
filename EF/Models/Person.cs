using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Person
{
    public long ID { get; set; }

    public string? Name { get; set; }

    public int? Person_IDType { get; set; }

    /// <summary>
    /// رقم قومى/ باسبور
    /// </summary>
    public string? IDNumber { get; set; }

    public short? Country_ID { get; set; }

    public string? Job { get; set; }

    public string? Address { get; set; }

    public string? Phone { get; set; }

    public string? Email { get; set; }

    public short? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public bool? IsActive { get; set; }

    public short? User_Activation_Id { get; set; }

    public DateTime? User_Activation_Date { get; set; }

    public short? Village_ID { get; set; }

    public short? Center_ID { get; set; }

    public short? Govern_ID { get; set; }

    public string? Name_EN { get; set; }

    public string? Address_EN { get; set; }

    public virtual Country? Country { get; set; }

    public virtual A_SystemCode? Person_IDTypeNavigation { get; set; }
}
