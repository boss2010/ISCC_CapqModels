using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// المرفقات الخاصة بالشهادة
/// </summary>
public partial class Ex_CertificatesRequestsFile
{
    public long ID { get; set; }

    public long PlantCertificatesRequestsID { get; set; }

    public string? FilePath { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public long? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public short? A_AttachmentTableTypeID { get; set; }

    public virtual A_AttachmentTableType? A_AttachmentTableType { get; set; }

    public virtual Ex_CertificatesRequest PlantCertificatesRequests { get; set; } = null!;
}
