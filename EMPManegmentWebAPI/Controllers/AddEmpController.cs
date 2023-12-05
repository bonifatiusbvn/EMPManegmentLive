using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.Inretface.EmployeesInterface.AddEmployee;
using EMPManegment.Inretface.Services.AddEmployeeServies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EMPManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddEmpController : ControllerBase
    {
        public IAuthenticationServices Authentication { get; }

        public AddEmpController(IAuthenticationServices authentication)
        {
            Authentication = authentication;
        }

       

        [HttpGet]
        [Route("CheckUser")]
        public IActionResult CheckUser()
        {
            var checkUser = Authentication.CheckEmloyess();
            return Ok(new { code = 200, data = checkUser });
        }

        [HttpPost]
        [Route("AddEmployees")]
        public async Task<IActionResult> AddEmployees(EmpDetailsView AddEmployee)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var addEmployee = Authentication.UserSingUp(AddEmployee);
                if (addEmployee.Result.Code == 200)
                {
                    response.Code = (int)HttpStatusCode.OK;
                  
                }
                else
                {
                    response.Message = addEmployee.Result.Message;
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
