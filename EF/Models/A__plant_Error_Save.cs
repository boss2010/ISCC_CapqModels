using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class A__plant_Error_Save
{
    public long Id { get; set; }

    public string PageName { get; set; } = null!;

    public string ErrorMessage { get; set; } = null!;

    public DateTime Date { get; set; }

    public string FunctionName { get; set; } = null!;

    public string User_Ip { get; set; } = null!;

    /// <summary>
    /// 1-&gt;web, 0-&gt;android
    /// </summary>
    public bool IsWeb { get; set; }
}
