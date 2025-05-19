using EMPManagment.API;
using EMPManegment.EntityModels.Common;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels.AGGridModels;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.ExpenseMaster;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.Inretface.Interface.UserAttendance;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq.Dynamic.Core;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;
#nullable disable
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

        public async Task<AGGridResponseModel<UserAttendanceModel>> GetUserAttendanceList(AGGridRequestModel AttendanceRequest)
        {
            try
            {
                var filterConditions = string.Join(" AND ", AttendanceRequest.filters.Select(f =>
                                    $"{f.ColId} LIKE '%{f.FilterValue}%'"));
                string sortColumn = AttendanceRequest.SortModel?.FirstOrDefault()?.ColId ?? "Date";
                string sortDirection = AttendanceRequest.SortModel?.FirstOrDefault()?.Sort ?? "desc";

                var parameters = new List<SqlParameter>
                {
                new SqlParameter("@SearchValue", (object)AttendanceRequest.SearchValue ?? DBNull.Value),
                new SqlParameter("@SortColumn", sortColumn),
                new SqlParameter("@SortDirection", sortDirection),
                new SqlParameter("@PageSize", AttendanceRequest.PageSize),
                new SqlParameter("@Skip", AttendanceRequest.StartRow),
                new SqlParameter("@StartDate", AttendanceRequest.StartDate),
                new SqlParameter("@EndDate", AttendanceRequest.EndDate),
                new SqlParameter("@UserFilter", AttendanceRequest.UserFilter),
                new SqlParameter("@FilterConditions", (object)filterConditions ?? DBNull.Value),
                new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output }
                };

                var dataSet = DbHelper.GetDataSet("spGetAttendanceList", CommandType.StoredProcedure, parameters.ToArray(), Configuration.GetConnectionString("EMPDbconn"));

                var AttendanceList = dataSet.Tables[0].AsEnumerable().Select(row => new UserAttendanceModel
                {
                    AttendanceId = row["AttendanceId"] != DBNull.Value ? Convert.ToInt32(row["AttendanceId"]) : 0,
                    UserId = row["UserId"] != DBNull.Value ? Guid.Parse(row["UserId"].ToString()) : Guid.Empty,
                    FirstName = row["FirstName"]?.ToString(),
                    LastName = row["LastName"]?.ToString(),
                    Date = Convert.ToDateTime(row["Date"]),
                    Intime = Convert.ToDateTime(row["InTime"]),
                    OutTime = row["OutTime"] != DBNull.Value ? Convert.ToDateTime(row["OutTime"]) : (DateTime?)null,
                    TotalHours = row["Totalhours"] != DBNull.Value ? (TimeSpan)row["Totalhours"] : TimeSpan.Zero,
                    CreatedOn = row["CreatedOn"] != DBNull.Value ? Convert.ToDateTime(row["CreatedOn"]) : (DateTime?)null,
                }).ToList();

                int totalRecords = (int)parameters.First(p => p.ParameterName == "@TotalRecords").Value;

                return new AGGridResponseModel<UserAttendanceModel>
                {
                    Data = AttendanceList,
                    RecordsTotal = totalRecords
                };
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the inword list.", ex);
            }
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
                    string today = DateTime.Now.ToString("dd/MM/yyyy");
                    string intimeDate = userAttendance.Intime.ToString("dd/MM/yyyy");
                    string outTimeDate = userAttendance.OutTime?.ToString("dd/MM/yyyy");
                    if (outTimeDate == intimeDate || intimeDate == today)
                    {
                        if (userAttendance.OutTime == null || userAttendance.Intime <= userAttendance.OutTime)
                        {
                            UserAttendance.Intime = userAttendance.Intime;
                            UserAttendance.OutTime = userAttendance.OutTime;
                            UserAttendance.TotalHours = UserAttendance.OutTime.HasValue ? UserAttendance.OutTime - UserAttendance.Intime : (TimeSpan?)null;
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
                        response.Message = "Please select valid date!";
                        response.Code = (int)HttpStatusCode.NotFound;
                    }
                }
                else
                {
                    response.Message = "Please select valid date!";
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
        public async Task<AGGridResponseModel<UserAttendanceModel>> GetMyAttendanceList(AGGridRequestModel AttendanceRequest)
        {
            try
            {
                var filterConditions = string.Join(" AND ", AttendanceRequest.filters.Select(f =>
                                    $"{f.ColId} LIKE '%{f.FilterValue}%'"));
                string sortColumn = AttendanceRequest.SortModel?.FirstOrDefault()?.ColId ?? "Date";
                string sortDirection = AttendanceRequest.SortModel?.FirstOrDefault()?.Sort ?? "desc";
                DateTime? parsedMonth = null;
                if (!string.IsNullOrEmpty(AttendanceRequest.Month))
                {
                    parsedMonth = DateTime.ParseExact(AttendanceRequest.Month + "-01", "yyyy-MM-dd", CultureInfo.InvariantCulture);
                }

                var parameters = new List<SqlParameter>
                {
                new SqlParameter("@SearchValue", (object)AttendanceRequest.SearchValue ?? DBNull.Value),
                new SqlParameter("@SortColumn", sortColumn),
                new SqlParameter("@SortDirection", sortDirection),
                new SqlParameter("@PageSize", AttendanceRequest.PageSize),
                new SqlParameter("@Skip", AttendanceRequest.StartRow),
                new SqlParameter("@StartDate", AttendanceRequest.StartDate),
                new SqlParameter("@EndDate", AttendanceRequest.EndDate),
                new SqlParameter("@Month", (object)parsedMonth ?? DBNull.Value),
                new SqlParameter("@UserFilter", AttendanceRequest.UserFilter),
                new SqlParameter("@FilterConditions", (object)filterConditions ?? DBNull.Value),
                new SqlParameter("@TotalRecords", SqlDbType.Int) { Direction = ParameterDirection.Output }
                };

                var dataSet = DbHelper.GetDataSet("spGetMySearchAttendanceList", CommandType.StoredProcedure, parameters.ToArray(), Configuration.GetConnectionString("EMPDbconn"));

                var AttendanceList = dataSet.Tables[0].AsEnumerable().Select(row => new UserAttendanceModel
                {
                    AttendanceId = row["AttendanceId"] != DBNull.Value ? Convert.ToInt32(row["AttendanceId"]) : 0,
                    UserId = row["UserId"] != DBNull.Value ? Guid.Parse(row["UserId"].ToString()) : Guid.Empty,
                    FirstName = row["FirstName"]?.ToString(),
                    LastName = row["LastName"]?.ToString(),
                    Date = Convert.ToDateTime(row["Date"]),
                    Intime = Convert.ToDateTime(row["InTime"]),
                    OutTime = row["OutTime"] != DBNull.Value ? Convert.ToDateTime(row["OutTime"]) : (DateTime?)null,
                    TotalHours = row["Totalhours"] != DBNull.Value ? (TimeSpan)row["Totalhours"] : TimeSpan.Zero,
                    CreatedOn = row["CreatedOn"] != DBNull.Value ? Convert.ToDateTime(row["CreatedOn"]) : (DateTime?)null,
                }).ToList();

                int totalRecords = (int)parameters.First(p => p.ParameterName == "@TotalRecords").Value;

                return new AGGridResponseModel<UserAttendanceModel>
                {
                    Data = AttendanceList,
                    RecordsTotal = totalRecords
                };
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the inword list.", ex);
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

        public async Task<IEnumerable<UserAttendanceModel>> GetMySearchAttendanceList(SearchAttendanceModel GetSearchAttendanceList)
        {
            try
            {
                IEnumerable<UserAttendanceModel> userAttendance = null;
                if (GetSearchAttendanceList.Cmonth != null && GetSearchAttendanceList.Cmonth != DateTime.MinValue)
                {
                    IEnumerable<UserAttendanceModel> userAttendance1 = from a in Context.TblAttendances
                                                                       join
                                                                       b in Context.TblUsers on a.User.Id equals b.Id
                                                                       where (b.Id == GetSearchAttendanceList.UserId && (a.Date.Month == Convert.ToDateTime(GetSearchAttendanceList.Cmonth).Month && a.Date.Year == Convert.ToDateTime(GetSearchAttendanceList.Cmonth).Year))
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
                    if (GetSearchAttendanceList.StartDate != null && GetSearchAttendanceList.EndDate != null && GetSearchAttendanceList.StartDate != DateTime.MinValue && GetSearchAttendanceList.EndDate != DateTime.MinValue)
                    {
                        userAttendance = from a in Context.TblAttendances
                                         join b in Context.TblUsers on a.User.Id equals b.Id
                                         where (b.Id == GetSearchAttendanceList.UserId && a.Date >= GetSearchAttendanceList.StartDate && a.Date <= GetSearchAttendanceList.EndDate)
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
                                         where (b.Id == GetSearchAttendanceList.UserId && (a.Date.Month == Convert.ToDateTime(date).Month && a.Date.Year == Convert.ToDateTime(date).Year))
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

        public async Task<UserResponceModel> AddUserAttendance(UserAttendanceModel AddUser)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var usermodel = new TblAttendance()
                {
                    UserId = AddUser.UserId,
                    Date = AddUser.Date,
                    Intime = AddUser.Intime,
                    OutTime = AddUser.OutTime,
                    TotalHours = AddUser.OutTime.HasValue ? AddUser.OutTime.Value - AddUser.Intime : (TimeSpan?)null,
                    CreatedBy = AddUser.CreatedBy,
                    CreatedOn = DateTime.Now,
                };
                response.Message = "User add successfully!";
                Context.TblAttendances.Add(usermodel);
                Context.SaveChanges();
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "Error in creating user.";
            }
            return response;
        }
    }
}


