using Microsoft.AspNetCore.Mvc;
using PdfSharpCore.Pdf;
using PdfSharpCore;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using EMPManegment.EntityModels.ViewModels.SalesFolder;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using Newtonsoft.Json;
using EMPManegment.EntityModels.ViewModels.Models;

namespace EMPManegment.Web.Controllers
{
    public class SalesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateInvoice()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> GenerateInvoice(InvoiceView InvoiceData)
        {
            var document = new PdfDocument();
            //string HtmlContent = "<h1> Welcome to Bonifatius</h1>";
            string htmlcontent = "<div style='width:100%; text-align:center'>";
            htmlcontent += "<h1> Welcome to Bonifatius</h1>";

            htmlcontent += "<h2> Invoice No : "+ InvoiceData.InvoiceNo +" & Date:" + InvoiceData.Date + "</h2>";
            htmlcontent += "<h3> CompanyAddress :" + InvoiceData.CompanyAddress + "</h3> ";
            htmlcontent += "<h3> Legal Registration No : " + InvoiceData.LegalRegistrationNo + "</h3>";
            htmlcontent += "<h3> EmailAddress : "+ InvoiceData.EmailAddress +" </h3>";
            htmlcontent += "<div style='text-align:left'>";
            htmlcontent += "<p> ShippingName : "+ InvoiceData.ShippingName +" </p>";
            htmlcontent += "<p> ShippingAddress : "+ InvoiceData.ShippingAddress + " Nagar </p>";
            htmlcontent += "<p> ShippingPhoneNo : "+ InvoiceData.ShippingPhoneNo +" </p>";
            htmlcontent += "<p> ShippingTaxNo : "+ InvoiceData.ShippingTaxNo +" </p>";
            htmlcontent += "</div>";
            htmlcontent += "<div style='text-align:right'>";
            htmlcontent += "<p> BillingName : "+ InvoiceData.BillingName + " </p>";
            htmlcontent += "<p> BillingAddress : "+ InvoiceData.BillingAddress + " </p>";
            htmlcontent += "<p> BillingPhoneNo : "+ InvoiceData.BillingPhoneNo + " </p>";
            htmlcontent += "<p> BillingTaxNo : "+ InvoiceData.BillingTaxNo + " </p>";
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
            htmlcontent += "<td>" + InvoiceData.ProductName  + "</td>";
            htmlcontent += "<td>" + InvoiceData.ProductDetails + "</td>";
            htmlcontent += "<td>" + InvoiceData.Quantity + "</td>";
            htmlcontent += "<td>" + InvoiceData.Rate + "</td>";
            htmlcontent += "<td>" + InvoiceData.Amount + "</td>";
            htmlcontent += "</tr>";
            htmlcontent += "</tbody>";
            htmlcontent += "</table>";
            htmlcontent += "</div>";

            htmlcontent += "<div style='text-align:right'>";
            htmlcontent += "<h2>Summary Info</h2>";
            htmlcontent += "<h4> Total Amount : "+ InvoiceData.Subtotal + " </h4>";
            htmlcontent += "</div>";

            PdfGenerator.AddPdfPages(document, htmlcontent, PageSize.A4);
            byte[]? response = null;
            using (MemoryStream ms = new MemoryStream())
            {
                document.Save(ms);
                response = ms.ToArray();
            }
            string Filename = "Invoice_" + InvoiceData + ".pdf";
            return File(response, "application/pdf", Filename);
        }
    }
}
