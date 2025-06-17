using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// أعضاء اللجنة
/// </summary>
public partial class CommitteeEmployee
{
    public long Committee_ID { get; set; }

    public long Employee_Id { get; set; }

    /// <summary>
    /// is admin for the current committee
    /// </summary>
    public bool ISAdmin { get; set; }

    /// <summary>
    /// from system code 20 (export :73 , Import 74, Farm 78) 
    /// </summary>
    public int OperationType { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public virtual A_SystemCode OperationTypeNavigation { get; set; } = null!;
}
