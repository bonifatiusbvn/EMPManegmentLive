using EMPManegment.EntityModels.ViewModels.AGGridModels;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.ExpenseMaster;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.Inretface.Interface.ExpenseMaster;
using EMPManegment.Inretface.Interface.ProductMaster;
using EMPManegment.Inretface.Services.ExpenseMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Services.ExpenseMaster
{
    public class ExpenseMasterServices : IExpenseMasterServices
    {
        private readonly IExpenseMaster expenseMaster;
        public ExpenseMasterServices(IExpenseMaster ExpenseMaster)
        {
            expenseMaster = ExpenseMaster;
        }

        public async Task<UserResponceModel> AddExpenseDetails(ExpenseDetailsView ExpenseDetails)
        {
            return await expenseMaster.AddExpenseDetails(ExpenseDetails);
        }

        public async Task<UserResponceModel> AddExpenseType(ExpenseTypeView AddExpense)
        {
            return await expenseMaster.AddExpenseType(AddExpense);
        }

        public async Task<UserResponceModel> AddPaymentType(PaymentTypeView AddPayment)
        {
            return await expenseMaster.AddPaymentType(AddPayment);
        }

        public async Task<UserResponceModel> ApprovedExpense(List<ApprovedExpense> InsertOrder)
        {
            return await expenseMaster.ApprovedExpense(InsertOrder);
        }

        public async Task<UserResponceModel> DeleteExpense(Guid Id)
        {
            return await expenseMaster.DeleteExpense(Id);
        }

        public async Task<IEnumerable<ExpenseTypeView>> GetAllExpensType()
        {
            return await expenseMaster.GetAllExpensType();
        }

        public async Task<IEnumerable<PaymentTypeView>> GetAllPaymentType(string? search)
        {
            return await expenseMaster.GetAllPaymentType(search);
        }

        public async Task<UserResponceModel> GetExpenseById(int ExpenseId)
        {
            return await expenseMaster.GetExpenseById(ExpenseId);
        }

        public async Task<UserResponceModel> GetExpenseDetailById(Guid Id)
        {
            return await expenseMaster.GetExpenseDetailById(Id);
        }

        public async Task<List<ExpenseDetailsView>> GetExpenseDetailByUserId(Guid UserId)
        {
            return await expenseMaster.GetExpenseDetailByUserId(UserId);
        }

        public async Task<AGGridResponseModel<ExpenseDetailsView>> GetExpenseDetailList(AGGridRequestModel ExpenseRequest)
        {
            return await expenseMaster.GetExpenseDetailList(ExpenseRequest);
        }

        public async Task<UserResponceModel> GetPaymentById(int PaymentId)
        {
            return await expenseMaster.GetPaymentById(PaymentId);
        }

        public async Task<AGGridResponseModel<ExpenseDetailsView>> GetUserExpenseList(AGGridRequestModel ExpenseRequest)
        {
            return await expenseMaster.GetUserExpenseList(ExpenseRequest);
        }

        public async Task<AGGridResponseModel<UserExpenseDetailsView>> GetUserList(AGGridRequestModel ExpenseRequest)
        {
            return await expenseMaster.GetUserList(ExpenseRequest);
        }

        public async Task<UserResponceModel> UpdateExpenseDetail(ExpenseDetailsView ExpenseDetails)
        {
            return await expenseMaster.UpdateExpenseDetail(ExpenseDetails);
        }

        public async Task<UserResponceModel> UpdateExpenseType(ExpenseTypeView UpdateExpense)
        {
            return await expenseMaster.UpdateExpenseType(UpdateExpense);
        }

        public async Task<UserResponceModel> UpdatePaymentType(PaymentTypeView UpdatePayment)
        {
            return await expenseMaster.UpdatePaymentType(UpdatePayment);
        }
    }
}
