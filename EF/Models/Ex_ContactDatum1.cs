using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Ex_ContactDatum1
{
    public long ID { get; set; }

    public long Exporter_ID { get; set; }

    public byte ContactType_ID { get; set; }

    public int ExporterType_Id { get; set; }

    public string Value { get; set; } = null!;

    public bool IsActive { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short? User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }
}
