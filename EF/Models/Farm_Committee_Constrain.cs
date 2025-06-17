using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// لجنه المزرعه
/// </summary>
public partial class Farm_Committee_Constrain
{
    public long ID { get; set; }

    public long? Farm_Constrain_ID { get; set; }

    public long? Farm_Committee_ID { get; set; }

    public virtual Farm_Committee? Farm_Committee { get; set; }

    public virtual Farm_Constrain? Farm_Constrain { get; set; }
}
