using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Table_Action
{
    public short ID { get; set; }

    public short ID_Table_Name { get; set; }

    public string? Name_Ar { get; set; }

    public string? Name_En { get; set; }

    public string? NOTS_Ar { get; set; }

    public string? NOTS_En { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public bool? IsActive { get; set; }

    public string? NOTS { get; set; }

    public virtual A_AttachmentTableName ID_Table_NameNavigation { get; set; } = null!;

    public virtual ICollection<Table_Action_Log_CheckRequest> Table_Action_Log_CheckRequests { get; set; } = new List<Table_Action_Log_CheckRequest>();

    public virtual ICollection<Table_Action_Log_EX> Table_Action_Log_EXes { get; set; } = new List<Table_Action_Log_EX>();

    public virtual ICollection<Table_Action_Log_Farm> Table_Action_Log_Farms { get; set; } = new List<Table_Action_Log_Farm>();

    public virtual ICollection<Table_Action_Log_Station> Table_Action_Log_Stations { get; set; } = new List<Table_Action_Log_Station>();

    public virtual ICollection<Table_Action_Log> Table_Action_Logs { get; set; } = new List<Table_Action_Log>();
}
