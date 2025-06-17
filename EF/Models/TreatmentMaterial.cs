using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// مادة المعالجة
/// </summary>
public partial class TreatmentMaterial
{
    public byte ID { get; set; }

    public long? Item_ID { get; set; }

    public byte? TreatmentMethods_ID { get; set; }

    public bool IsActive { get; set; }

    public string? ChemicalComposition { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public virtual ICollection<Ex_Request_TreatmentDatum> Ex_Request_TreatmentData { get; set; } = new List<Ex_Request_TreatmentDatum>();

    public virtual ICollection<Im_Request_TreatmentDatum> Im_Request_TreatmentData { get; set; } = new List<Im_Request_TreatmentDatum>();

    public virtual Item? Item { get; set; }

    public virtual TreatmentMethod? TreatmentMethods { get; set; }
}
