using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class A_SystemCode
{
    public int Id { get; set; }

    public string? ValueName { get; set; }

    public string? ValueNameEN { get; set; }

    public int? SystemCodeTypeId { get; set; }

    public byte? Value { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<CommitteeEmployee> CommitteeEmployees { get; set; } = new List<CommitteeEmployee>();

    public virtual ICollection<CommitteeType> CommitteeTypes { get; set; } = new List<CommitteeType>();

    public virtual ICollection<CompanyActivity> CompanyActivities { get; set; } = new List<CompanyActivity>();

    public virtual ICollection<Ex_ContactDatum> Ex_ContactData { get; set; } = new List<Ex_ContactDatum>();

    public virtual ICollection<HagrContact> HagrContacts { get; set; } = new List<HagrContact>();

    public virtual ICollection<Im_Request_Port> Im_Request_PortIsNationalNavigations { get; set; } = new List<Im_Request_Port>();

    public virtual ICollection<Im_Request_Port> Im_Request_PortReqPortTypes { get; set; } = new List<Im_Request_Port>();

    public virtual ICollection<Im_ScientificResearch_ItemPlant_Inseket_Lieble> Im_ScientificResearch_ItemPlant_Inseket_Liebles { get; set; } = new List<Im_ScientificResearch_ItemPlant_Inseket_Lieble>();

    public virtual ICollection<Im_ScientificResearch_ItemPlant_Product> Im_ScientificResearch_ItemPlant_Products { get; set; } = new List<Im_ScientificResearch_ItemPlant_Product>();

    public virtual ICollection<Im_Warehouse> Im_Warehouses { get; set; } = new List<Im_Warehouse>();

    public virtual ICollection<LiableItem> LiableItems { get; set; } = new List<LiableItem>();

    public virtual ICollection<Outlet> Outlets { get; set; } = new List<Outlet>();

    public virtual ICollection<Person> People { get; set; } = new List<Person>();

    public virtual ICollection<Station_Accreditation_Datum> Station_Accreditation_Data { get; set; } = new List<Station_Accreditation_Datum>();

    public virtual A_SystemCodeType? SystemCodeType { get; set; }
}
