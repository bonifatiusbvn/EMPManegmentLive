using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels.Purchase_Request
{
    public class PurchaseRequestModel
    {
        public Guid PrId { get; set; }

        public Guid UserId { get; set; }

        public Guid ProjectId { get; set; }

        public string PrNo { get; set; } = null!;

        public Guid? ProductId { get; set; }

        public string ProductName { get; set; } = null!;

        public int ProductTypeId { get; set; }

        public decimal Quantity { get; set; }

        public bool? IsApproved { get; set; }

        public bool? IsDeleted { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public string? UserName { get; set; }

        public string? ProjectName { get; set; }

        public int RowNumber {get; set; }

        public string? FullName {get; set; }

        public string? ProductTypeName {get; set; }

        public decimal? Price { get; set; }

        public decimal? ProductTotal { get; set; }

        public decimal? PerUnitWithGstprice { get; set; }

        public string? ProductImage { get; set; }

    }
    public class PurchaseRequestMasterView
    {
        public string? PrNo { get; set; }
        public decimal? SubTotal { get; set; }
        public List<PurchaseRequestModel>? PRList { get; set; }
    }   
}
