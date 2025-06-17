using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// لجنة الاعدام
/// </summary>
public partial class Im_Execution
{
    public long ID { get; set; }

    public long Im_RequestCommittee_Id { get; set; }

    public string Execution_Place { get; set; } = null!;

    public string Execution_Method { get; set; } = null!;

    public byte[]? Execution_File { get; set; }

    public virtual ICollection<Im_Execution_Item> Im_Execution_Items { get; set; } = new List<Im_Execution_Item>();

    public virtual Im_RequestCommittee Im_RequestCommittee { get; set; } = null!;
}
