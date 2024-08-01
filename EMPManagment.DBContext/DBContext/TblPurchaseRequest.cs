using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblPurchaseRequest
{
    public Guid PrId { get; set; }

    public Guid UserId { get; set; }

    public string PrNo { get; set; } = null!;

    public Guid ProjectId { get; set; }

    public Guid? ProductId { get; set; }

    public DateTime? PrDate { get; set; }

    public DateTime? Date { get; set; }

    public string ProductName { get; set; } = null!;

    public int ProductTypeId { get; set; }

    public decimal Quantity { get; set; }

    public bool? IsApproved { get; set; }

    public bool? IsDeleted { get; set; }

    public Guid CreatedBy { get; set; }

    public DateTime CreatedOn { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual TblProductDetailsMaster? Product { get; set; }

    public virtual TblProjectMaster Project { get; set; } = null!;

    public virtual TblUser User { get; set; } = null!;
}
