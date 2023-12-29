using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels.SalesFolder
{
    public class CreateInvoiceModle
    {
        public Guid Id { get; set; }
        public int? ProductType { get; set; }
        public string? ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public string? ProductShortDescription { get; set; }
        public string? ProductImage { get; set; }
        public decimal? ProductStocks { get; set; }
        public decimal? PerUnitPrice { get; set; }
        public int? HSN { get; set; }
        public decimal? GST { get; set; }
        public decimal? PerUnitWithGSTPrice { get; set; }
        public DateTime? CreatedOn { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public Guid? UpdatedBy { get; set; }
       
    }
}
