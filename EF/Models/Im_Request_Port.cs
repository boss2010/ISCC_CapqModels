using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// ميناء
/// </summary>
public partial class Im_Request_Port
{
    public int ID { get; set; }

    /// <summary>
    /// طلب الفحص
    /// </summary>
    public long? Im_RequestData_ID { get; set; }

    /// <summary>
    /// الميناء
    /// </summary>
    public int? Port_ID { get; set; }

    /// <summary>
    /// نوع الميناء التصدير
    /// transit/arrive/shipping
    /// </summary>
    public int ReqPortType_ID { get; set; }

    /// <summary>
    /// 21 National / 22 International
    /// </summary>
    public int IsNational { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public byte Port_Type_ID { get; set; }

    public virtual Im_RequestDatum? Im_RequestData { get; set; }

    public virtual A_SystemCode IsNationalNavigation { get; set; } = null!;

    public virtual A_SystemCode ReqPortType { get; set; } = null!;
}
