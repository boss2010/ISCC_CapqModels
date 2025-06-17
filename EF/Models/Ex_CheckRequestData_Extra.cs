using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Ex_CheckRequestData_Extra
{
    public long ID { get; set; }

    public long Ex_CheckRequest_Data_ID { get; set; }

    public string? ImportCompany { get; set; }

    public string? ImporeterCompanyAddress { get; set; }

    public string? Reciever_Name { get; set; }

    public string? OwnerName { get; set; }

    public string? OwnerAddress { get; set; }

    public string? ImportCompany_EN { get; set; }

    public string? ImporeterCompanyAddress_EN { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public bool? IsActive { get; set; }

    public virtual Ex_CheckRequest_Datum Ex_CheckRequest_Data { get; set; } = null!;
}
