using EMPManegment.EntityModels.ViewModels.ExpenseMaster;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using EMPManegment.Inretface.Interface.ProductMaster;
using EMPManegment.Inretface.Services.ExpenseMaster;
using EMPManegment.Inretface.Services.ProductMaster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EMPManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseMasterController : ControllerBase
    {
        private readonly IExpenseMasterServices expenseMaster;
        public ExpenseMasterController(IExpenseMasterServices ExpenseMaster)
        {
            expenseMaster = ExpenseMaster;
        }
        [HttpPost]
        [Route("AddExpenseDetails")]
        public async Task<IActionResult> AddExpenseDetails(ExpenseDetailsView AddExpense)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var result = expenseMaster.AddExpenseDetails(AddExpense);
                if (result.Result.Code == 200)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message = result.Result.Message;
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
        [HttpGet]
        [Route("GetExpenseDetailList")]
        public async Task<IActionResult> GetExpenseDetailList()
        {
            IEnumerable<ExpenseDetailsView> getExpense = await expenseMaster.GetExpenseDetailList();
            return Ok(new { code = 200, data = getExpense.ToList() });
        }
        [HttpGet]
        [Route("GetExpenseDetailById")]
        public async Task<IActionResult> GetExpenseDetailById(Guid Id)
        {
            var getExpense = await expenseMaster.GetExpenseDetailById(Id);
            return Ok(new { code = 200, data = getExpense });
        }
        [HttpPost]
        [Route("UpdateExpenseDetails")]
        public async Task<IActionResult> UpdateExpenseDetails(ExpenseDetailsView ExpenseDetail)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var result = expenseMaster.UpdateExpenseDetail(ExpenseDetail);
                if (result.Result.Code == 200)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message = result.Result.Message;
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
        [HttpGet]
        [Route("GetExpensetypeList")]
        public async Task<IActionResult> GetExpensetypeList()
        {
            IEnumerable<ExpenseTypeView> getExpense = await expenseMaster.GetExpensetypeList();
            return Ok(new { code = 200, data = getExpense.ToList() });
        }
        [HttpGet]
        [Route("GetPaymentTypeList")]
        public async Task<IActionResult> GetPaymentTypeList()
        {
            IEnumerable<PaymentTypeView> getExpense = await expenseMaster.GetpaymenttypeList();
            return Ok(new { code = 200, data = getExpense.ToList() });
        }
    }
}
