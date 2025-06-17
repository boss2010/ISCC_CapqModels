using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class A_AttachmentDatum1
{
    public long Id { get; set; }

    public long RowId { get; set; }

    public short A_AttachmentTableNameId { get; set; }

    public string? Attachment_Number { get; set; }

    public string? Attachment_TypeName { get; set; }

    public string? AttachmentPath { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public byte[]? AttachmentPath_Binary { get; set; }

    public short? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }
}
