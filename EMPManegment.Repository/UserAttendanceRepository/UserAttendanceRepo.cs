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
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using EMPManegment.EntityModels.ViewModels.ProductMaster;

namespace EMPManegment.Repository.UserAttendanceRepository
{
    public class UserAttendanceRepo : IUserAttendance
    {
        public UserAttendanceRepo(BonifatiusEmployeesContext context, IConfiguration configuration)
        {
            Context = context;
            _configuration = configuration;
        }
        public IConfiguration _configuration { get; }
        public BonifatiusEmployeesContext Context { get; }


        public async Task<jsonData> GetUserAttendanceList(DataTableRequstModel dataTable)
        {
            var AttendanceDataTable = from a in Context.TblAttendances
                                      join b in Context.TblUsers on a.User.Id equals b.Id
                                      orderby a.Date descending
                                      select new UserAttendanceModel
                                      {
                                          UserName = b.FirstName + ' ' + b.LastName,
                                          UserId = a.UserId,
                                          AttendanceId = a.Id,
                                          Date = a.Date,
                                          Intime = a.Intime,
                                          OutTime = a.OutTime,
                                          TotalHours = a.TotalHours,
                                          CreatedBy = a.CreatedBy,
                                          CreatedOn = a.CreatedOn,

                                      };
            if (!string.IsNullOrEmpty(dataTable.sortColumn) && !string.IsNullOrEmpty(dataTable.sortColumnDir))
            {
                AttendanceDataTable = AttendanceDataTable.OrderBy(dataTable.sortColumn + " " + dataTable.sortColumnDir);
            }

            if (!string.IsNullOrEmpty(dataTable.searchValue))
            {
                AttendanceDataTable = AttendanceDataTable.Where(e => e.UserName.Contains(dataTable.searchValue) || e.Date.ToString().ToLower().Contains(dataTable.searchValue.ToLower()));
            }

            int totalRecord = AttendanceDataTable.Count();

            var cData = AttendanceDataTable.Skip(dataTable.skip).Take(dataTable.pageSize).ToList();

            jsonData jsonData = new jsonData
            {
                draw = dataTable.draw,
                recordsFiltered = totalRecord,
                recordsTotal = totalRecord,
                data = cData
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
                        UserAttendance.OutTime = userAttendance.OutTime;
                        UserAttendance.TotalHours = UserAttendance.OutTime - UserAttendance.Intime;
                        UserAttendance.UpdatedOn = DateTime.Now;
                        UserAttendance.UpdatedBy = userAttendance.UpdatedBy;
                        response.Message = "User outTime successfully updated";
                        Context.TblAttendances.Update(UserAttendance);
                        Context.SaveChanges();
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
        public async Task<IEnumerable<UserAttendanceModel>> GetAttendanceList(SearchAttendanceModel GetAttendanceList)
        {
            try
            {
                IEnumerable<UserAttendanceModel> userAttendance = null;
                if (GetAttendanceList.Cmonth != null && GetAttendanceList.Cmonth != DateTime.MinValue)
                {
                    IEnumerable<UserAttendanceModel> userAttendance1 = from a in Context.TblAttendances
                                                                       join
                                                                       b in Context.TblUsers on a.User.Id equals b.Id
                                                                       where (b.Id == GetAttendanceList.UserId && (a.Date.Month == Convert.ToDateTime(GetAttendanceList.Cmonth).Month && a.Date.Year == Convert.ToDateTime(GetAttendanceList.Cmonth).Year))
                                                                       orderby a.Date descending
                                                                       select new UserAttendanceModel
                                                                       {
                                                                           UserName = b.FirstName + ' ' + b.LastName,
                                                                           UserId = a.UserId,
                                                                           AttendanceId = a.Id,
                                                                           Date = a.Date,
                                                                           Intime = a.Intime,
                                                                           OutTime = a.OutTime,
                                                                           TotalHours = a.TotalHours,
                                                                           CreatedBy = a.CreatedBy,
                                                                           CreatedOn = a.CreatedOn,
                                                                       };
                    return userAttendance1;
                }
                else
                {
                    if (GetAttendanceList.StartDate != null && GetAttendanceList.EndDate != null && GetAttendanceList.StartDate != DateTime.MinValue && GetAttendanceList.EndDate != DateTime.MinValue)
                    {
                        userAttendance = from a in Context.TblAttendances
                                         join b in Context.TblUsers on a.User.Id equals b.Id
                                         where (b.Id == GetAttendanceList.UserId && a.Date >= GetAttendanceList.StartDate && a.Date <= GetAttendanceList.EndDate)
                                         orderby a.Date descending
                                         select new UserAttendanceModel
                                         {
                                             UserName = b.FirstName + ' ' + b.LastName,
                                             UserId = a.UserId,
                                             AttendanceId = a.Id,
                                             Date = a.Date,
                                             Intime = a.Intime,
                                             OutTime = a.OutTime,
                                             TotalHours = a.TotalHours,
                                             CreatedBy = a.CreatedBy,
                                             CreatedOn = a.CreatedOn,
                                         };

                        return userAttendance;
                    }
                    else
                    {
                        DateTime date = DateTime.Today;
                        userAttendance = from a in Context.TblAttendances
                                         join
                                         b in Context.TblUsers on a.User.Id equals b.Id
                                         where (b.Id == GetAttendanceList.UserId && (a.Date.Month == Convert.ToDateTime(date).Month && a.Date.Year == Convert.ToDateTime(date).Year))
                                         orderby a.Date descending
                                         select new UserAttendanceModel
                                         {
                                             UserName = b.FirstName + ' ' + b.LastName,
                                             UserId = a.UserId,
                                             AttendanceId = a.Id,
                                             Date = a.Date,
                                             Intime = a.Intime,
                                             OutTime = a.OutTime,
                                             TotalHours = a.TotalHours,
                                             CreatedBy = a.CreatedBy,
                                             CreatedOn = a.CreatedOn,
                                         };

                        return userAttendance;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<UserAttendanceModel>> GetSearchAttendanceList(searchAttendanceListModel AttendanceList)
        {
            try
            {
                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter("@UserId", AttendanceList.UserId ?? (object)DBNull.Value),
                    new SqlParameter("@Date", AttendanceList.Date != DateTime.MinValue ? (object)AttendanceList.Date : DBNull.Value),
                    new SqlParameter("@StartDate", AttendanceList.StartDate ?? (object)DBNull.Value),
                    new SqlParameter("@EndDate", AttendanceList.EndDate ?? (object)DBNull.Value)
                };

                string dbConnectionStr = _configuration.GetConnectionString("EMPDbconn");
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

                return userAttendance;
            }
            catch (Exception ex)
            {
                throw new Exception("Error fetching attendance list", ex);
            }
        }



    }
}


