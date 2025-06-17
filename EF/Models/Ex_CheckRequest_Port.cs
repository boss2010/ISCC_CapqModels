using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Ex_CheckRequest_Port
{
    public int ID { get; set; }

    public long? Ex_CheckRequest_Data_ID { get; set; }

    public int Port_ID { get; set; }

    public int ReqPortType_ID { get; set; }

    public int IsNational { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public byte Port_Type_ID { get; set; }

    public virtual Ex_CheckRequest_Datum? Ex_CheckRequest_Data { get; set; }
}
