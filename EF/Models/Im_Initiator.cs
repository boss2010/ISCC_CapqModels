using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// المناشىء
/// </summary>
public partial class Im_Initiator
{
    public long ID { get; set; }

    public short? Country_Id { get; set; }

    public long? Item_ShortName_ID { get; set; }

    /// <summary>
    /// المجموعة النوعية
    /// </summary>
    public short? QualitativeGroup_Id { get; set; }

    /// <summary>
    /// حالة المنشأ
    /// from systemcode 16
    /// 
    /// </summary>
    public int Initiator_Status { get; set; }

    public bool IsActive { get; set; }

    public string? ForbiddenReason { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public string? AttachmentPath { get; set; }

    public virtual Country? Country { get; set; }

    public virtual ICollection<Im_CheckRequest_Item> Im_CheckRequest_Items { get; set; } = new List<Im_CheckRequest_Item>();

    public virtual ICollection<Im_Constrain_Initiator_Text> Im_Constrain_Initiator_Texts { get; set; } = new List<Im_Constrain_Initiator_Text>();

    public virtual ICollection<Im_PermissionItem> Im_PermissionItems { get; set; } = new List<Im_PermissionItem>();

    public virtual Item_ShortName? Item_ShortName { get; set; }
}
