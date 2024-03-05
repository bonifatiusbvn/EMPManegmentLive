using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using EMPManegment.EntityModels.ViewModels.VendorModels;
using EMPManegment.Inretface.Interface.ProductMaster;
using EMPManegment.Inretface.Interface.UserAttendance;
using EMPManegment.Inretface.Interface.VendorDetails;
using EMPManegment.Inretface.Services.VendorDetailsServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EMPManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VendorController : ControllerBase
    {
        private readonly IAddVendorDetailsServices vendorServices;

        public VendorController(IAddVendorDetailsServices VendorServices)
        {
            vendorServices = VendorServices;
        }

        [HttpPost]
        [Route("CreateVendors")]
        public async Task<IActionResult> CreateVendors(VendorDetailsView AddVendor)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var addVendor = vendorServices.AddVendor(AddVendor);
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
            var GetvendorList = await vendorServices.GetVendorsList(VendorList);
            return Ok(new { code = 200, data = GetvendorList });
        }

        [HttpGet]
        [Route("GetVendorType")]
        public async Task<IActionResult> GetVendorType()
        {
            IEnumerable<VendorTypeView> VendorType = await vendorServices.GetVendorType();
            return Ok(new { code = 200, data = VendorType.ToList() });
        }

        [HttpGet]
        [Route("GetVendorDetailsById")]
        public async Task<IActionResult> GetVendorDetailsById(Guid vendorId)
        {
            var vendor = await vendorServices.GetVendorById(vendorId);
            return Ok(new { code = 200, data = vendor });
        }

        [HttpGet]
        [Route("GetVendorsNameList")]
        public async Task<IActionResult> GetVendorNameList()
        {
            var getVendorsNameList = await vendorServices.GetVendorNameList();
            return Ok(new { code = 200, data = getVendorsNameList.ToList() });
        }

        [HttpPost]
        [Route("UpdateVendorDetails")]
        public async Task<IActionResult> UpdateVendorDetails(VendorDetailsView updateVendor)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var vendordetails = vendorServices.UpdateVendorDetails(updateVendor);
                if (vendordetails.Result.Code == 200)
                {
                    response.Code = 200;
                    response.Message = vendordetails.Result.Message;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return StatusCode(response.Code, response);
        }
    }
}
