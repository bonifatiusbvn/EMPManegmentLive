using EMPManagment.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels.ExpenseMaster;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using EMPManegment.Inretface.Interface.ExpenseMaster;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Repository.ExponseMasterRepository
{
    public class ExpenseMasterRepo:IExpenseMaster
    {
        public ExpenseMasterRepo(BonifatiusEmployeesContext Context)
        {
            this.Context = Context;
        }
        public BonifatiusEmployeesContext Context { get; }

        public async Task<UserResponceModel> AddExpenseType(ExpenseTypeView AddExpense)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var Expense = new TblExpenseType()
                {
                    Type = AddExpense.Type,
                    CreatedOn = DateTime.Now,
                };
                response.Code = (int)HttpStatusCode.OK;
                response.Message = "ExpenseType Successfully Inserted";
                response.Icone = "success";
                Context.TblExpenseTypes.Add(Expense);
                Context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
            return response;
        }

        public async Task<UserResponceModel> AddPaymentType(PaymentTypeView AddPayment)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var Payment = new TblPaymentType()
                {
                    Type = AddPayment.Type,
                    CreatedOn = DateTime.Now,
                };
                response.Code = (int)HttpStatusCode.OK;
                response.Message = "PaymentType Successfully Inserted";
                response.Icone = "success";
                Context.TblPaymentTypes.Add(Payment);
                Context.SaveChanges();
            }
            catch (Exception)
            {

                throw;
            }
            return response;
        }

        public async Task<IEnumerable<ExpenseTypeView>> GetAllExpensType()
        {
            try
            {
                IEnumerable<ExpenseTypeView> ExpenseType = Context.TblExpenseTypes.ToList().Select(a => new ExpenseTypeView
                {
                    Id = a.Id,
                    Type = a.Type,
                    CreatedOn = a.CreatedOn,
                });
                return ExpenseType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<PaymentTypeView>> GetAllPaymentType()
        {
            try
            {
                IEnumerable<PaymentTypeView> paymentType = Context.TblPaymentTypes.ToList().Select(a => new PaymentTypeView
                {
                    Id = a.Id,
                    Type = a.Type,
                    CreatedOn=a.CreatedOn,
                });
                return paymentType;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ExpenseTypeView> GetExpenseById(int ExpenseId)
        {
            var ExpenseType = await Context.TblExpenseTypes.SingleOrDefaultAsync(x => x.Id == ExpenseId);
            ExpenseTypeView model = new ExpenseTypeView
            {
                Id = ExpenseType.Id,
                Type = ExpenseType.Type,
                CreatedOn = ExpenseType.CreatedOn,
            };
            return model;
        }

        public async Task<PaymentTypeView> GetPaymentById(int PaymentId)
        {
            var PaymentType = await Context.TblPaymentTypes.SingleOrDefaultAsync(x => x.Id == PaymentId);
            PaymentTypeView model = new PaymentTypeView
            {
               Id= PaymentType.Id,
               Type = PaymentType.Type,
               CreatedOn=PaymentType.CreatedOn,
            };
            return model;
        }

        public async Task<UserResponceModel> UpdateExpenseType(ExpenseTypeView UpdateExpense)
        {
            UserResponceModel model = new UserResponceModel();
            var ExpenseType = Context.TblExpenseTypes.Where(e => e.Id == UpdateExpense.Id).FirstOrDefault();
            try
            {
                if (ExpenseType != null)
                {
                    ExpenseType.Id = UpdateExpense.Id;
                    ExpenseType.Type = UpdateExpense.Type;
                    ExpenseType.CreatedOn = UpdateExpense.CreatedOn;
                }
                Context.TblExpenseTypes.Update(ExpenseType);
                Context.SaveChanges();
                model.Code = 200;
                model.Message = "ExpenseType Updated Successfully!";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }

        public async Task<UserResponceModel> UpdatePaymentType(PaymentTypeView UpdatePayment)
        {
            UserResponceModel model = new UserResponceModel();
            var paymentType = Context.TblPaymentTypes.Where(e => e.Id == UpdatePayment.Id).FirstOrDefault();
            try
            {
                if (paymentType != null)
                {
                    paymentType.Id = UpdatePayment.Id;
                    paymentType.Type = UpdatePayment.Type;
                    paymentType.CreatedOn = UpdatePayment.CreatedOn;
                }
                Context.TblPaymentTypes.Update(paymentType);
                Context.SaveChanges();
                model.Code = 200;
                model.Message = "PaymentType Updated Successfully!";
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return model;
        }
    }
}
