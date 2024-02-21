using EMPManegment.EntityModels.ViewModels.ExpenseMaster;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.Interface.ExpenseMaster
{
    public interface IExpenseMaster
    {
        Task<UserResponceModel> AddExpenseDetails(ExpenseDetailsView ExpenseDetails);
        Task<IEnumerable<ExpenseDetailsView>> GetExpenseDetailList();
        Task<ExpenseDetailsView> GetExpenseDetailById(Guid Id);
        Task<UserResponceModel> UpdateExpenseDetail(ExpenseDetailsView ExpenseDetails);
    }
}
