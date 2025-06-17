using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// موظفين المنافذ
/// </summary>
public partial class Outlet_Employee
{
    public long ID { get; set; }

    /// <summary>
    /// المنافذ
    /// </summary>
    public long Outlet_ID { get; set; }

    /// <summary>
    /// الموظف
    /// </summary>
    public short Employee_Id { get; set; }

    public short? User_Updation_Id { get; set; }

    public bool IsActive { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public virtual Outlet Outlet { get; set; } = null!;
}
