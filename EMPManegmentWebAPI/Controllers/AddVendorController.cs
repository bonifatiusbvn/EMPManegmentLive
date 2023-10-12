using EMPManegment.EntityModels.View_Model;
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
        public async Task<IActionResult> AddVendors(VendorDetailsView vendor)
        {
            VendorDetailsResponseModel response = new VendorDetailsResponseModel();
            try
            {
                var result = VendorDetails.AddVendor(vendor);
                if (result.Result.Code == 200)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Data = result.Result.Data;
                }
                else
                {
                    response.Message = result.Result.Message;
                    response.Code = (int)HttpStatusCode.NotFound;
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
