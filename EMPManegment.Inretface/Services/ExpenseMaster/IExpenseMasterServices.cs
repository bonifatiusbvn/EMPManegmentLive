using EMPManegment.EntityModels.ViewModels.ExpenseMaster;
using EMPManegment.EntityModels.ViewModels.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.Services.ExpenseMaster
{
    public interface IExpenseMasterServices
    {
        Task<UserResponceModel> AddExpenseDetails(ExpenseDetailsView ExpenseDetails);
        Task<IEnumerable<ExpenseDetailsView>> GetExpenseDetailList();
        Task<ExpenseDetailsView> GetExpenseDetailById(Guid Id);
        Task<UserResponceModel> UpdateExpenseDetail(ExpenseDetailsView ExpenseDetails);
        Task<IEnumerable<ExpenseTypeView>> GetExpensetypeList();
        Task<IEnumerable<PaymentTypeView>> GetpaymenttypeList();
    }
}
