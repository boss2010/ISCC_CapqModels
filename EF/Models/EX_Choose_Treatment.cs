using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// اختيار المعالجه
/// </summary>
public partial class EX_Choose_Treatment
{
    public long ID { get; set; }

    public long? Ex_CheckRequest_ID { get; set; }

    public byte? TreatmentMethods_ID { get; set; }

    /// <summary>
    /// product or plant ID manual no relation
    /// </summary>
    public long Item_ShortName_id { get; set; }

    /// <summary>
    /// التحليل اختيارى =0 
    /// التحليل اجباري =1
    /// </summary>
    public bool IS_Optional { get; set; }

    public long? Ex_CountryConstrain_Treatment_id { get; set; }

    public virtual Ex_CheckRequest? Ex_CheckRequest { get; set; }

    public virtual Ex_CountryConstrain_Treatment? Ex_CountryConstrain_Treatment { get; set; }

    public virtual TreatmentMethod? TreatmentMethods { get; set; }
}
