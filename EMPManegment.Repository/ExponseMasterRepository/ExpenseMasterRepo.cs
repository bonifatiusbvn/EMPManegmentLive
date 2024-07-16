using EMPManagment.API;
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
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using EMPManegment.EntityModels.ViewModels.OrderModels;
using EMPManegment.EntityModels.ViewModels.Invoice;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using System.Globalization;
using Azure;

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
                bool isExpenseTypeAlreadyExists = Context.TblExpenseTypes.Any(x => x.Type == AddExpense.Type);
                if (isExpenseTypeAlreadyExists == true)
                {
                    response.Message = "Expense type already exists";
                    response.Code = (int)HttpStatusCode.NotFound;
                }
                else
                {

                    var Expense = new TblExpenseType()
                    {
                        Type = AddExpense.Type,
                        CreatedOn = DateTime.Now,
                    };

                    response.Message = "Expense type successfully inserted";
                    Context.TblExpenseTypes.Add(Expense);
                    Context.SaveChanges();
                }
            }
            catch (Exception)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "Error in creating expense type";
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
                response.Message = "PaymentType successfully inserted";
                Context.TblPaymentTypes.Add(Payment);
                Context.SaveChanges();
            }
            catch (Exception)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "Error in creating payment type";
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

        public async Task<UserResponceModel> GetExpenseById(int ExpenseId)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var ExpenseType = await Context.TblExpenseTypes.SingleOrDefaultAsync(x => x.Id == ExpenseId);
                ExpenseTypeView model = new ExpenseTypeView
                {
                    Id = ExpenseType.Id,
                    Type = ExpenseType.Type,
                    CreatedOn = ExpenseType.CreatedOn,
                };

                response.Data = model;
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "Error in getting expense by id";
            }
            return response;
        }

        public async Task<UserResponceModel> GetPaymentById(int PaymentId)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {

                var PaymentType = await Context.TblPaymentTypes.SingleOrDefaultAsync(x => x.Id == PaymentId);
                PaymentTypeView model = new PaymentTypeView
                {
                    Id = PaymentType.Id,
                    Type = PaymentType.Type,
                };

                response.Data = model;
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "Error in getting payment by id";
            }
            return response;
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
                model.Message = "ExpenseType updated successfully!";
            }
            catch (Exception ex)
            {
                model.Code = (int)HttpStatusCode.InternalServerError;
                model.Message = "Error in updating expense type";
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

                model.Message = "PaymentType updated successfully!";
            }
            catch (Exception ex)
            {
                model.Code = (int)HttpStatusCode.InternalServerError;
                model.Message = "Error in updating payment type";
            }
            return model;
        }

        public async Task<UserResponceModel> AddExpenseDetails(ExpenseDetailsView ExpenseDetails)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                TblExpenseMaster expense = new TblExpenseMaster
                {
                    Id = Guid.NewGuid(),
                    UserId = ExpenseDetails.UserId,
                    ExpenseType = ExpenseDetails.ExpenseType,
                    TotalAmount = ExpenseDetails.TotalAmount,
                    IsDeleted = false,
                    CreatedBy = ExpenseDetails.CreatedBy,
                    CreatedOn = DateTime.Now,
                    PaymentType = 1
                };

                if (ExpenseDetails.Account == "Debit")
                {
                    expense.BillNumber = ExpenseDetails.BillNumber;
                    expense.Description = ExpenseDetails.Description;
                    expense.Date = ExpenseDetails.Date;
                    expense.Image = ExpenseDetails.Image;

                    if (ExpenseDetails.Role == "Account" || ExpenseDetails.Role == "Super Admin")
                    {
                        expense.Account = "Credit";
                        expense.IsPaid = true;
                        expense.IsApproved = true;
                    }
                    else
                    {
                        expense.Account = ExpenseDetails.Account;
                        expense.IsPaid = false;
                        expense.IsApproved = false;
                    }
                }
                else
                {
                    expense.BillNumber = "BTPLBILL";
                    expense.Description = "Expense Paid";
                    expense.Date = DateTime.Today;
                    expense.Account = ExpenseDetails.Account;
                    expense.PaymentType = ExpenseDetails.PaymentType;
                    expense.IsPaid = false;
                    expense.IsApproved = true;
                    expense.ApprovedBy = ExpenseDetails.ApprovedBy;
                    expense.ApprovedByName = ExpenseDetails.ApprovedByName;
                    expense.ApprovedDate = DateTime.Now;
                }

                response.Message = "Expense added successfully!";
                Context.TblExpenseMasters.Add(expense);
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = $"Error in adding expense: {ex.Message}";
            }
            return response;
        }




        public async Task<UserResponceModel> GetExpenseDetailById(Guid Id)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                ExpenseDetailsView ExpenseDetail = new ExpenseDetailsView();
                ExpenseDetail = (from a in Context.TblExpenseMasters.Where(x => x.Id == Id)
                                 join b in Context.TblPaymentTypes on a.PaymentType equals b.Id
                                 join c in Context.TblExpenseTypes on a.ExpenseType equals c.Id

                                 select new ExpenseDetailsView
                                 {
                                     Id = a.Id,
                                     UserId = a.UserId,
                                     ExpenseType = a.ExpenseType,
                                     PaymentType = a.PaymentType,
                                     PaymentTypeName = b.Type,
                                     BillNumber = a.BillNumber,
                                     Description = a.Description,
                                     Date = a.Date,
                                     TotalAmount = a.TotalAmount,
                                     Image = a.Image,
                                     Account = a.Account,
                                     IsPaid = a.IsPaid,
                                     IsApproved = a.IsApproved,
                                     ProjectId = a.ProjectId,
                                     ApprovedBy = a.ApprovedBy,
                                     ApprovedByName = a.ApprovedByName,
                                     CreatedBy = a.CreatedBy,
                                     CreatedOn = a.CreatedOn,
                                     ExpenseTypeName = c.Type,
                                 }).First();

                response.Data = ExpenseDetail;
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "Error in getting expense detail by id";
            }
            return response;
        }

        public async Task<jsonData> GetExpenseDetailList(DataTableRequstModel dataTable)
        {
            try
            {
                var Expense = (from a in Context.TblExpenseMasters
                               join b in Context.TblExpenseTypes on a.ExpenseType equals b.Id
                               join c in Context.TblPaymentTypes on a.PaymentType equals c.Id
                               join d in Context.TblUsers on a.UserId equals d.Id
                               where a.IsDeleted != true
                               select new ExpenseDetailsView
                               {
                                   Id = a.Id,
                                   UserId = a.UserId,
                                   UserName = d.FirstName + " " + d.LastName,
                                   ExpenseType = a.ExpenseType,
                                   PaymentType = a.PaymentType,
                                   BillNumber = a.BillNumber,
                                   Description = a.Description,
                                   Date = a.Date,
                                   TotalAmount = a.TotalAmount,
                                   Image = a.Image,
                                   Account = a.Account,
                                   ExpenseTypeName = b.Type,
                                   PaymentTypeName = c.Type,
                                   IsApproved = a.IsApproved,
                               });


                if (!string.IsNullOrEmpty(dataTable.sortColumn) && !string.IsNullOrEmpty(dataTable.sortColumnDir))
                {
                    Expense = Expense.OrderBy(dataTable.sortColumn + " " + dataTable.sortColumnDir);
                }
                else
                {
                    Expense = Expense.OrderBy("Date desc");
                }


                if (!string.IsNullOrEmpty(dataTable.searchValue))
                {
                    string searchValue = dataTable.searchValue.ToLower();
                    DateTime searchDate;
                    bool isDate = DateTime.TryParseExact(dataTable.searchValue, "dd MMM yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out searchDate);

                    Expense = Expense.Where(e => e.Description.ToLower().Contains(searchValue) ||
                                                 (isDate && e.Date == searchDate) ||
                                                 e.Account.ToLower().Contains(searchValue) ||
                                                 e.BillNumber.ToLower().Contains(searchValue) ||
                                                 e.UserName.ToLower().Contains(searchValue) ||
                                                 e.TotalAmount.ToString().ToLower().Contains(searchValue));
                }

                int totalRecord = await Expense.CountAsync();

                var cData = await Expense.Skip(dataTable.skip).Take(dataTable.pageSize).ToListAsync();

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

                    Context.TblExpenseMasters.Update(GetExpenseDetail);
                    Context.SaveChanges();
                    model.Message = "Expense details updated successfully!";
                }
                else
                {
                    model.Code = (int)HttpStatusCode.NotFound;
                    model.Message = "Can't Find Expense Details";
                }
            }
            catch (Exception ex)
            {
                model.Code = (int)HttpStatusCode.InternalServerError;
                model.Message = "Error in updating expense details";
            }
            return model;
        }

        public async Task<jsonData> GetUserExpenseList(Guid UserId, DataTableRequstModel dataTable)
        {
            try
            {
                var expenses = (from a in Context.TblExpenseMasters
                                join b in Context.TblExpenseTypes on a.ExpenseType equals b.Id
                                join c in Context.TblPaymentTypes on a.PaymentType equals c.Id
                                where a.UserId == UserId && a.IsDeleted == false
                                select new ExpenseDetailsView
                                {
                                    Id = a.Id,
                                    UserId = a.UserId,
                                    ExpenseType = a.ExpenseType,
                                    PaymentType = a.PaymentType,
                                    BillNumber = a.BillNumber,
                                    IsApproved = a.IsApproved,
                                    Description = a.Description,
                                    Date = a.Date,
                                    TotalAmount = a.TotalAmount,
                                    Image = a.Image,
                                    Account = a.Account,
                                    ExpenseTypeName = b.Type,
                                    PaymentTypeName = c.Type,
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
                        case "Description":
                            expenses = dataTable.sortColumnDir == "asc" ? expenses.OrderBy(e => e.Description) : expenses.OrderByDescending(e => e.Description);
                            break;
                        case "ExpenseTypeName":
                            expenses = dataTable.sortColumnDir == "asc" ? expenses.OrderBy(e => e.ExpenseTypeName) : expenses.OrderByDescending(e => e.ExpenseTypeName);
                            break;
                        case "PaymentTypeName":
                            expenses = dataTable.sortColumnDir == "asc" ? expenses.OrderBy(e => e.PaymentTypeName) : expenses.OrderByDescending(e => e.PaymentTypeName);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    expenses = expenses.OrderBy("Date desc");
                }
                if (!string.IsNullOrEmpty(dataTable.searchValue))
                {
                    string searchValue = dataTable.searchValue.ToLower();
                    DateTime searchDate;
                    bool isDate = DateTime.TryParseExact(dataTable.searchValue, "dd MMM yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out searchDate);

                    expenses = expenses.Where(e => e.BillNumber.ToLower().Contains(searchValue) ||
                                                 (isDate && e.Date == searchDate) ||
                                                 e.Account.ToLower().Contains(searchValue) ||
                                                 e.TotalAmount.ToString().ToLower().Contains(searchValue));
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
                               where a.IsDeleted == false
                               group new { a, b } by new { a.UserId, b.Image, b.UserName, FullName = b.FirstName + " " + b.LastName, b.FirstName, b.LastName } into userGroup
                               select new UserExpenseDetailsView
                               {
                                   UserId = userGroup.Key.UserId,
                                   FullName = userGroup.Key.FullName,
                                   FirstName = userGroup.Key.FirstName,
                                   LastName = userGroup.Key.LastName,
                                   Image = userGroup.Key.Image,
                                   UserName = userGroup.Key.UserName,
                                   Date = userGroup.Max(e => e.a.Date),
                                   TotalAmount = userGroup.Sum(e => e.a.TotalAmount),
                                   UnapprovedPendingAmount = userGroup.Where(e => e.a.IsApproved == true && e.a.Account == "Debit").Sum(e => e.a.TotalAmount),
                                   TotalPendingAmount = userGroup.Where(e => e.a.Account == "Debit").Sum(e => e.a.TotalAmount)
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
                        case "UnapprovedPendingAmount":
                            UserList = dataTable.sortColumnDir == "asc" ? UserList.OrderBy(e => e.UnapprovedPendingAmount) : UserList.OrderByDescending(e => e.UnapprovedPendingAmount);
                            break;
                        case "TotalPendingAmount":
                            UserList = dataTable.sortColumnDir == "asc" ? UserList.OrderBy(e => e.TotalPendingAmount) : UserList.OrderByDescending(e => e.TotalPendingAmount);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    UserList = UserList.OrderBy("Date desc");
                }
                if (!string.IsNullOrEmpty(dataTable.searchValue))
                {
                    string searchValue = dataTable.searchValue.ToLower();
                    DateTime searchDate;
                    bool isDate = DateTime.TryParseExact(dataTable.searchValue, "dd MMM yy", CultureInfo.InvariantCulture, DateTimeStyles.None, out searchDate);

                    UserList = UserList.Where(e => e.UserName.ToLower().Contains(searchValue) ||
                                                 (isDate && e.Date == searchDate) ||
                                                 e.FullName.ToLower().Contains(searchValue) ||
                                                 e.TotalAmount.ToString().ToLower().Contains(searchValue));
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

        public async Task<List<ExpenseDetailsView>> GetExpenseDetailByUserId(Guid UserId)
        {
            var ExpenseDetail = new List<ExpenseDetailsView>();
            var data = await Context.TblExpenseMasters.Where(x => x.UserId == UserId && x.IsDeleted == false).ToListAsync();
            if (data != null)
            {
                foreach (var item in data)
                {
                    ExpenseDetail.Add(new ExpenseDetailsView()
                    {
                        Id = item.Id,
                        UserId = item.UserId,
                        ExpenseType = item.ExpenseType,
                        PaymentType = item.PaymentType,
                        BillNumber = item.BillNumber,
                        Description = item.Description,
                        Date = item.Date,
                        TotalAmount = item.TotalAmount,
                        Image = item.Image,
                        Account = item.Account,
                        IsPaid = item.IsPaid,
                        IsApproved = item.IsApproved,
                        ApprovedBy = item.ApprovedBy,
                        ApprovedByName = item.ApprovedByName,
                        CreatedBy = item.CreatedBy,
                        CreatedOn = item.CreatedOn,

                    });
                }
            }
            return ExpenseDetail;
        }

        public async Task<UserResponceModel> ApprovedExpense(List<ApprovedExpense> ApprovedallExpense)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                foreach (var item in ApprovedallExpense)
                {

                    var expense = await Context.TblExpenseMasters.FindAsync(item.Id);

                    if (expense != null)
                    {

                        expense.IsApproved = true;
                        expense.ApprovedBy = item.ApprovedBy;
                        expense.ApprovedByName = item.ApprovedByName;
                        expense.ApprovedDate = DateTime.Now;

                        Context.Entry(expense).State = EntityState.Modified;
                    }
                    else
                    {

                        response.Code = (int)HttpStatusCode.NotFound;
                        response.Message = "Expense not found for ID: " + item.Id;
                        return response;
                    }
                }


                await Context.SaveChangesAsync();


                response.Message = "All expenses approved successfully!";
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "Error in updating expenses.";
            }
            return response;
        }

        public async Task<UserResponceModel> DeleteExpense(Guid Id)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var GetExpensedata = Context.TblExpenseMasters.Where(a => a.Id == Id).FirstOrDefault();

                if (GetExpensedata != null)
                {
                    GetExpensedata.IsDeleted = true;
                    Context.TblExpenseMasters.Update(GetExpensedata);
                    Context.SaveChanges();

                    response.Data = GetExpensedata;
                    response.Message = "Expense is deleted successfully";
                }
                else
                {
                    response.Code = (int)HttpStatusCode.NotFound;
                    response.Message = "Can't find the expense Id";
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "Error deleting expenses";
            }
            return response;
        }
    }
}
