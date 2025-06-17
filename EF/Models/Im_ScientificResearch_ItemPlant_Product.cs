using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// النبات/ منتج
/// </summary>
public partial class Im_ScientificResearch_ItemPlant_Product
{
    public long ID { get; set; }

    public long Im_ScientificResearch_ID { get; set; }

    /// <summary>
    /// اسم النبات او المنتج
    /// </summary>
    public string ProdPlant__Name { get; set; } = null!;

    /// <summary>
    /// حالة المنتج او النبات
    /// </summary>
    public int ProductStatus_Id { get; set; }

    /// <summary>
    /// اسم الجزء النباتى فى حالة النبات
    /// </summary>
    public string PlantPart_Name { get; set; } = null!;

    /// <summary>
    /// الاسم العلمى
    /// </summary>
    public string Scientific_Name { get; set; } = null!;

    /// <summary>
    /// ملخص الاجراءات
    /// </summary>
    public string Procedure_Summery { get; set; } = null!;

    /// <summary>
    /// الأصناف الزراعية
    /// </summary>
    public string? PlantCategories { get; set; }

    /// <summary>
    /// نوع الرسالة from systemcode=18
    /// 
    /// </summary>
    public int Research_Type_Id { get; set; }

    public virtual Im_ScientificResearch Im_ScientificResearch { get; set; } = null!;

    public virtual Item_Status ProductStatus { get; set; } = null!;

    public virtual A_SystemCode Research_Type { get; set; } = null!;
}
