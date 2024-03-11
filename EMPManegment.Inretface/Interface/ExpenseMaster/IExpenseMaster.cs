﻿using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.ExpenseMaster;
using EMPManegment.EntityModels.ViewModels.Models;
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

        Task<IEnumerable<PaymentTypeView>> GetAllPaymentType();

        Task<PaymentTypeView> GetPaymentById(int PaymentId);

        Task<UserResponceModel> UpdatePaymentType(PaymentTypeView UpdatePayment);

        Task<UserResponceModel> AddExpenseType(ExpenseTypeView AddExpense);

        Task<IEnumerable<ExpenseTypeView>> GetAllExpensType();

        Task<ExpenseTypeView> GetExpenseById(int ExpenseId);

        Task<UserResponceModel> UpdateExpenseType(ExpenseTypeView UpdateExpense);

        Task<UserResponceModel> AddExpenseDetails(ExpenseDetailsView ExpenseDetails);
        Task<jsonData> GetUserExpenseList(Guid UserId, DataTableRequstModel dataTable);
        Task<jsonData> GetUserList(DataTableRequstModel dataTable);
        Task<jsonData> GetExpenseDetailList(DataTableRequstModel dataTable);
        Task<ExpenseDetailsView> GetExpenseDetailById(Guid Id);
        Task<UserResponceModel> UpdateExpenseDetail(ExpenseDetailsView ExpenseDetails);
        Task<List<ExpenseDetailsView>> GetExpenseDetailByUserId(Guid UserId);
    }
}
