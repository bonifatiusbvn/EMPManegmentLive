using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblProductTypeMaster
{
    public int Id { get; set; }

    public string? ProductType { get; set; }

    public virtual ICollection<TblProductDetailsMaster> TblProductDetailsMasters { get; set; } = new List<TblProductDetailsMaster>();
}
