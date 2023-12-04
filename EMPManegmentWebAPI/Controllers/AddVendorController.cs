using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.VendorModels;
using EMPManegment.Inretface.Interface.VendorDetails;
using EMPManegment.Inretface.Services.VendorDetailsServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EMPManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddVendorController : ControllerBase
    {
        public IAddVendorDetailsServices VendorDetails { get; }
        public AddVendorController(IAddVendorDetailsServices VendorDetails)
        {
            this.VendorDetails = VendorDetails;
        }

        [HttpPost]
        [Route("AddVendors")]
        public async Task<IActionResult> AddVendors(VendorDetailsView AddVendor)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var addVendor = VendorDetails.AddVendor(AddVendor);
                if (addVendor.Result.Code == 200)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message = addVendor.Result.Message;
                }
                else
                {
                    response.Message = addVendor.Result.Message;
                    response.Code = (int)HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return StatusCode(response.Code, response);
        }

        [HttpPost]
        [Route("GetVendorList")]
        public async Task<IActionResult> GetVendorList(DataTableRequstModel VendorList)
        {
            var GetvendorList = await VendorDetails.GetVendorsList(VendorList);
            return Ok(new { code = 200, data = GetvendorList });
        }

        [HttpGet]
        [Route("GetVendorType")]
        public async Task<IActionResult> GetVendorType()
        {
            IEnumerable<VendorTypeView> VendorType = await VendorDetails.GetVendorType();
            return Ok(new { code = 200, data = VendorType.ToList() });
        }

        [HttpGet]
        [Route("GetVendorDetailsById")]
        public async Task<IActionResult> GetVendorDetailsById(Guid vendorId)
        {
            var vendor = await VendorDetails.GetVendorById(vendorId);
            return Ok(new { code = 200, data = vendor });
        }
    }
}
