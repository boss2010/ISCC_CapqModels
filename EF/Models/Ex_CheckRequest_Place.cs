using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Ex_CheckRequest_Place
{
    public long ID { get; set; }

    public long Ex_CheckRequest_ID { get; set; }

    /// <summary>
    ///  الميناء للجشني
    /// </summary>
    public int? PortNational_ID { get; set; }

    /// <summary>
    /// المركز
    /// </summary>
    public short? Center_ID { get; set; }

    /// <summary>
    /// المحافظة
    /// </summary>
    public short? Govern_ID { get; set; }

    /// <summary>
    /// محطة الجشني
    /// </summary>
    public long? Station_Genshi_ID { get; set; }

    /// <summary>
    /// محطة الفحص
    /// </summary>
    public long? Station_Examination_ID { get; set; }

    /// <summary>
    /// مكان الفحص
    /// </summary>
    public string? Examination_location { get; set; }

    /// <summary>
    /// مسمسي الاعتماد للمحطة
    /// </summary>
    public long? Station_Accreditation_Data_Examination_ID { get; set; }

    public long? Outlet_Exmainiation_ID { get; set; }

    public long? Outlet_Genshi_ID { get; set; }

    public virtual Center? Center { get; set; }

    public virtual Ex_CheckRequest Ex_CheckRequest { get; set; } = null!;

    public virtual Governate? Govern { get; set; }

    public virtual PortNational? PortNational { get; set; }

    public virtual Station? Station_Examination { get; set; }

    public virtual Station? Station_Genshi { get; set; }
}
