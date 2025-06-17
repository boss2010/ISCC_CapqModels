using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// اماكن التحفظ للطلب
/// </summary>
public partial class Im_CustodyPlace_CheckRequest
{
    public long ID { get; set; }

    /// <summary>
    /// رقم اذن الاستيراد
    /// </summary>
    public long Im_CheckRequest_ID { get; set; }

    /// <summary>
    /// 0 if exporter doesn&apos;t accept else 1
    /// </summary>
    public bool? IsApproved { get; set; }

    /// <summary>
    /// 0 if not done, 1 if investigation is done
    /// </summary>
    public bool? Status { get; set; }

    public long User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public long? Im_CustodyPlace_ID { get; set; }

    /// <summary>
    /// المحطة
    /// </summary>
    public long? Station_ID { get; set; }

    public virtual Im_CheckRequest Im_CheckRequest { get; set; } = null!;

    public virtual ICollection<Im_Committee_CustodyPlace> Im_Committee_CustodyPlaces { get; set; } = new List<Im_Committee_CustodyPlace>();

    public virtual Im_CustodyPlace? Im_CustodyPlace { get; set; }

    public virtual ICollection<Im_PermissionItem_Division_Custody> Im_PermissionItem_Division_Custodies { get; set; } = new List<Im_PermissionItem_Division_Custody>();

    public virtual Station? Station { get; set; }
}
