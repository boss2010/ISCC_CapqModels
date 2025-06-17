using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// الرسائل العليمة
/// </summary>
public partial class Im_ScientificResearch
{
    public long ID { get; set; }

    public long? ImPermission_ID { get; set; }

    public long Im_ScientificResearch_Person_Id { get; set; }

    public long Im_ScientificResearch_Organization_Id { get; set; }

    /// <summary>
    /// الوظيفة الحالية
    /// </summary>
    public string? Person_Job_Title_Ar { get; set; }

    /// <summary>
    /// المدير الحالى
    /// </summary>
    public string? Org_Manager_Name_Ar { get; set; }

    public string? Org_Manager_Name_En { get; set; }

    /// <summary>
    /// ميناء الدخول
    /// </summary>
    public int PortNational_Id { get; set; }

    /// <summary>
    /// رقم الشهادة الجمركية
    /// </summary>
    public string Tax_Certificate_Number { get; set; } = null!;

    /// <summary>
    /// رقم بوليصة الشحن
    /// </summary>
    public string Shipment_Policy_Number { get; set; } = null!;

    /// <summary>
    /// وسيلة الشحن
    /// </summary>
    public byte Shipment_Mean_Id { get; set; }

    /// <summary>
    /// وسيلة النقل
    /// </summary>
    public byte Transport_Mean_Id { get; set; }

    /// <summary>
    /// العدد
    /// </summary>
    public short Quantity { get; set; }

    public short User_Creation_Id { get; set; }

    public DateTime User_Creation_Date { get; set; }

    public short? User_Updation_Id { get; set; }

    public DateTime? User_Updation_Date { get; set; }

    public short? User_Deletion_Id { get; set; }

    public DateTime? User_Deletion_Date { get; set; }

    /// <summary>
    /// اسم وسيلة الشحن
    /// </summary>
    public string? Transport_Mean_Name { get; set; }

    public virtual Im_PermissionRequest? ImPermission { get; set; }

    public virtual ICollection<Im_ScientificResearch_ItemPlant_Inseket_Lieble> Im_ScientificResearch_ItemPlant_Inseket_Liebles { get; set; } = new List<Im_ScientificResearch_ItemPlant_Inseket_Lieble>();

    public virtual ICollection<Im_ScientificResearch_ItemPlant_Product> Im_ScientificResearch_ItemPlant_Products { get; set; } = new List<Im_ScientificResearch_ItemPlant_Product>();

    public virtual Im_ScientificResearch_Organization Im_ScientificResearch_Organization { get; set; } = null!;

    public virtual Im_ScientificResearch_Person Im_ScientificResearch_Person { get; set; } = null!;

    public virtual PortNational PortNational { get; set; } = null!;

    public virtual Shipment_Mean Shipment_Mean { get; set; } = null!;

    public virtual Transport_Mean Transport_Mean { get; set; } = null!;
}
