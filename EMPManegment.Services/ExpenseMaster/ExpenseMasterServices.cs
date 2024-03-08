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
    public class ExpenseMasterServices:IExpenseMasterServices
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

        public async Task<IEnumerable<ExpenseTypeView>> GetAllExpensType()
        {
            return await expenseMaster.GetAllExpensType();
        }

        public async Task<IEnumerable<PaymentTypeView>> GetAllPaymentType()
        {
            return await expenseMaster.GetAllPaymentType();
        }

        public async Task<ExpenseTypeView> GetExpenseById(int ExpenseId)
        {
            return await expenseMaster.GetExpenseById(ExpenseId);
        }

        public async Task<ExpenseDetailsView> GetExpenseDetailById(Guid Id)
        {
            return await expenseMaster.GetExpenseDetailById(Id);
        }

        public async Task<jsonData> GetExpenseDetailList(DataTableRequstModel dataTable)
        {
            return await expenseMaster.GetExpenseDetailList(dataTable);
        }

        public async Task<PaymentTypeView> GetPaymentById(int PaymentId)
        {
            return await expenseMaster.GetPaymentById(PaymentId);
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
