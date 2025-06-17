using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Ex_OpertaionType
{
    public int ID { get; set; }

    public string? Name_Ar { get; set; }

    public string? Name_En { get; set; }

    public byte? withPermission { get; set; }
}
