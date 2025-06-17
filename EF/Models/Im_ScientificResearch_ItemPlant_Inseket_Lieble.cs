using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// افات/حشرات/كائنات
/// </summary>
public partial class Im_ScientificResearch_ItemPlant_Inseket_Lieble
{
    public long ID { get; set; }

    public long Im_ScientificResearch_ID { get; set; }

    /// <summary>
    /// الاسم العلمى
    /// </summary>
    public string Scientific_Name { get; set; } = null!;

    /// <summary>
    /// السلالة
    /// </summary>
    public short? LiableItems_Strain_Id { get; set; }

    /// <summary>
    /// الحالة
    /// </summary>
    public int? LiableItems_Status_Id { get; set; }

    /// <summary>
    /// نوع العبوة
    /// </summary>
    public short Package_Type_Id { get; set; }

    /// <summary>
    /// ملخص الاجراءات
    /// </summary>
    public string Procedure_Summery { get; set; } = null!;

    /// <summary>
    /// نوع الرسالة from systemcode=18
    /// 
    /// </summary>
    public int Research_Type_Id { get; set; }

    /// <summary>
    /// الطور الحيوى
    /// </summary>
    public int Biological_Phase_id { get; set; }

    public virtual Biological_Phase Biological_Phase { get; set; } = null!;

    public virtual Im_ScientificResearch Im_ScientificResearch { get; set; } = null!;

    public virtual LiableItems_Status? LiableItems_Status { get; set; }

    public virtual ItemCategories_Type? LiableItems_Strain { get; set; }

    public virtual Package_Type Package_Type { get; set; } = null!;

    public virtual A_SystemCode Research_Type { get; set; } = null!;
}
