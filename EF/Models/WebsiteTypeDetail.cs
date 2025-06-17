using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class WebsiteTypeDetail
{
    public int ID { get; set; }

    public string? TitleAr { get; set; }

    public string? TitleEn { get; set; }

    public string? descAr { get; set; }

    public string? descEn { get; set; }

    public DateTime? Date { get; set; }

    public string? filepath { get; set; }

    public int? WebsitetypeID { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public bool? IsActive { get; set; }
    //public bool EditMode { get; set; }

    public string? linkURL { get; set; }
    
}
