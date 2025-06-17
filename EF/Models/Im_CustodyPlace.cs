using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// اماكن التحفظ
/// </summary>
public partial class Im_CustodyPlace
{
    public long ID { get; set; }

    /// <summary>
    /// الوصف انجليزى
    /// </summary>
    public string En_Desc { get; set; } = null!;

    /// <summary>
    /// الوصف عربى
    /// </summary>
    public string Ar_Desc { get; set; } = null!;

    public double Storage_capacity { get; set; }

    public short Center_Id { get; set; }

    public string Address { get; set; } = null!;

    public string Owner_Name { get; set; } = null!;

    public string NationalID { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public double PreviewQuantityDuration { get; set; }

    /// <summary>
    /// كمية/تاريخ
    /// </summary>
    public DateTime? DateStored { get; set; }

    /// <summary>
    /// الكمية المخزنة/التاريخ
    /// </summary>
    public double Quantity { get; set; }

    /// <summary>
    /// 0 if exporter doesn&apos;t accept else 1
    /// </summary>
    public bool? IsApproved { get; set; }

    /// <summary>
    /// 0 if not done, 1 if investigation is done
    /// </summary>
    public bool? Status { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    /// <summary>
    /// مخزن/ساحة
    /// </summary>
    public byte Im_CustodyPlaceType { get; set; }

    public virtual Im_CustodyPlaceType Im_CustodyPlaceTypeNavigation { get; set; } = null!;

    public virtual ICollection<Im_CustodyPlace_CheckRequest> Im_CustodyPlace_CheckRequests { get; set; } = new List<Im_CustodyPlace_CheckRequest>();
}
