using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// الاعتمادات وقوائم الاندريد
/// </summary>
public partial class Station_Accreditation_CheckList
{
    public long ID { get; set; }

    public long Station_Accreditation_Data_ID { get; set; }

    public long Station_CheckList_ID { get; set; }

    public bool IsActive { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public virtual ICollection<Station_Accreditation_Committee_CheckList> Station_Accreditation_Committee_CheckLists { get; set; } = new List<Station_Accreditation_Committee_CheckList>();

    public virtual Station_Accreditation_Datum Station_Accreditation_Data { get; set; } = null!;

    public virtual Station_CheckList Station_CheckList { get; set; } = null!;
}
