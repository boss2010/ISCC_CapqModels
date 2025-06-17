using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// تفاصيل الشركة او الهيئة
/// </summary>
public partial class Im_RequestDat_Extra
{
    public long ID { get; set; }

    /// <summary>
    /// طلب الفحص
    /// </summary>
    public long Im_RequestData_ID { get; set; }

    /// <summary>
    /// الشركة المستوردة
    /// </summary>
    public string? ImportCompany { get; set; }

    /// <summary>
    /// عنوان مندوب صاحب الرسالة
    /// </summary>
    public string? ImporeterCompanyAddress { get; set; }

    /// <summary>
    /// اسم المرسل إليه
    /// </summary>
    public string? Reciever_Name { get; set; }

    /// <summary>
    /// صاحب الرسالة
    /// </summary>
    public string? OwnerName { get; set; }

    /// <summary>
    /// عنوان صلحب الرسالة
    /// </summary>
    public string? OwnerAddress { get; set; }

    /// <summary>
    /// الشركة المستوردة
    /// </summary>
    public string? ImportCompany_EN { get; set; }

    /// <summary>
    /// عنوان مندوب صاحب الرسالة
    /// </summary>
    public string? ImporeterCompanyAddress_EN { get; set; }

    public virtual Im_RequestDatum Im_RequestData { get; set; } = null!;
}
