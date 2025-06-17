using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class A__User_Login
{
    public long Id { get; set; }

    public long User_Id { get; set; }

    public DateTime Login_Date { get; set; }

    public DateTime? LogOut_Date { get; set; }

    public string AccessToken { get; set; } = null!;
}
