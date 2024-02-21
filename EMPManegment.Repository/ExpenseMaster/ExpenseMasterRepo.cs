using EMPManagment.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels.ExpenseMaster;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using EMPManegment.Inretface.Interface.ExpenseMaster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Repository.ExpenseMaster
{
    public class ExpenseMasterRepo:IExpenseMaster
    {
        public ExpenseMasterRepo(BonifatiusEmployeesContext Context)
        {
            this.Context = Context;
        }
        public BonifatiusEmployeesContext Context { get; }

        public async Task<UserResponceModel> AddExpenseDetails(ExpenseDetailsView ExpenseDetails)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                    var expense = new TblExpenseMaster()
                    {
                        Id = Guid.NewGuid(),
                        UserId = ExpenseDetails.UserId,
                        ExpenseType = ExpenseDetails.ExpenseType,
                        PaymentType = ExpenseDetails.PaymentType,
                        BillNumber = ExpenseDetails.BillNumber,
                        Description = ExpenseDetails.Description,
                        Date= ExpenseDetails.Date,
                        TotalAmount = ExpenseDetails.TotalAmount,
                        Image= ExpenseDetails.Image,
                        Account= ExpenseDetails.Account,
                        IsPaid= ExpenseDetails.IsPaid,
                        IsApproved= ExpenseDetails.IsApproved,
                        ApprovedBy= ExpenseDetails.ApprovedBy,
                        ApprovedByName= ExpenseDetails.ApprovedByName,
                        CreatedOn = DateTime.Today,
                        CreatedBy= ExpenseDetails.CreatedBy,
                    };
                    response.Code = 200;
                    response.Message = "Expense add successfully!";
                    Context.TblExpenseMasters.Add(expense);
                    Context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return response;
        }

        public async Task<ExpenseDetailsView> GetExpenseDetailById(Guid Id)
        {
            var ExpenseDetail = await Context.TblExpenseMasters.SingleOrDefaultAsync(x => x.Id == Id);
            ExpenseDetailsView model = new ExpenseDetailsView
            {
                Id = ExpenseDetail.Id,
                UserId=ExpenseDetail.UserId,
                ExpenseType=ExpenseDetail.ExpenseType,
                PaymentType=ExpenseDetail.PaymentType,
                BillNumber=ExpenseDetail.BillNumber,
                Description=ExpenseDetail.Description,
                Date=ExpenseDetail.Date,
                TotalAmount=ExpenseDetail.TotalAmount,
                Image=ExpenseDetail.Image,
                Account=ExpenseDetail.Account,
                IsPaid=ExpenseDetail.IsPaid,
                IsApproved=ExpenseDetail.IsApproved,
                ApprovedBy=ExpenseDetail.ApprovedBy,
                ApprovedByName  =ExpenseDetail.ApprovedByName,
                CreatedBy=ExpenseDetail.CreatedBy,
                CreatedOn=ExpenseDetail.CreatedOn,
                
            };
            return model;
        }

        public async Task<IEnumerable<ExpenseDetailsView>> GetExpenseDetailList()
        {
            try
            {
                IEnumerable<ExpenseDetailsView> Expense = Context.TblExpenseMasters.ToList().Select(a => new ExpenseDetailsView
                {
                    Id = a.Id,
                    UserId = a.UserId,
                    ExpenseType=a.ExpenseType,
                    PaymentType=a.PaymentType,
                    BillNumber=a.BillNumber,
                    Description=a.Description,
                    Date=a.Date,
                    TotalAmount=a.TotalAmount,
                    Image=a.Image,
                    Account=a.Account,
                    IsPaid=a.IsPaid,
                    IsApproved=a.IsApproved,
                    ApprovedBy=a.ApprovedBy,
                    ApprovedByName=a.ApprovedByName,
                    CreatedBy=a.CreatedBy,
                    CreatedOn =a.CreatedOn,
                });
                return Expense;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<ExpenseTypeView>> GetExpensetypeList()
        {
            try
            {
                IEnumerable<ExpenseTypeView> Expense = Context.TblExpenseTypes.ToList().Select(a => new ExpenseTypeView
                {
                    Id = a.Id,
                    Type = a.Type,
                    CreatedOn = a.CreatedOn,
                });
                return Expense;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<PaymentTypeView>> GetpaymenttypeList()
        {
            try
            {
                IEnumerable<PaymentTypeView> Payment = Context.TblPaymentTypes.ToList().Select(a => new PaymentTypeView
                {
                    Id = a.Id,
                    Type = a.Type,
                    CreatedOn = a.CreatedOn,
                });
                return Payment;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserResponceModel> UpdateExpenseDetail(ExpenseDetailsView ExpenseDetails)
        {
            UserResponceModel model = new UserResponceModel();
            var GetExpenseDetail = Context.TblExpenseMasters.Where(e => e.Id == ExpenseDetails.Id).FirstOrDefault();
            try
            {
                if (GetExpenseDetail != null)
                {
                    GetExpenseDetail.Id = ExpenseDetails.Id;
                    GetExpenseDetail.UserId = ExpenseDetails.UserId;
                    GetExpenseDetail.ExpenseType = ExpenseDetails.ExpenseType;
                    GetExpenseDetail.PaymentType = ExpenseDetails.PaymentType;
                    GetExpenseDetail.BillNumber = ExpenseDetails.BillNumber;
                    GetExpenseDetail.Description = ExpenseDetails.Description;
                    GetExpenseDetail.Date=ExpenseDetails.Date;
                    GetExpenseDetail.TotalAmount = ExpenseDetails.TotalAmount;
                    GetExpenseDetail.Image=ExpenseDetails.Image;
                    GetExpenseDetail.Account=ExpenseDetails.Account;
                    GetExpenseDetail.IsPaid = ExpenseDetails.IsPaid;
                    GetExpenseDetail.IsApproved = ExpenseDetails.IsApproved;
                    GetExpenseDetail.ApprovedBy = ExpenseDetails.ApprovedBy;
                    GetExpenseDetail.ApprovedByName = ExpenseDetails.ApprovedByName;
                    GetExpenseDetail.CreatedBy = ExpenseDetails.CreatedBy;
                    GetExpenseDetail.CreatedOn = ExpenseDetails.CreatedOn;
                }
                Context.TblExpenseMasters.Update(GetExpenseDetail);
                Context.SaveChanges();
                model.Code = 200;
                model.Message = "Expense Details Updated Successfully!";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }
    }
}
