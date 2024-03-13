﻿using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.ExpenseMaster;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using EMPManegment.Inretface.Interface.InvoiceMaster;
using EMPManegment.Inretface.Interface.OrderDetails;
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
        [HttpPost]
        [Route("GetExpenseDetailList")]
        public async Task<IActionResult> GetExpenseDetailList(DataTableRequstModel DataTable)
        {
            var getExpense = await expenseMaster.GetExpenseDetailList(DataTable);
            return Ok(new { code = 200, data = getExpense });
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
        [Route("GetAllExpensType")]
        public async Task<IActionResult> GetAllExpensType()
        {
            IEnumerable<ExpenseTypeView> getExpense = await expenseMaster.GetAllExpensType();
            return Ok(new { code = 200, data = getExpense.ToList() });
        }
        [HttpGet]
        [Route("GetAllPaymentType")]
        public async Task<IActionResult> GetAllPaymentType()
        {
            IEnumerable<PaymentTypeView> getExpense = await expenseMaster.GetAllPaymentType();
            return Ok(new { code = 200, data = getExpense.ToList() });
        }

        [HttpPost]
        [Route("GetUserExpenseDetail")]
        public async Task<IActionResult> GetUserExpenseDetail(Guid UserId, DataTableRequstModel dataTable)
        {
            var getUserExpense = await expenseMaster.GetUserExpenseList(UserId, dataTable);
            return Ok(new { code = 200, data = getUserExpense });
        }
        [HttpPost]
        [Route("GetUserList")]
        public async Task<IActionResult> GetUserList(DataTableRequstModel dataTable)
        {
            var getUserExpense = await expenseMaster.GetUserList(dataTable);
            return Ok(new { code = 200, data = getUserExpense });
        }
        [HttpPost]
        [Route("GetExpenseDetailByUserId")]
        public async Task<IActionResult> GetExpenseDetailByUserId(Guid UserId)
        {
            List<ExpenseDetailsView> getExpense = await expenseMaster.GetExpenseDetailByUserId(UserId);
            return Ok(new { code = 200, data = getExpense.ToList() });
        }

        [HttpPost]
        [Route("GetAllUserExpenseDetail")]
        public async Task<IActionResult> GetAllUserExpenseDetail(Guid UserId, DataTableRequstModel dataTable)
        {
            var getallUserExpense = await expenseMaster.GetAllUserExpenseList(UserId, dataTable);
            return Ok(new { code = 200, data = getallUserExpense });
        }

        [HttpPost]
        [Route("ApprovedExpense")]
        public async Task<IActionResult> ApprovedExpense(List<ApprovedExpense> ApprovedallExpense)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var result = await expenseMaster.ApprovedExpense(ApprovedallExpense);

                if (result.Code == 200)
                {
                    response.Code = (int)HttpStatusCode.OK;
                    response.Message = result.Message;
                }
                else
                {
                    response.Message = result.Message;
                    response.Code = (int)HttpStatusCode.NotFound;
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
                if (expense != null)
                {
                    responseModel.Code = (int)HttpStatusCode.OK;
                    responseModel.Message = expense.Message;
                }
                else
                {
                    responseModel.Message = expense.Message;
                    responseModel.Code = (int)HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                responseModel.Code = (int)HttpStatusCode.InternalServerError;
            }
            return StatusCode(responseModel.Code, responseModel);
        }
    }
}
