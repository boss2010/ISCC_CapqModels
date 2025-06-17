using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Table_Action_Log
{
    public long ID { get; set; }

    public short ID_Table_Action { get; set; }

    public long? ID_TableActionValue { get; set; }

    public long? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public string? NOTS { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    /// <summary>
    /// from A_SystemCode =3
    /// </summary>
    public int? User_Type_ID { get; set; }

    /// <summary>
    /// from A_SystemCode =32
    /// </summary>
    public int? Type_log_ID { get; set; }

    public long? Im_PermissionRequest_ID { get; set; }

    public virtual Table_Action ID_Table_ActionNavigation { get; set; } = null!;
}
