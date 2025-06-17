using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Station_Requst
{
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

    public long Station_ID { get; set; }

    public long Station_Accreditation_Request_ID { get; set; }

    public int? Accreditation_Type_ID { get; set; }

    public byte? StationActivityType_ID { get; set; }

    public short? Request_User_Deletion_Id { get; set; }
}
