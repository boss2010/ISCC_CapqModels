using System;
using System.Collections.Generic;

namespace EF.Models;

public partial class Im_choose_Constrain_
{
    public long ID { get; set; }

    public byte? Im_Constrain_Type_Id { get; set; }

    public long? Im_Constrains_Special_Id { get; set; }

    public long? Im_Constrain_Initiator_Text_Id { get; set; }

    public virtual Im_Constrain_Initiator_Text? Im_Constrain_Initiator_Text { get; set; }

    public virtual Im_Constrain_Type? Im_Constrain_Type { get; set; }

    public virtual Im_Constrains_Special? Im_Constrains_Special { get; set; }
}
