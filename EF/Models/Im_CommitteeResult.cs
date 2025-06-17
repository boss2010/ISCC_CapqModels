using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// النتيجه
/// </summary>
public partial class Im_CommitteeResult
{
    public long ID { get; set; }

    /// <summary>
    /// كود اساسيات اللجنه
    /// </summary>
    public long Committee_ID { get; set; }

    public long Im_Request_Item_Id { get; set; }

    /// <summary>
    /// null for the whole request  /  كود بيانات الدفعه
    /// </summary>
    public long? LotData_ID { get; set; }

    public long? EmployeeId { get; set; }

    public byte? CommitteeResultType_ID { get; set; }

    public DateTime? Date { get; set; }

    /// <summary>
    /// null-&gt;exporter not take action 0 if exporter doesn&apos;t accept else 1
    /// </summary>
    public bool? IsAdminResult { get; set; }

    /// <summary>
    /// Admin Note
    /// </summary>
    public string? AdminFinalResult_Note { get; set; }

    /// <summary>
    /// العدد
    /// </summary>
    public double? QuantitySize { get; set; }

    /// <summary>
    /// الوزن
    /// </summary>
    public double? Weight { get; set; }

    public string? Notes { get; set; }

    /// <summary>
    /// (0) in the case of all,(1) in the case of the part   في حاله الجزئي او الكلي
    /// </summary>
    public bool? IS_Total { get; set; }

    /// <summary>
    /// الاسم المختصر
    /// </summary>
    public long? Item_ShortName_ID { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    /// <summary>
    /// في حاله الفحص لو كلي واتحول الي جزئي
    /// </summary>
    public bool? IS_Total_Android { get; set; }

    public virtual ICollection<A_AttachmentData_Im_CommitteeResult_Infection> A_AttachmentData_Im_CommitteeResult_Infections { get; set; } = new List<A_AttachmentData_Im_CommitteeResult_Infection>();

    public virtual Im_RequestCommittee Committee { get; set; } = null!;

    public virtual CommitteeResultType? CommitteeResultType { get; set; }

    public virtual ICollection<Im_CommitteeResult_Confirm> Im_CommitteeResult_Confirms { get; set; } = new List<Im_CommitteeResult_Confirm>();

    public virtual ICollection<Im_CommitteeResult_Infection> Im_CommitteeResult_Infections { get; set; } = new List<Im_CommitteeResult_Infection>();

    public virtual Item_ShortName? Item_ShortName { get; set; }
}
