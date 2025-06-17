using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// الدول
/// </summary>
public partial class Country
{
    public short ID { get; set; }

    /// <summary>
    /// الاسم بالعربية
    /// </summary>
    public string? Ar_Name { get; set; }

    /// <summary>
    /// الاسم بالانجليزية
    /// </summary>
    public string? En_Name { get; set; }

    public bool Is_IPPC { get; set; }

    public bool IsActive { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    public short? User_Creation_Id { get; set; }

    public DateTime? User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public byte? Continents_ID { get; set; }

    public byte? Regional_Area_ID { get; set; }

    public virtual ICollection<CompanyAccreditation> CompanyAccreditations { get; set; } = new List<CompanyAccreditation>();

    public virtual Continent? Continents { get; set; }

    public virtual ICollection<Ex_CheckRequest_Datum> Ex_CheckRequest_Data { get; set; } = new List<Ex_CheckRequest_Datum>();

    public virtual ICollection<Farm_Constrain> Farm_Constrains { get; set; } = new List<Farm_Constrain>();

    public virtual ICollection<Farm_Country> Farm_Countries { get; set; } = new List<Farm_Country>();

    public virtual ICollection<Farm_Country_CheckList> Farm_Country_CheckLists { get; set; } = new List<Farm_Country_CheckList>();

    public virtual ICollection<Im_CheckRequest_Datum> Im_CheckRequest_Data { get; set; } = new List<Im_CheckRequest_Datum>();

    public virtual ICollection<Im_Initiator> Im_Initiators { get; set; } = new List<Im_Initiator>();

    public virtual ICollection<Im_RequestDatum> Im_RequestData { get; set; } = new List<Im_RequestDatum>();

    public virtual ICollection<Person> People { get; set; } = new List<Person>();

    public virtual ICollection<Port_International> Port_Internationals { get; set; } = new List<Port_International>();

    public virtual Regional_Area? Regional_Area { get; set; }

    public virtual ICollection<Region> Regions { get; set; } = new List<Region>();

    public virtual ICollection<Station_Accreditation_Data_Country> Station_Accreditation_Data_Countries { get; set; } = new List<Station_Accreditation_Data_Country>();

    public virtual ICollection<Union_Country> Union_Countries { get; set; } = new List<Union_Country>();
}
