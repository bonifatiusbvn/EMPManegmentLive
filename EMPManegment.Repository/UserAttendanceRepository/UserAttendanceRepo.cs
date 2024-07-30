using EMPManagment.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.Inretface.Interface.UserAttendance;
using Microsoft.VisualBasic;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net;
using System.Linq.Dynamic.Core;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;

using System.Drawing;
using System.Globalization;
using Azure;
using System.Diagnostics.Eventing.Reader;
using Microsoft.Extensions.Logging;
using EMPManegment.EntityModels.ViewModels.ProjectModels;
using EMPManegment.EntityModels.Common;
using EMPManegment.EntityModels.ViewModels.Invoice;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using EMPManegment.EntityModels.ViewModels.ExpenseMaster;

namespace EMPManegment.Repository.UserAttendanceRepository
{
    public class UserAttendanceRepo : IUserAttendance
    {
        public UserAttendanceRepo(BonifatiusEmployeesContext context, IConfiguration configuration)
        {
            Context = context;
            Configuration = configuration;
        }
        public IConfiguration _configuration { get; }
        public BonifatiusEmployeesContext Context { get; }
        public IConfiguration Configuration { get; }

        public async Task<jsonData> GetUserAttendanceList(DataTableRequstModel dataTable)
        {
            string dbConnectionStr = Configuration.GetConnectionString("EMPDbconn");
            var dataSet = DbHelper.GetDataSet("[spGetAttendanceList]", System.Data.CommandType.StoredProcedure, new SqlParameter[] { }, dbConnectionStr);

            var AttendanceList = new List<UserAttendanceModel>();

            foreach (DataRow row in dataSet.Tables[0].Rows)
            {
                var Attendance = new UserAttendanceModel
                {

                    UserId = Guid.Parse(row["UserId"].ToString()),
                    UserName = row["UserName"].ToString(),
                    Date = Convert.ToDateTime(row["Date"]),
                    AttendanceId = row["AttendanceId"] != DBNull.Value ? Convert.ToInt32(row["AttendanceId"]) : (int?)null,
                    Intime = Convert.ToDateTime(row["INTime"]),
                    OutTime = row["OutTime"] != DBNull.Value ? Convert.ToDateTime(row["OutTime"]) : (DateTime?)null,
                    TotalHours = row["TotalHours"] != DBNull.Value ? TimeSpan.Parse(row["TotalHours"].ToString()) : (TimeSpan?)null,
                    CreatedOn = row["CreatedOn"] != DBNull.Value ? Convert.ToDateTime(row["CreatedOn"]) : (DateTime?)null,
                };
                AttendanceList.Add(Attendance);
            }
            if (!string.IsNullOrEmpty(dataTable.searchValue))
            {
                AttendanceList = AttendanceList.Where(e => e.UserName.Contains(dataTable.searchValue) || e.Date.ToString().ToLower().Contains(dataTable.searchValue.ToLower())).ToList();
            }

            IQueryable<UserAttendanceModel> queryableExpenseDetails = AttendanceList.AsQueryable();

            if (!string.IsNullOrEmpty(dataTable.sortColumn) && !string.IsNullOrEmpty(dataTable.sortColumnDir))
            {
                queryableExpenseDetails = queryableExpenseDetails.OrderBy(dataTable.sortColumn + " " + dataTable.sortColumnDir);
            }
            var totalRecord = queryableExpenseDetails.Count();
            var filteredData = queryableExpenseDetails.Skip(dataTable.skip).Take(dataTable.pageSize).ToList();

            var jsonData = new jsonData
            {
                draw = dataTable.draw,
                recordsFiltered = totalRecord,
                recordsTotal = totalRecord,
                data = filteredData
            };

            return jsonData;
        }

        public async Task<UserAttendanceResponseModel> GetUserAttendanceInTime(UserAttendanceRequestModel userAttendance)
        {
            UserAttendanceResponseModel response = new UserAttendanceResponseModel();
            var UserAttendance = Context.TblAttendances.Where(e => e.UserId == userAttendance.UserId && e.Date == DateTime.Today).FirstOrDefault();
            try
            {
                if (UserAttendance != null)
                {
                    UserAttendanceModel attendanceModel = new UserAttendanceModel()
                    {
                        UserId = UserAttendance.UserId,
                        Date = UserAttendance.Date,
                        Intime = UserAttendance.Intime,
                        TotalHours = UserAttendance.TotalHours,
                        CreatedBy = UserAttendance.CreatedBy,
                        CreatedOn = UserAttendance.CreatedOn,
                        OutTime = UserAttendance.OutTime,
                        AttendanceId = UserAttendance.Id,
                    };
                    response.Data = attendanceModel;
                }
                else
                {
                    response.Code = (int)HttpStatusCode.NotFound;
                    response.Message = "UserId Doesn't found.";
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "Error in getting user attendance in time.";
            }
            return response;
        }

        public async Task<UserResponceModel> UpdateUserOutTime(UserAttendanceModel userAttendance)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var UserAttendance = Context.TblAttendances.FirstOrDefault(a => a.Id == userAttendance.AttendanceId);
                if (UserAttendance != null)
                {
                    string today = UserAttendance.Date.ToString("dd/MM/yyyy");
                    string checkdate = userAttendance.OutTime?.ToString("dd/MM/yyyy");

                    if (today == checkdate)
                    {
                        if (userAttendance.Intime <= userAttendance.OutTime)
                        {
                            UserAttendance.Intime = userAttendance.Intime;
                            UserAttendance.OutTime = userAttendance.OutTime;
                            UserAttendance.TotalHours = UserAttendance.OutTime - UserAttendance.Intime;
                            UserAttendance.UpdatedOn = DateTime.Now;
                            UserAttendance.UpdatedBy = userAttendance.UpdatedBy;
                            response.Message = "User updated successfully!";
                            Context.TblAttendances.Update(UserAttendance);
                            Context.SaveChanges();
                        }
                        else
                        {
                            response.Code = (int)HttpStatusCode.InternalServerError;
                            response.Message = "Intime cannot be greater than OutTime.";
                        }
                    }

                    else
                    {
                        response.Code = (int)HttpStatusCode.NotFound;
                        response.Message = "Please select valid date!!";
                    }

                }
                else
                {
                    response.Message = "Please select valid date!!";
                    response.Code = (int)HttpStatusCode.NotFound;
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "Error in updating user out time.";
            }
            return response;
        }

        public async Task<IEnumerable<UserAttendanceModel>> GetUserAttendanceById(int attendanceId)
        {
            IEnumerable<UserAttendanceModel> attendanceById = from a in Context.TblAttendances
                                                              join b in Context.TblUsers on a.User.Id equals b.Id
                                                              where a.Id == attendanceId
                                                              orderby a.Date descending
                                                              select new UserAttendanceModel
                                                              {
                                                                  UserName = b.FirstName + ' ' + b.LastName,
                                                                  UserId = a.UserId,
                                                                  AttendanceId = a.Id,
                                                                  Date = a.Date,
                                                                  Intime = a.Intime,
                                                                  OutTime = a.OutTime,
                                                                  TotalHours = a.OutTime - a.Intime,
                                                                  CreatedOn = a.CreatedOn,
                                                                  CreatedBy = a.CreatedBy,
                                                              };
            return attendanceById;
        }
        public async Task<jsonData> GetAttendanceList(MyAttendanceRequestDataTableModel AttendanceRequestModel)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@UserId", AttendanceRequestModel.SearchAttendance.UserId ?? (object)DBNull.Value),
                    new SqlParameter("@Cmonth", AttendanceRequestModel.SearchAttendance.Cmonth != DateTime.MinValue ? (object)AttendanceRequestModel.SearchAttendance.Cmonth : DBNull.Value),
                    new SqlParameter("@StartDate", AttendanceRequestModel.SearchAttendance.StartDate ?? (object)DBNull.Value),
                    new SqlParameter("@EndDate", AttendanceRequestModel.SearchAttendance.EndDate ?? (object)DBNull.Value)
                };

                string dbConnectionStr = Configuration.GetConnectionString("EMPDbconn");
                var DS = DbHelper.GetDataSet("spGetMySearchAttendanceList", CommandType.StoredProcedure, parameters.ToArray(), dbConnectionStr);

                List<UserAttendanceModel> userAttendance = new List<UserAttendanceModel>();

                if (DS != null && DS.Tables.Count > 0)
                {
                    foreach (DataRow row in DS.Tables[0].Rows)
                    {
                        UserAttendanceModel attendance = new UserAttendanceModel
                        {
                            UserName = row["UserName"]?.ToString(),
                            UserId = row["UserId"] != DBNull.Value ? (Guid)row["UserId"] : Guid.Empty,
                            AttendanceId = row["AttendanceId"] != DBNull.Value ? (int)row["AttendanceId"] : (int?)null,
                            Date = row["Date"] != DBNull.Value ? (DateTime)row["Date"] : DateTime.MinValue,
                            Intime = row["Intime"] != DBNull.Value ? (DateTime)row["Intime"] : DateTime.MinValue,
                            OutTime = row["OutTime"] != DBNull.Value ? (DateTime?)row["OutTime"] : null,
                            TotalHours = row["TotalHours"] != DBNull.Value ? (TimeSpan?)row["TotalHours"] : null,
                        };
                        userAttendance.Add(attendance);
                    }
                }
                if (!string.IsNullOrEmpty(AttendanceRequestModel.DataTable.searchValue))
                {
                    userAttendance = userAttendance.Where(e => e.UserName.Contains(AttendanceRequestModel.DataTable.searchValue) || e.Date.ToString().ToLower().Contains(AttendanceRequestModel.DataTable.searchValue.ToLower())).ToList();
                }

                IQueryable<UserAttendanceModel> queryableExpenseDetails = userAttendance.AsQueryable();

                if (!string.IsNullOrEmpty(AttendanceRequestModel.DataTable.sortColumn) && !string.IsNullOrEmpty(AttendanceRequestModel.DataTable.sortColumnDir))
                {
                    queryableExpenseDetails = queryableExpenseDetails.OrderBy(AttendanceRequestModel.DataTable.sortColumn + " " + AttendanceRequestModel.DataTable.sortColumnDir);
                }
                var totalRecord = queryableExpenseDetails.Count();
                var filteredData = queryableExpenseDetails.Skip(AttendanceRequestModel.DataTable.skip).Take(AttendanceRequestModel.DataTable.pageSize).ToList();

                var jsonData = new jsonData
                {
                    draw = AttendanceRequestModel.DataTable.draw,
                    recordsFiltered = totalRecord,
                    recordsTotal = totalRecord,
                    data = filteredData
                };

                return jsonData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching attendance list", ex);
            }
        }
        public async Task<jsonData> GetSearchAttendanceList(AttendanceRequestDataTableModel AttendanceRequestModel)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@UserId", AttendanceRequestModel.SearchAttendance.UserId ?? (object)DBNull.Value),
                    new SqlParameter("@Date", AttendanceRequestModel.SearchAttendance.Date != DateTime.MinValue ? (object)AttendanceRequestModel.SearchAttendance.Date : DBNull.Value),
                    new SqlParameter("@StartDate", AttendanceRequestModel.SearchAttendance.StartDate ?? (object)DBNull.Value),
                    new SqlParameter("@EndDate", AttendanceRequestModel.SearchAttendance.EndDate ?? (object)DBNull.Value)
                };

                string dbConnectionStr = Configuration.GetConnectionString("EMPDbconn");
                var DS = DbHelper.GetDataSet("GetSearchAttendanceList", CommandType.StoredProcedure, parameters.ToArray(), dbConnectionStr);

                List<UserAttendanceModel> userAttendance = new List<UserAttendanceModel>();

                if (DS != null && DS.Tables.Count > 0)
                {
                    foreach (DataRow row in DS.Tables[0].Rows)
                    {
                        UserAttendanceModel attendance = new UserAttendanceModel
                        {
                            UserName = row["UserName"]?.ToString(),
                            UserId = row["UserId"] != DBNull.Value ? (Guid)row["UserId"] : Guid.Empty,
                            AttendanceId = row["AttendanceId"] != DBNull.Value ? (int)row["AttendanceId"] : (int?)null,
                            Date = row["Date"] != DBNull.Value ? (DateTime)row["Date"] : DateTime.MinValue,
                            Intime = row["Intime"] != DBNull.Value ? (DateTime)row["Intime"] : DateTime.MinValue,
                            OutTime = row["OutTime"] != DBNull.Value ? (DateTime?)row["OutTime"] : null,
                            TotalHours = row["TotalHours"] != DBNull.Value ? (TimeSpan?)row["TotalHours"] : null,
                        };
                        userAttendance.Add(attendance);
                    }
                }
                if (!string.IsNullOrEmpty(AttendanceRequestModel.DataTable.searchValue))
                {
                    userAttendance = userAttendance.Where(e => e.UserName.Contains(AttendanceRequestModel.DataTable.searchValue) || e.Date.ToString().ToLower().Contains(AttendanceRequestModel.DataTable.searchValue.ToLower())).ToList();
                }

                IQueryable<UserAttendanceModel> queryableExpenseDetails = userAttendance.AsQueryable();

                if (!string.IsNullOrEmpty(AttendanceRequestModel.DataTable.sortColumn) && !string.IsNullOrEmpty(AttendanceRequestModel.DataTable.sortColumnDir))
                {
                    queryableExpenseDetails = queryableExpenseDetails.OrderBy(AttendanceRequestModel.DataTable.sortColumn + " " + AttendanceRequestModel.DataTable.sortColumnDir);
                }
                var totalRecord = queryableExpenseDetails.Count();
                var filteredData = queryableExpenseDetails.Skip(AttendanceRequestModel.DataTable.skip).Take(AttendanceRequestModel.DataTable.pageSize).ToList();

                var jsonData = new jsonData
                {
                    draw = AttendanceRequestModel.DataTable.draw,
                    recordsFiltered = totalRecord,
                    recordsTotal = totalRecord,
                    data = filteredData
                };

                return jsonData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching attendance list", ex);
            }
        }
    }
}


