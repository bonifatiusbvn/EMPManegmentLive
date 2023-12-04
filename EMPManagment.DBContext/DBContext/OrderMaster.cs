using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class OrderMaster
{
    public Guid Id { get; set; }

    public string OrderId { get; set; } = null!;

    public string CompanyName { get; set; } = null!;

    public string Product { get; set; } = null!;

    public string Quantity { get; set; } = null!;

    public decimal? Amount { get; set; }

    public decimal? Total { get; set; }

    public DateTime OrderDate { get; set; }

    public DateTime? DeliveryDate { get; set; }

    public string? PaymentMethod { get; set; }

    public string? DeliveryStatus { get; set; }

    public DateTime? CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }
}
