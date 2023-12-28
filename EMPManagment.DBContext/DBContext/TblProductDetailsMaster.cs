using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblProductDetailsMaster
{
    public Guid Id { get; set; }

    public int ProductType { get; set; }

    public string? ProductName { get; set; }

    public string? ProductDescription { get; set; }

    public string? ProductShortDescription { get; set; }

    public string? ProductImage { get; set; }

    public string? ProductStocks { get; set; }

    public decimal? ProductPrice { get; set; }

    public virtual TblProductTypeMaster ProductTypeNavigation { get; set; } = null!;
}
