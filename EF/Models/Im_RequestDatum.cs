using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// تفاصيل عناصر الوارد
/// </summary>
public partial class Im_RequestDatum
{
    public long ID { get; set; }

    /// <summary>
    /// نوع إذن الاستراد
    /// </summary>
    public byte? Im_OperationType { get; set; }

    /// <summary>
    /// الشركة/الهيئة/الفرد المستوردة
    /// </summary>
    public long Importer_ID { get; set; }

    /// <summary>
    /// from systemcode table 3
    /// </summary>
    public int ImporterType_Id { get; set; }

    /// <summary>
    /// الدولة المصدرة
    /// </summary>
    public short ExportCountry_Id { get; set; }

    /// <summary>
    /// وسيلة الشحن
    /// </summary>
    public byte? Shipment_Mean_Id { get; set; }

    /// <summary>
    /// وسيلة النقل
    /// </summary>
    public byte? Transport_Mean_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public long? Im_PermissionRequest_ID { get; set; }

    public long? Im_CheckRequest_ID { get; set; }

    /// <summary>
    /// اسم الباخرة
    /// </summary>
    public string? Ship_Name { get; set; }

    /// <summary>
    /// مندوب صاحب الرسالة
    /// </summary>
    public string? DelegateName { get; set; }

    /// <summary>
    /// عنوان مندوب صاحب الرسالة
    /// </summary>
    public string? DelegateAddress { get; set; }

    public virtual Country ExportCountry { get; set; } = null!;

    public virtual ICollection<Im_RequestDat_Extra> Im_RequestDat_Extras { get; set; } = new List<Im_RequestDat_Extra>();

    public virtual ICollection<Im_Request_Port> Im_Request_Ports { get; set; } = new List<Im_Request_Port>();

    public virtual Shipment_Mean? Shipment_Mean { get; set; }

    public virtual Transport_Mean? Transport_Mean { get; set; }
}
