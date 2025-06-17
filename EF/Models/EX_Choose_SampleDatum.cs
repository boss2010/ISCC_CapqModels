using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class EX_Choose_SampleDatum
{
    public long ID { get; set; }

    public long? Item_ShortName_ID { get; set; }

    public int? AnalysisLabType_ID { get; set; }

    public long? Ex_CountryConstrain_AnalysisLabType_ID { get; set; }

    public long? Ex_CheckRequest_ID { get; set; }

    public virtual AnalysisLabType? AnalysisLabType { get; set; }

    public virtual Ex_CheckRequest? Ex_CheckRequest { get; set; }

    public virtual Ex_CountryConstrain_AnalysisLabType? Ex_CountryConstrain_AnalysisLabType { get; set; }

    public virtual Item_ShortName? Item_ShortName { get; set; }
}
