using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Fees_Money
{
    public byte ID { get; set; }

    public decimal? Fees { get; set; }

    public byte? Type { get; set; }
}
