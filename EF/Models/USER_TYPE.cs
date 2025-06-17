using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class USER_TYPE
{
    public int ID { get; set; }

    public string UserID { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool IsCompany { get; set; }

    public long IDSource { get; set; }

    public bool? ISAdmin { get; set; }

    public int? UserType { get; set; }

    public long? User_Delete_Id { get; set; }

    public DateTime? User_Delete_Date { get; set; }

    public long? User_Updata_Id { get; set; }

    public DateTime? User_Updata_Date { get; set; }
}
