using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Im_CheckRequest_Port
{
    public int ID { get; set; }

    public long? Im_CheckRequest_Data_ID { get; set; }

    /// <summary>
    /// رقم الميناء
    /// </summary>
    public int? Port_ID { get; set; }

    /// <summary>
    /// ميناء شحن وصور وعبور
    /// </summary>
    public int ReqPortType_ID { get; set; }

    /// <summary>
    /// دولية ولا لا
    /// </summary>
    public int IsNational { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    /// <summary>
    /// نوع المينا بحرى جوى مطار
    /// </summary>
    public byte Port_Type_ID { get; set; }
}
