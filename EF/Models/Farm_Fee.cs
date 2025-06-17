using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Farm_Fee
{
    public int FarmFeesID { get; set; }

    public int? acreStart { get; set; }

    public int? acreEnd { get; set; }

    public decimal? fees { get; set; }

    /// <summary>
    /// null-&gt; for user , value -&gt; if the admin add the row
    /// </summary>
    public short? User_Creation_Id { get; set; }

    /// <summary>
    /// null-&gt; for user , value -&gt; if the admin add the row
    /// </summary>
    public DateTime? User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public bool? Type { get; set; }
}
