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
    public class InvoiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CreateInvoice()
        {
            return View();
        }

        public IActionResult EditInvoice()
        {
            return View();
        }


    }
}
