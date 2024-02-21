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

        public async Task<ExpenseDetailsView> GetExpenseDetailById(Guid Id)
        {
            return await expenseMaster.GetExpenseDetailById(Id);
        }

        public async Task<IEnumerable<ExpenseDetailsView>> GetExpenseDetailList()
        {
            return await expenseMaster.GetExpenseDetailList();
        }

        public async Task<IEnumerable<ExpenseTypeView>> GetExpensetypeList()
        {
            return await expenseMaster.GetExpensetypeList();
        }

        public async Task<IEnumerable<PaymentTypeView>> GetpaymenttypeList()
        {
            return await expenseMaster.GetpaymenttypeList();
        }

        public async Task<UserResponceModel> UpdateExpenseDetail(ExpenseDetailsView ExpenseDetails)
        {
            return await expenseMaster.UpdateExpenseDetail(ExpenseDetails);
        }
    }
}
