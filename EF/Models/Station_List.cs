using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Station_List
{
    public string station_Status { get; set; } = null!;

    public int station_btn { get; set; }

    public long Station_ID { get; set; }

    public string? Ar_Name { get; set; }

    public string? StationCode { get; set; }

    public long? Company_ID { get; set; }

    public string? Company_Name { get; set; }

    public short? Gov_Id { get; set; }

    public short? Center_Id { get; set; }

    public short? Village_Id { get; set; }

    public string? Gov_Ar_Name { get; set; }

    public string? Center_Ar_Name { get; set; }

    public string? Village_Ar_Name { get; set; }

    public bool? station_IsActive { get; set; }

    public bool? station_IsAccepted { get; set; }

    public short? Station_User_Deletion_Id { get; set; }

    public long Station_Accreditation_Data_ID { get; set; }

    public byte Station_Accreditation_Request_Type_ID { get; set; }

    public bool? Station_Accreditation_Request_ISActive { get; set; }

    public bool? Station_Accreditation_Request_ISpaid { get; set; }

    public bool? Station_Accreditation_Request_IsAccepted { get; set; }

    public bool? Is_Final_requst { get; set; }

    public string? Station_Accreditation_Data_Name { get; set; }

    public bool? Station_Accreditation_Data_IsActive { get; set; }

    public string? Station_Accreditation_Request_Type_Name { get; set; }

    public bool? Station_Accreditation_Request_Type_IsActive { get; set; }

    public long Station_Accreditation_Request_ID { get; set; }

    public int? Accreditation_Type_ID { get; set; }

    public byte? StationActivityType_ID { get; set; }

    public short? Request_User_Deletion_Id { get; set; }

    public long? Station_Accreditation_Committee_ID { get; set; }

    public DateOnly? Delegation_Date { get; set; }

    public TimeOnly? StartTime { get; set; }

    public TimeOnly? EndTime { get; set; }

    public bool? IsApproved { get; set; }

    public bool? IsPaid { get; set; }

    public bool? Status { get; set; }

    public bool? IsAccepted { get; set; }

    public bool? Is_Start_Android { get; set; }

    public bool? Is_Cancel { get; set; }

    public short? Committee_User_Deletion_Id { get; set; }

    public string Expr1 { get; set; } = null!;

    public int Expr2 { get; set; }
}
