﻿using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblProductDetailsMaster
{
    public Guid Id { get; set; }

    public Guid VendorId { get; set; }

    public int? ProductType { get; set; }

    public string ProductName { get; set; } = null!;

    public string ProductDescription { get; set; } = null!;

    public string? ProductShortDescription { get; set; }

    public string? ProductImage { get; set; }

    public decimal ProductStocks { get; set; }

    public decimal PerUnitPrice { get; set; }

    public int? Hsn { get; set; }

    public decimal Gst { get; set; }

    public decimal PerUnitWithGstprice { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public virtual TblProductTypeMaster? ProductTypeNavigation { get; set; }
}