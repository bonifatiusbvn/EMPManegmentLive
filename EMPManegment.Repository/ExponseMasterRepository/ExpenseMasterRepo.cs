﻿using EMPManagment.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
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
    public class ExpenseMasterRepo : IExpenseMaster
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
                    CreatedOn = a.CreatedOn,
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
                Id = PaymentType.Id,
                Type = PaymentType.Type,
                CreatedOn = PaymentType.CreatedOn,
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
                    Date = ExpenseDetails.Date,
                    TotalAmount = ExpenseDetails.TotalAmount,
                    Image = ExpenseDetails.Image,
                    Account = ExpenseDetails.Account,
                    IsPaid = ExpenseDetails.IsPaid,
                    IsApproved = ExpenseDetails.IsApproved,
                    ApprovedBy = ExpenseDetails.ApprovedBy,
                    ApprovedByName = ExpenseDetails.ApprovedByName,
                    CreatedOn = DateTime.Today,
                    CreatedBy = ExpenseDetails.CreatedBy,
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
                UserId = ExpenseDetail.UserId,
                ExpenseType = ExpenseDetail.ExpenseType,
                PaymentType = ExpenseDetail.PaymentType,
                BillNumber = ExpenseDetail.BillNumber,
                Description = ExpenseDetail.Description,
                Date = ExpenseDetail.Date,
                TotalAmount = ExpenseDetail.TotalAmount,
                Image = ExpenseDetail.Image,
                Account = ExpenseDetail.Account,
                IsPaid = ExpenseDetail.IsPaid,
                IsApproved = ExpenseDetail.IsApproved,
                ApprovedBy = ExpenseDetail.ApprovedBy,
                ApprovedByName = ExpenseDetail.ApprovedByName,
                CreatedBy = ExpenseDetail.CreatedBy,
                CreatedOn = ExpenseDetail.CreatedOn,

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
                    ExpenseType = a.ExpenseType,
                    PaymentType = a.PaymentType,
                    BillNumber = a.BillNumber,
                    Description = a.Description,
                    Date = a.Date,
                    TotalAmount = a.TotalAmount,
                    Image = a.Image,
                    Account = a.Account,
                    IsPaid = a.IsPaid,
                    IsApproved = a.IsApproved,
                    ApprovedBy = a.ApprovedBy,
                    ApprovedByName = a.ApprovedByName,
                    CreatedBy = a.CreatedBy,
                    CreatedOn = a.CreatedOn,
                });
                return Expense;
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
                    GetExpenseDetail.ExpenseType = ExpenseDetails.ExpenseType;
                    GetExpenseDetail.PaymentType = ExpenseDetails.PaymentType;
                    GetExpenseDetail.BillNumber = ExpenseDetails.BillNumber;
                    GetExpenseDetail.Description = ExpenseDetails.Description;
                    GetExpenseDetail.Date = ExpenseDetails.Date;
                    GetExpenseDetail.TotalAmount = ExpenseDetails.TotalAmount;
                    GetExpenseDetail.Account = ExpenseDetails.Account;
                    GetExpenseDetail.IsPaid = ExpenseDetails.IsPaid;
                    GetExpenseDetail.IsApproved = ExpenseDetails.IsApproved;
                    GetExpenseDetail.ApprovedBy = ExpenseDetails.ApprovedBy;
                    GetExpenseDetail.ApprovedByName = ExpenseDetails.ApprovedByName;
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

        public async Task<jsonData> GetUserExpenseList(Guid UserId, DataTableRequstModel dataTable)
        {
            try
            {
                var expenses = Context.TblExpenseMasters
                    .Where(a => a.UserId == UserId)
                    .Select(a => new ExpenseDetailsView
                    {
                        Id = a.Id,
                        UserId = a.UserId,
                        ExpenseType = a.ExpenseType,
                        PaymentType = a.PaymentType,
                        BillNumber = a.BillNumber,
                        Description = a.Description,
                        Date = a.Date,
                        TotalAmount = a.TotalAmount,
                        Image = a.Image,
                        Account = a.Account,
                        IsPaid = a.IsPaid,
                        IsApproved = a.IsApproved,
                        ApprovedBy = a.ApprovedBy,
                        ApprovedByName = a.ApprovedByName,
                        CreatedBy = a.CreatedBy,
                        CreatedOn = a.CreatedOn
                    });


                if (!string.IsNullOrEmpty(dataTable.sortColumn) && !string.IsNullOrEmpty(dataTable.sortColumnDir))
                {
                    switch (dataTable.sortColumn)
                    {
                        case "BillNumber":
                            expenses = dataTable.sortColumnDir == "asc" ? expenses.OrderBy(e => e.BillNumber) : expenses.OrderByDescending(e => e.BillNumber);
                            break;
                        case "Date":
                            expenses = dataTable.sortColumnDir == "asc" ? expenses.OrderBy(e => e.Date) : expenses.OrderByDescending(e => e.Date);
                            break;
                        case "Account":
                            expenses = dataTable.sortColumnDir == "asc" ? expenses.OrderBy(e => e.Account) : expenses.OrderByDescending(e => e.Account);
                            break;
                        case "TotalAmount":
                            expenses = dataTable.sortColumnDir == "asc" ? expenses.OrderBy(e => e.TotalAmount) : expenses.OrderByDescending(e => e.TotalAmount);
                            break;
                        default:

                            break;
                    }
                }
                if (!string.IsNullOrEmpty(dataTable.searchValue))
                {
                    string searchLower = dataTable.searchValue.ToLower();
                    expenses = expenses.Where(e =>
                        e.BillNumber.ToLower().Contains(searchLower) ||
                        e.Date.ToString().Contains(searchLower) ||
                        e.Account.ToLower().Contains(searchLower) ||
                        e.TotalAmount.ToString().Contains(dataTable.searchValue));
                }

                int totalRecord = await expenses.CountAsync();

                var cData = await expenses.Skip(dataTable.skip).Take(dataTable.pageSize).ToListAsync();

                jsonData jsonData = new jsonData
                {
                    draw = dataTable.draw,
                    recordsFiltered = totalRecord,
                    recordsTotal = totalRecord,
                    data = cData
                };
                return jsonData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<jsonData> GetUserList(DataTableRequstModel dataTable)
        {
            try
            {
                var UserList = from a in Context.TblExpenseMasters
                               join b in Context.TblUsers on a.UserId equals b.Id
                               where a.Account == "Credit"
                               group a by new { a.UserId, b.Image, b.UserName, FullName = b.FirstName + " " + b.LastName } into userGroup
                               select new UserExpenseDetailsView
                               {
                                   FullName = userGroup.Key.FullName,
                                   Image = userGroup.Key.Image,
                                   UserName = userGroup.Key.UserName,
                                   Date = userGroup.Max(e => e.Date),
                                   TotalAmount = userGroup.Sum(e => e.TotalAmount)
                               };

                if (!string.IsNullOrEmpty(dataTable.sortColumn) && !string.IsNullOrEmpty(dataTable.sortColumnDir))
                {
                    switch (dataTable.sortColumn)
                    {
                        case "UserName":
                            UserList = dataTable.sortColumnDir == "asc" ? UserList.OrderBy(e => e.UserName) : UserList.OrderByDescending(e => e.UserName);
                            break;
                        case "Date":
                            UserList = dataTable.sortColumnDir == "asc" ? UserList.OrderBy(e => e.Date) : UserList.OrderByDescending(e => e.Date);
                            break;
                        case "FullName":
                            UserList = dataTable.sortColumnDir == "asc" ? UserList.OrderBy(e => e.FullName) : UserList.OrderByDescending(e => e.FullName);
                            break;
                        case "TotalAmount":
                            UserList = dataTable.sortColumnDir == "asc" ? UserList.OrderBy(e => e.TotalAmount) : UserList.OrderByDescending(e => e.TotalAmount);
                            break;
                        default:
                            break;
                    }
                }

                if (!string.IsNullOrEmpty(dataTable.searchValue))
                {
                    string searchLower = dataTable.searchValue.ToLower();
                    UserList = UserList.Where(e =>
                        e.UserName.ToLower().Contains(searchLower) ||
                        e.Date.ToString().Contains(searchLower) ||
                        e.FullName.ToLower().Contains(searchLower) ||
                        e.TotalAmount.ToString().Contains(dataTable.searchValue));
                }

                int totalRecord = await UserList.CountAsync();

                var cData = await UserList.Skip(dataTable.skip).Take(dataTable.pageSize).ToListAsync();

                jsonData jsonData = new jsonData
                {
                    draw = dataTable.draw,
                    recordsFiltered = totalRecord,
                    recordsTotal = totalRecord,
                    data = cData
                };
                return jsonData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


    }
}
