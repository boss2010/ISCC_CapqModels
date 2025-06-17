using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class InternationalTransportation
{
    public long ID { get; set; }

    public string? Ar_Name { get; set; }

    public string? En_Name { get; set; }

    public string? TransferMethod { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public byte? Transport_Mean_Id { get; set; }

    public byte? Shipment_Mean_Id { get; set; }

    public virtual ICollection<Ex_CheckRequest_Datum> Ex_CheckRequest_Data { get; set; } = new List<Ex_CheckRequest_Datum>();

    public virtual ICollection<Im_CheckRequest_Datum> Im_CheckRequest_Data { get; set; } = new List<Im_CheckRequest_Datum>();
}
