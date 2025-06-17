using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Andriod_Location
{
    public long Id { get; set; }

    public long Committe_ID { get; set; }

    /// <summary>
    /// 1-&gt;Export , 0-&gt;Import
    /// </summary>
    public bool IsExport { get; set; }

    /// <summary>
    /// the action that user made &apos;Export, Treatment, Sample Data, ....
    /// </summary>
    public byte Operation_ID { get; set; }

    public long User_Id { get; set; }

    public double Latitude { get; set; }

    public double Longitude { get; set; }

    public DateTime Created_Date { get; set; }

    public virtual Andriod_Operation Operation { get; set; } = null!;
}
