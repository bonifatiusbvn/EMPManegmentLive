using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.ExpenseMaster;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using EMPManegment.Inretface.Interface.ExpenseMaster;
using EMPManegment.Inretface.Interface.InvoiceMaster;
using EMPManegment.Inretface.Interface.OrderDetails;
using EMPManegment.Inretface.Interface.ProductMaster;
using EMPManegment.Inretface.Services.ExpenseMaster;
using EMPManegment.Inretface.Services.ProductMaster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace EMPManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
                if (result.Result.Code != (int)HttpStatusCode.InternalServerError)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message = result.Result.Message;
                }
                else
                {
                    response.Message = result.Result.Message;
                    response.Code = result.Result.Code;
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "An error occurred while processing the request.";
            }
            return StatusCode(response.Code, response);
        }

        [HttpPost]
        [Route("GetExpenseDetailList")]
        public async Task<IActionResult> GetExpenseDetailList(DataTableRequstModel DataTable)
        {
            try
            {
                var getExpense = await expenseMaster.GetExpenseDetailList(DataTable);
                return Ok(new { code = (int)HttpStatusCode.OK, data = getExpense });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { code = (int)HttpStatusCode.InternalServerError, message = "An error occurred while processing the request." });
            }
        }

        [HttpGet]
        [Route("GetExpenseDetailById")]
        public async Task<IActionResult> GetExpenseDetailById(Guid Id)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var getExpense = await expenseMaster.GetExpenseDetailById(Id);
                if (getExpense.Data != null)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Data = getExpense.Data;
                }
                else
                {
                    response.Message = getExpense.Message;
                    response.Code = getExpense.Code;
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "An error occurred while processing the request.";
            }
            return StatusCode(response.Code, response);
        }

        [HttpPost]
        [Route("UpdateExpenseDetails")]
        public async Task<IActionResult> UpdateExpenseDetails(ExpenseDetailsView ExpenseDetail)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var result = expenseMaster.UpdateExpenseDetail(ExpenseDetail);
                if (result.Result.Code != (int)HttpStatusCode.NotFound && result.Result.Code != (int)HttpStatusCode.InternalServerError)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message = result.Result.Message;
                }
                else
                {
                    response.Message = result.Result.Message;
                    response.Code = result.Result.Code;
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "An error occurred while processing the request.";
            }
            return StatusCode(response.Code, response);
        }

        [HttpGet]
        [Route("GetAllExpensType")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllExpensType()
        {
            try
            {
                IEnumerable<ExpenseTypeView> getExpense = await expenseMaster.GetAllExpensType();
                return Ok(new { code = (int)HttpStatusCode.OK, data = getExpense.ToList() });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { code = (int)HttpStatusCode.InternalServerError, message = "An error occurred while processing the request." });
            }
        }

        [HttpGet]
        [Route("GetAllPaymentType")]
        public async Task<IActionResult> GetAllPaymentType()
        {
            try
            {
                IEnumerable<PaymentTypeView> getExpense = await expenseMaster.GetAllPaymentType();
                return Ok(new { code = (int)HttpStatusCode.OK, data = getExpense.ToList() });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { code = (int)HttpStatusCode.InternalServerError, message = "An error occurred while processing the request." });
            }
        }

        [HttpPost]
        [Route("GetUserExpenseDetail")]
        public async Task<IActionResult> GetUserExpenseDetail(Guid UserId, DataTableRequstModel dataTable)
        {
            try
            {
                var getUserExpense = await expenseMaster.GetUserExpenseList(UserId, dataTable);
                return Ok(new { code = (int)HttpStatusCode.OK, data = getUserExpense });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { code = (int)HttpStatusCode.InternalServerError, message = "An error occurred while processing the request." });
            }
        }

        [HttpPost]
        [Route("GetUserList")]
        public async Task<IActionResult> GetUserList(DataTableRequstModel dataTable)
        {
            try
            {
                var getUserExpense = await expenseMaster.GetUserList(dataTable);
                return Ok(new { code = (int)HttpStatusCode.OK, data = getUserExpense });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { code = (int)HttpStatusCode.InternalServerError, message = "An error occurred while processing the request." });
            }
        }

        [HttpPost]
        [Route("GetExpenseDetailByUserId")]
        public async Task<IActionResult> GetExpenseDetailByUserId(Guid UserId)
        {
            try
            {
                List<ExpenseDetailsView> getExpense = await expenseMaster.GetExpenseDetailByUserId(UserId);
                return Ok(new { code = (int)HttpStatusCode.OK, data = getExpense.ToList() });
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, new { code = (int)HttpStatusCode.InternalServerError, message = "An error occurred while processing the request." });
            }
        }

        [HttpPost]
        [Route("ApprovedExpense")]
        public async Task<IActionResult> ApprovedExpense(List<ApprovedExpense> ApprovedallExpense)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var result = await expenseMaster.ApprovedExpense(ApprovedallExpense);

                if (result.Code != (int)HttpStatusCode.NotFound && result.Code != (int)HttpStatusCode.InternalServerError)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message = result.Message;
                }
                else
                {
                    response.Message = result.Message;
                    response.Code = result.Code;
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "An error occurred while processing the request.";

            }

            return StatusCode(response.Code, response);
        }

        [HttpPost]
        [Route("DeleteExpense")]
        public async Task<IActionResult> DeleteExpense(Guid Id)
        {
            UserResponceModel responseModel = new UserResponceModel();
            var expense = await expenseMaster.DeleteExpense(Id);
            try
            {
                if (expense.Code != (int)HttpStatusCode.NotFound && expense.Code != (int)HttpStatusCode.InternalServerError)
                {
                    responseModel.Code = (int)HttpStatusCode.OK;
                    responseModel.Message = expense.Message;
                }
                else
                {
                    responseModel.Message = expense.Message;
                    responseModel.Code = expense.Code;
                }
            }
            catch (Exception ex)
            {
                responseModel.Code = (int)HttpStatusCode.InternalServerError;
                responseModel.Message = "An error occurred while processing the request.";
            }
            return StatusCode(responseModel.Code, responseModel);
        }

        [HttpPost]
        [Route("AddExpenseType")]
        public async Task<IActionResult> AddExpenseType(ExpenseTypeView ExpenseDetails)
        {
            UserResponceModel responseModel = new UserResponceModel();
            var expense = await expenseMaster.AddExpenseType(ExpenseDetails);
            try
            {
                if (expense.Code != (int)HttpStatusCode.NotFound && expense.Code != (int)HttpStatusCode.InternalServerError)
                {
                    responseModel.Code = expense.Code;
                    responseModel.Message = expense.Message;
                }
                else
                {
                    responseModel.Message = expense.Message;
                    responseModel.Code = expense.Code;
                }
            }
            catch (Exception ex)
            {
                responseModel.Code = (int)HttpStatusCode.InternalServerError;
                responseModel.Message = "An error occurred while processing the request.";
            }
            return StatusCode(responseModel.Code, responseModel);
        }
    }
}
