using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Station_Accreditation_Request_Fee
{
    public long ID { get; set; }

    public long Station_Accreditation_Request_ID { get; set; }

    public int Station_Fees_Type_ID { get; set; }

    public decimal? Value { get; set; }

    public bool? IsActive { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    /// <summary>
    /// تم الانتهاء من الدفع
    /// </summary>
    public bool? IsPaid { get; set; }

    public virtual Station_Accreditation_Request Station_Accreditation_Request { get; set; } = null!;

    public virtual Station_Fees_Type Station_Fees_Type { get; set; } = null!;
}
