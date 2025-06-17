using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// اسماء الجداول فى قواعد البيانات
/// </summary>
public partial class A_AttachmentTableName
{
    public short ID { get; set; }

    public string? TableName { get; set; }

    public string? Description { get; set; }

    public byte? PR_Module_ID { get; set; }

    public virtual ICollection<A_AttachmentDatum> A_AttachmentData { get; set; } = new List<A_AttachmentDatum>();

    public virtual ICollection<A_AttachmentData_Ex_CheckRequest> A_AttachmentData_Ex_CheckRequests { get; set; } = new List<A_AttachmentData_Ex_CheckRequest>();

    public virtual ICollection<A_AttachmentData_Station> A_AttachmentData_Stations { get; set; } = new List<A_AttachmentData_Station>();

    public virtual ICollection<Table_Action> Table_Actions { get; set; } = new List<Table_Action>();
}
