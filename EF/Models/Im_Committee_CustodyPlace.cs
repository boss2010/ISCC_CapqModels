using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// لجنة معاينة مكان التحفظ
/// </summary>
public partial class Im_Committee_CustodyPlace
{
    public int ID { get; set; }

    public long Im_CustodyPlace_Id { get; set; }

    /// <summary>
    /// تاريخ الفحص
    /// </summary>
    public DateOnly? Check_Date { get; set; }

    /// <summary>
    ///  بداية ساعة الفحص 
    /// </summary>
    public TimeOnly? StartTime { get; set; }

    /// <summary>
    /// انتهاء ساعة الفحص
    /// </summary>
    public TimeOnly? EndTime { get; set; }

    /// <summary>
    /// الوزن
    /// </summary>
    public double Weight { get; set; }

    /// <summary>
    /// العدد
    /// </summary>
    public short Quantity { get; set; }

    /// <summary>
    /// هل حاوية أم لا
    /// </summary>
    public bool IsPackage { get; set; }

    /// <summary>
    /// 0 if exporter doesn&apos;t accept else 1
    /// </summary>
    public short IsApproved { get; set; }

    /// <summary>
    /// 0 if not done, 1 if investigation is done
    /// </summary>
    public bool Status { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public virtual Im_CustodyPlace_CheckRequest Im_CustodyPlace { get; set; } = null!;
}
