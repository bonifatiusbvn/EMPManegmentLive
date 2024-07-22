using EMPManegment.EntityModels.ViewModels.DataTableParameters;
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
        Task<UserResponceModel> AddPaymentType(PaymentTypeView AddPayment);

        Task<IEnumerable<PaymentTypeView>> GetAllPaymentType();

        Task<UserResponceModel> GetPaymentById(int PaymentId);

        Task<UserResponceModel> UpdatePaymentType(PaymentTypeView UpdatePayment);

        Task<UserResponceModel> AddExpenseType(ExpenseTypeView AddExpense);

        Task<IEnumerable<ExpenseTypeView>> GetAllExpensType();

        Task<UserResponceModel> GetExpenseById(int ExpenseId);

        Task<UserResponceModel> UpdateExpenseType(ExpenseTypeView UpdateExpense);

        Task<UserResponceModel> AddExpenseDetails(ExpenseDetailsView ExpenseDetails);

        Task<jsonData> GetUserExpenseList(Guid UserId, DataTableRequstModel dataTable, string filterType = null, bool? unapprove = null, bool? approve = null, DateTime? startDate = null, DateTime? endDate = null, string account = null, string selectMonthlyExpense = null);

        Task<jsonData> GetUserList(DataTableRequstModel dataTable);

        Task<jsonData> GetExpenseDetailList(DataTableRequstModel dataTable, bool? unapprove = null, DateTime? TodayDate = null);

        Task<UserResponceModel> GetExpenseDetailById(Guid Id);

        Task<UserResponceModel> UpdateExpenseDetail(ExpenseDetailsView ExpenseDetails);

        Task<List<ExpenseDetailsView>> GetExpenseDetailByUserId(Guid UserId);

        Task<UserResponceModel> ApprovedExpense(List<ApprovedExpense> InsertOrder);

        Task<UserResponceModel> DeleteExpense(Guid Id);
    }
}
