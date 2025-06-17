using System;
using System.Collections.Generic;

namespace EF.Models;

/// <summary>
/// عناصر لجنة الاعدام
/// </summary>
public partial class Im_Execution_Item
{
    public long ID { get; set; }

    public long Im_Execution_Id { get; set; }

    public long Im_CheckRequest_Item_ID { get; set; }

    public decimal GrossWeight { get; set; }

    public long? Im_CheckRequest_Items_Lot_Category_ID { get; set; }

    public virtual Im_CheckRequest_Item Im_CheckRequest_Item { get; set; } = null!;

    public virtual Im_CheckRequest_Items_Lot_Category? Im_CheckRequest_Items_Lot_Category { get; set; }

    public virtual Im_Execution Im_Execution { get; set; } = null!;
}
