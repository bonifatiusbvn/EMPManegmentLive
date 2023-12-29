using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace EMPManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> GenerateInvoice(string InvoiceNo) 
        {
            var document = new PdfDocument();
            //string HtmlContent = "<h1> Welcome to Bonifatius</h1>";
            string htmlcontent = "<div style='width:100%; text-align:center'>";
            htmlcontent += "<h1> Welcome to Bonifatius</h1>";

            htmlcontent += "<h2> Invoice No : IV0004 & Date: 4/12/2000 </h2>";
            htmlcontent += "<h3> CompanyAddress : Akota </h3> ";
            htmlcontent += "<h3> Legal Registration No : 123gst </h3>";
            htmlcontent += "<h3> EmailAddress : demo@gmail.com </h3>";
            htmlcontent += "<div style='text-align:left'>";
            htmlcontent += "<p> ShippingName : Rita </p>";
            htmlcontent += "<p> ShippingAddress : Shnati Nagar </p>";
            htmlcontent += "<p> ShippingPhoneNo : 9867561230 </p>";
            htmlcontent += "<p> ShippingTaxNo : abc123cst </p>";
            htmlcontent += "</div>";
            htmlcontent += "<div style='text-align:right'>";
            htmlcontent += "<p> BillingName : Sita </p>";
            htmlcontent += "<p> BillingName : Sharda Nagar </p>";
            htmlcontent += "<p> BillingPhoneNo : 9800491230 </p>";
            htmlcontent += "<p> BillingTaxNo : 1a2b3ccst </p>";
            htmlcontent += "</div>";

            htmlcontent += "<div>";
            htmlcontent += "<table style='width:100%; border:1px solid #000'>";
            htmlcontent += "<thead style='font-weight:bold'>";
            htmlcontent += "<tr>";
            htmlcontent += "<td style='border:1px; solid:#000'>Product Name</td>";
            htmlcontent += "<td style='border:1px; solid:#000'>Product Details</td>";
            htmlcontent += "<td style='border:1px; solid:#000'>Qty</td>";
            htmlcontent += "<td style='border:1px; solid:#000'>Price</td>";
            htmlcontent += "<td style='border:1px; solid:#000'>Total</td>";
            htmlcontent += "</tr>";
            htmlcontent += "</thead>";
            htmlcontent += "<tbody>";
            htmlcontent += "<tr>";
            htmlcontent += "<td>" + "abc" + "</td>";
            htmlcontent += "<td>" + "abc" + "</td>";
            htmlcontent += "<td>" + "2" + "</td>";
            htmlcontent += "<td>" + "500" + "</td>";
            htmlcontent += "<td>" + "1000" + "</td>";
            htmlcontent += "</tr>";
            htmlcontent += "</tbody>";
            htmlcontent += "</table>";
            htmlcontent += "</div>";

            htmlcontent += "<div style='text-align:right'>";
            htmlcontent += "<h2>Summary Info</h2>";
            htmlcontent += "<h4> Total Amount : 1000 </h4>";
            htmlcontent += "</div>";

            PdfGenerator.AddPdfPages(document, htmlcontent, PageSize.A4);
            byte[]? response = null;
            using (MemoryStream ms = new MemoryStream())
            {
                document.Save(ms);
                response = ms.ToArray();
            }
            string Filename = "Invoice_" + InvoiceNo + ".pdf";
            return File(response, "application/pdf", Filename);
        }
    }
}
