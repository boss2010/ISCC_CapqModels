using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// طلب تغير وجهه
/// </summary>
public partial class Ex_CertificatesNewCountry
{
    public int ID { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public long? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public int? NewCountryId { get; set; }

    public bool? ISAccepted { get; set; }

    public int? OldCountryID { get; set; }

    public long? Ex_CheckRequest_ID { get; set; }

    public int? Port_International_ID_new { get; set; }

    public int? Port_International_ID_old { get; set; }

    public int? ReqPortType_ID { get; set; }

    public int? Port_Type_ID_Old { get; set; }

    public int? Port_Type_ID_New { get; set; }

    public virtual Ex_CheckRequest? Ex_CheckRequest { get; set; }
}
