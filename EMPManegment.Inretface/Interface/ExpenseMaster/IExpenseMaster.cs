using EMPManegment.EntityModels.ViewModels.AGGridModels;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.ExpenseMaster;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.Interface.ExpenseMaster
{
    public interface IExpenseMaster
    {
        Task<UserResponceModel> AddPaymentType(PaymentTypeView AddPayment);

        Task<IEnumerable<PaymentTypeView>> GetAllPaymentType(string? search);

        Task<UserResponceModel> GetPaymentById(int PaymentId);

        Task<UserResponceModel> UpdatePaymentType(PaymentTypeView UpdatePayment);

        Task<UserResponceModel> AddExpenseType(ExpenseTypeView AddExpense);

        Task<IEnumerable<ExpenseTypeView>> GetAllExpensType();

        Task<UserResponceModel> GetExpenseById(int ExpenseId);

        Task<UserResponceModel> UpdateExpenseType(ExpenseTypeView UpdateExpense);

        Task<UserResponceModel> AddExpenseDetails(ExpenseDetailsView ExpenseDetails);

        Task<AGGridResponseModel<ExpenseDetailsView>> GetUserExpenseList(AGGridRequestModel ExpenseRequest);

        Task<jsonData> GetUserList(DataTableRequstModel dataTable);

        Task<jsonData> GetExpenseDetailList(DataTableRequstModel dataTable, bool? unapprove = null, DateTime? TodayDate = null);

        Task<UserResponceModel> GetExpenseDetailById(Guid Id);

        Task<UserResponceModel> UpdateExpenseDetail(ExpenseDetailsView ExpenseDetails);

        Task<List<ExpenseDetailsView>> GetExpenseDetailByUserId(Guid UserId);

        Task<UserResponceModel> ApprovedExpense(List<ApprovedExpense> InsertOrder);

        Task<UserResponceModel> DeleteExpense(Guid Id);
    }
}
