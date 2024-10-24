using Azure;
using Azure.Core;
using EMPManagment.API;
using EMPManegment.EntityModels.Crypto;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.Inretface.Interface.UserAttendance;
using EMPManegment.Inretface.Interface.UserList;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Linq.Dynamic.Core;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using EMPManegment.EntityModels.ViewModels.UserModels;
using EMPManegment.EntityModels.ViewModels.ProductMaster;
using Microsoft.Extensions.Configuration;
using EMPManegment.EntityModels.Common;
using System.Security.Cryptography;
using EMPManegment.EntityModels.ViewModels.TaskModels;
using EMPManegment.EntityModels.ViewModels.ExpenseMaster;
using EMPManegment.EntityModels.ViewModels.ProjectModels;
#nullable disable

namespace EMPManegment.Repository.UserListRepository
{
    public class UserDetailsRepo : IUserDetails
    {
        public BonifatiusEmployeesContext Context { get; }
        public IConfiguration _configuration { get; }

        public UserDetailsRepo(BonifatiusEmployeesContext context, IConfiguration configuration)
        {
            Context = context;
            _configuration = configuration;

        }


        public async Task<jsonData> GetUsersList(DataTableRequstModel dataTable)
        {
            try
            {
                string dbConnectionStr = _configuration.GetConnectionString("EMPDbconn");

                var dataSet = DbHelper.GetDataSet("spGetUsersList", CommandType.StoredProcedure, new SqlParameter[] { }, dbConnectionStr);

                var userList = ConvertDataSetToUserList(dataSet);

                if (!string.IsNullOrEmpty(dataTable.searchValue.ToLower()))
                {
                    userList = userList.Where(e =>
                        e.UserName.Contains(dataTable.searchValue.ToLower(), StringComparison.OrdinalIgnoreCase) ||
                        e.FullName.Contains(dataTable.searchValue.ToLower(), StringComparison.OrdinalIgnoreCase) ||
                        e.DepartmentName.Contains(dataTable.searchValue.ToLower(), StringComparison.OrdinalIgnoreCase) ||
                        e.CityName.Contains(dataTable.searchValue.ToLower(), StringComparison.OrdinalIgnoreCase) ||
                        e.PhoneNumber.Contains(dataTable.searchValue.ToLower(), StringComparison.OrdinalIgnoreCase) ||
                        e.DateOfBirth.ToString().Contains(dataTable.searchValue)).ToList();
                }

                IQueryable<UserDataTblModel> queryableUserDetails = userList.AsQueryable();

                if (!string.IsNullOrEmpty(dataTable.sortColumn) && !string.IsNullOrEmpty(dataTable.sortColumnDir))
                {
                    switch (dataTable.sortColumn)
                    {
                        case "UserName":
                            queryableUserDetails = dataTable.sortColumnDir == "asc" ? queryableUserDetails.OrderBy(e => e.UserName) : queryableUserDetails.OrderByDescending(e => e.UserName);
                            break;
                        case "DepartmentName":
                            queryableUserDetails = dataTable.sortColumnDir == "asc" ? queryableUserDetails.OrderBy(e => e.DepartmentName) : queryableUserDetails.OrderByDescending(e => e.DepartmentName);
                            break;
                        case "RoleName":
                            queryableUserDetails = dataTable.sortColumnDir == "asc" ? queryableUserDetails.OrderBy(e => e.RoleName) : queryableUserDetails.OrderByDescending(e => e.RoleName);
                            break;
                        case "Address":
                            queryableUserDetails = dataTable.sortColumnDir == "asc" ? queryableUserDetails.OrderBy(e => e.Address) : queryableUserDetails.OrderByDescending(e => e.Address);
                            break;
                        case "PhoneNumber":
                            queryableUserDetails = dataTable.sortColumnDir == "asc" ? queryableUserDetails.OrderBy(e => e.PhoneNumber) : queryableUserDetails.OrderByDescending(e => e.PhoneNumber);
                            break;
                        case "DateOfBirth":
                            queryableUserDetails = dataTable.sortColumnDir == "asc" ? queryableUserDetails.OrderBy(e => e.DateOfBirth) : queryableUserDetails.OrderByDescending(e => e.DateOfBirth);
                            break;
                        case "FirstName":
                            queryableUserDetails = dataTable.sortColumnDir == "asc" ? queryableUserDetails.OrderBy(e => e.FirstName) : queryableUserDetails.OrderByDescending(e => e.FirstName);
                            break;
                        case "Gender":
                            queryableUserDetails = dataTable.sortColumnDir == "asc" ? queryableUserDetails.OrderBy(e => e.Gender) : queryableUserDetails.OrderByDescending(e => e.Gender);
                            break;
                        case "Email":
                            queryableUserDetails = dataTable.sortColumnDir == "asc" ? queryableUserDetails.OrderBy(e => e.Email) : queryableUserDetails.OrderByDescending(e => e.Email);
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    queryableUserDetails = queryableUserDetails.OrderBy("UserName d");
                }

                var totalRecord = queryableUserDetails.Count();
                var filteredData = queryableUserDetails.Skip(dataTable.skip).Take(dataTable.pageSize).ToList();

                var jsonData = new jsonData
                {
                    draw = dataTable.draw,
                    recordsFiltered = totalRecord,
                    recordsTotal = totalRecord,
                    data = filteredData
                };

                return jsonData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private List<UserDataTblModel> ConvertDataSetToUserList(DataSet dataSet)
        {
            var userDetails = new List<UserDataTblModel>();
            try
            {

                foreach (DataRow row in dataSet.Tables[0].Rows)
                {
                    var userData = new UserDataTblModel
                    {
                        Id = Guid.Parse(row["Id"].ToString()),
                        IsActive = (bool)row["IsActive"],
                        UserName = row["UserName"].ToString(),
                        FirstName = row["FirstName"].ToString(),
                        LastName = row["LastName"].ToString(),
                        Image = row["Image"].ToString(),
                        Gender = row["Gender"].ToString(),
                        Email = row["Email"].ToString(),
                        PhoneNumber = row["PhoneNumber"].ToString(),
                        Address = row["Address"].ToString(),
                        CityName = row["CityName"].ToString(),
                        StateName = row["StateName"].ToString(),
                        CountryName = row["CountryName"].ToString(),
                        DepartmentName = row["DepartmentName"].ToString(),
                        RoleId = Guid.Parse(row["RoleId"].ToString()),
                        RoleName = row["RoleName"].ToString(),
                        FullName = row["FullName"].ToString(),
                        DateOfBirth = Convert.ToDateTime(row["DateOfBirth"]),

                    };
                    userDetails.Add(userData);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return userDetails;
        }

        public async Task<UserResponceModel> ActiveDeactiveUsers(Guid UserId, Guid UpdatedBy)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var GetUserdta = Context.TblUsers.Where(a => a.Id == UserId).FirstOrDefault();

                if (GetUserdta != null)
                {

                    if (GetUserdta.IsActive == true)
                    {
                        GetUserdta.IsActive = false;
                        GetUserdta.UpdatedOn = DateTime.Now;
                        GetUserdta.UpdatedBy = UpdatedBy;
                        Context.TblUsers.Update(GetUserdta);
                        Context.SaveChanges();
                        response.Data = GetUserdta;
                        response.Message = "User" + " " + GetUserdta.UserName + " " + "Is Deactive Succesfully";
                    }

                    else
                    {
                        GetUserdta.IsActive = true;
                        GetUserdta.UpdatedOn = DateTime.Now;
                        GetUserdta.UpdatedBy = UpdatedBy;
                        Context.TblUsers.Update(GetUserdta);
                        Context.SaveChanges();
                        response.Data = GetUserdta;
                        response.Message = "User" + " " + GetUserdta.UserName + " " + "Is Active Succesfully";
                    }
                }
                else
                {
                    response.Code = (int)HttpStatusCode.NotFound;
                    response.Message = "Can't find the User";
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "Error in active-deactive of the user";
            }
            return response;
        }

        public async Task<UserResponceModel> EnterInTime(UserAttendanceModel userAttendance)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var Intimedata = Context.TblAttendances.Where(a => a.UserId == userAttendance.UserId).OrderByDescending(a => a.Date).FirstOrDefault();

                if (Intimedata != null)
                {
                    if (Intimedata.OutTime == null && Intimedata.Date != DateTime.Today)
                    {
                        response.Message = "You missed out-time of " + Intimedata.Date.ToString("dd/MM/yyyy") + " " + "Kindly contact your admin";
                        response.Icone = "warning";
                        response.Code = (int)HttpStatusCode.NotFound;
                    }


                    else
                    {
                        if (Intimedata.Date == DateTime.Today && Intimedata.Intime != null)
                        {
                            response.Message = "Your already enter in-time";
                            response.Icone = "warning";
                            response.Code = (int)HttpStatusCode.NotFound;
                        }

                        else
                        {
                            TblAttendance tblAttendance = new TblAttendance();
                            tblAttendance.UserId = userAttendance.UserId;
                            tblAttendance.Intime = DateTime.Now;
                            tblAttendance.Date = DateTime.Today;
                            tblAttendance.CreatedBy = userAttendance.CreatedBy;
                            tblAttendance.CreatedOn = DateTime.Now;
                            tblAttendance.OutTime = null;
                            Context.TblAttendances.Add(tblAttendance);
                            Context.SaveChanges();
                            response.Message = "In-time enter successfully";
                            response.Icone = "success";

                        }

                    }
                }

                else
                {
                    TblAttendance tblAttendance = new TblAttendance();
                    tblAttendance.UserId = userAttendance.UserId;
                    tblAttendance.Intime = DateTime.Now;
                    tblAttendance.Date = DateTime.Today;
                    tblAttendance.CreatedOn = DateTime.Now;
                    tblAttendance.CreatedBy = userAttendance.CreatedBy;
                    tblAttendance.OutTime = null;
                    Context.TblAttendances.Add(tblAttendance);
                    Context.SaveChanges();
                    response.Message = "In-time enter successfully";
                    response.Icone = "success";
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "Error occur at entering the time in.";
            }

            return response;
        }

        public async Task<UserResponceModel> EnterOutTime(UserAttendanceModel userAttendance)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var Outtimedata = Context.TblAttendances.Where(a => a.UserId == userAttendance.UserId).OrderByDescending(a => a.Date).FirstOrDefault(); ;
                if (Outtimedata.OutTime == null && Outtimedata.Date != DateTime.Today)
                {
                    response.Message = "You missed out-time of " + Outtimedata.Date.ToString("dd/MM/yyyy") + " " + "Kindly contact your admin";
                    response.Icone = "warning";
                    response.Code = (int)HttpStatusCode.NotFound;
                }

                else
                {
                    if (Outtimedata.Date != DateTime.Today)
                    {
                        response.Code = (int)HttpStatusCode.NotFound;
                        response.Message = "Please enter in-time first";
                        response.Icone = "warning";
                    }

                    else
                    {

                        if (Outtimedata.Date == DateTime.Today && Outtimedata.OutTime == null)
                        {
                            var outtime = Context.TblAttendances.Where(a => a.UserId == userAttendance.UserId && a.Date == DateTime.Today).FirstOrDefault();
                            outtime.OutTime = DateTime.Now;
                            outtime.CreatedOn = DateTime.Now;
                            outtime.TotalHours = outtime.OutTime - outtime.Intime;
                            Context.TblAttendances.Update(outtime);
                            Context.SaveChanges();
                            response.Message = "Out-time enter successfully";
                            response.Icone = "success";

                        }
                        else
                        {
                            response.Code = (int)HttpStatusCode.NotFound;
                            response.Message = "Your already enter out-time";
                            response.Icone = "warning";

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "Error occur at entering the time out.";
            }

            return response;
        }

        public async Task<UserResponceModel> ResetPassword(PasswordResetView ResetPassword)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                if (ResetPassword.UserId != null)
                {
                    var userdata = Context.TblUsers.FirstOrDefault(x => x.Id == ResetPassword.UserId);
                    if (userdata != null)
                    {
                        userdata.UpdatedBy = ResetPassword.UpdatedBy;
                        userdata.UpdatedOn = DateTime.Now;
                        userdata.PasswordHash = ResetPassword.PasswordHash;
                        userdata.PasswordSalt = ResetPassword.PasswordSalt;

                        Context.TblUsers.Update(userdata);
                        Context.SaveChanges();
                        response.Message = "Password updated!";
                    }
                    else
                    {
                        response.Code = (int)HttpStatusCode.NotFound;
                        response.Message = "Can't found User";
                    }
                }
                else
                {
                    if (ResetPassword.UserName != null)
                    {
                        var userdata = Context.TblUsers.FirstOrDefault(x => x.UserName == ResetPassword.UserName);
                        if (userdata != null)
                        {
                            userdata.UpdatedOn = DateTime.Now;
                            userdata.PasswordHash = ResetPassword.PasswordHash;
                            userdata.PasswordSalt = ResetPassword.PasswordSalt;

                            Context.TblUsers.Update(userdata);
                            Context.SaveChanges();
                            response.Message = "Password updated!";
                        }
                        else
                        {
                            response.Code = (int)HttpStatusCode.NotFound;
                            response.Message = "Can't found User";
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "Error in reset password.";
            }
            return response;
        }
        public async Task<IEnumerable<EmpDocumentView>> GetDocumentType()
        {
            try
            {
                IEnumerable<EmpDocumentView> documentList = Context.TblDocumentMasters.ToList().Select(a => new EmpDocumentView
                {
                    Id = a.Id,
                    DocumentType = a.DocumentType,
                });
                return documentList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<DocumentInfoView>> GetDocumentList(Guid Userid)
        {
            try
            {
                string dbConnectionStr = _configuration.GetConnectionString("EMPDbconn");

                var sqlPar = new SqlParameter[]
                {
                   new SqlParameter("@UserId", Userid),
                };

                var DS = DbHelper.GetDataSet("GetDocumentList", CommandType.StoredProcedure, sqlPar, dbConnectionStr);

                List<DocumentInfoView> DocumentList = new List<DocumentInfoView>();

                if (DS != null && DS.Tables.Count > 0)
                {
                    foreach (DataRow row in DS.Tables[0].Rows)
                    {
                        var userDocDetails = new DocumentInfoView
                        {
                            UserId = row["UserId"] != DBNull.Value ? (Guid)row["UserId"] : Guid.Empty,
                            Id = row["Id"] != DBNull.Value ? (int)row["Id"] : 0,
                            DocumentType = row["DocumentType"]?.ToString(),
                            DocumentName = row["DocumentName"]?.ToString(),
                            CreatedBy = row["CreatedBy"]?.ToString(),
                            CreatedOn = row["CreatedOn"] != DBNull.Value ? (DateTime)row["CreatedOn"] : DateTime.MinValue,

                        };
                        DocumentList.Add(userDocDetails);
                    }
                }
                return DocumentList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<DocumentInfoView> UploadDocument(DocumentInfoView docview)
        {
            var UploadDocument = new TblUserDocument()
            {
                Id = docview.Id,
                UserId = docview.UserId,
                DocumentTypeId = docview.DocumentTypeId,
                DocumentName = docview.DocumentName,
                CreatedOn = DateTime.Now,
                CreatedBy = docview.CreatedBy,
            };
            Context.TblUserDocuments.Add(UploadDocument);
            Context.SaveChanges();
            return docview;
        }

        public async Task<UserResponceModel> UserLockScreen(LoginRequest loginrequest)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var tblUser = Context.TblUsers.Where(p => p.UserName == loginrequest.UserName).SingleOrDefault();
                if (tblUser != null)
                {
                    if (tblUser.UserName == loginrequest.UserName && Crypto.VarifyHash(loginrequest.Password, tblUser.PasswordHash, tblUser.PasswordSalt))
                    {
                        LoginView userModel = new LoginView();
                        userModel.UserName = tblUser.UserName;
                    }
                    else
                    {
                        response.Code = (int)HttpStatusCode.NotFound;
                        response.Message = "Your password is wrong";
                    }
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "Error in locking the screen";
            }
            return response;
        }

        public async Task<UserResponceModel> UserBirsthDayWish(Guid UserId)
        {
            UserResponceModel response = new UserResponceModel();
            var userdata = Context.TblUsers.Where(e => e.Id == UserId).FirstOrDefault();

            try
            {
                if (userdata != null)
                {

                    string today = DateTime.Now.ToString("dd/MM");
                    string checkdate = userdata.DateOfBirth.ToString("dd/MM");
                    if (today == checkdate)
                    {
                        response.Message = "Bonifatius wish you a very happy birthday.." + " " + userdata.FirstName + " " + userdata.LastName + " " + "Enjoy your day";
                    }
                    else
                    {
                        response.Code = (int)HttpStatusCode.NotFound;
                    }
                }
            }
            catch (Exception ex)
            {
                response.Message = "Error in wishing User's Birthday";
                response.Code = (int)HttpStatusCode.InternalServerError;
            }
            return response;
        }

        public async Task<IEnumerable<EmpDetailsView>> UserEdit()
        {
            IEnumerable<EmpDetailsView> UserEdit = from e in Context.TblUsers
                                                   join d in Context.TblDepartments on e.DepartmentId equals d.Id
                                                   join c in Context.TblCountries on e.CountryId equals c.Id
                                                   join s in Context.TblStates on e.StateId equals s.Id
                                                   join ct in Context.TblCities on e.CityId equals ct.Id
                                                   select new EmpDetailsView
                                                   {
                                                       Id = e.Id,
                                                       IsActive = e.IsActive,
                                                       UserName = e.UserName,
                                                       FirstName = e.FirstName,
                                                       LastName = e.LastName,
                                                       Image = e.Image,
                                                       Gender = e.Gender,
                                                       DateOfBirth = e.DateOfBirth,
                                                       Email = e.Email,
                                                       PhoneNumber = e.PhoneNumber,
                                                       Address = e.Address,
                                                       CityName = ct.City,
                                                       StateName = s.State,
                                                       CountryName = c.Country,
                                                       DepartmentName = d.Department
                                                   };
            return UserEdit;
        }

        public async Task<EmpDetailsView> GetEmployeeById(Guid Userid)
        {
            try
            {
                string dbConnectionStr = _configuration.GetConnectionString("EMPDbconn");
                var sqlPar = new SqlParameter[]
                {
                    new SqlParameter("@Userid", Userid),
                };

                var DS = DbHelper.GetDataSet("[GetEmployeeById]", System.Data.CommandType.StoredProcedure, sqlPar, dbConnectionStr);

                EmpDetailsView Userdata = new EmpDetailsView();


                if (DS != null && DS.Tables.Count > 0)
                {
                    if (DS.Tables[0].Rows.Count > 0)
                    {
                        DataRow row = DS.Tables[0].Rows[0];

                        Userdata.Id = row["Id"] != DBNull.Value ? (Guid)row["Id"] : Guid.Empty;
                        Userdata.IsActive = row["IsActive"] != DBNull.Value ? (bool)row["IsActive"] : false; ;
                        Userdata.UserName = row["UserName"]?.ToString();
                        Userdata.FirstName = row["FirstName"]?.ToString();
                        Userdata.LastName = row["LastName"]?.ToString();
                        Userdata.Image = row["Image"]?.ToString();
                        Userdata.Gender = row["Gender"]?.ToString();
                        Userdata.DateOfBirth = row["DateOfBirth"] != DBNull.Value ? (DateTime)row["DateOfBirth"] : DateTime.MinValue; ;
                        Userdata.Email = row["Email"]?.ToString();
                        Userdata.PhoneNumber = row["PhoneNumber"]?.ToString();
                        Userdata.Address = row["Address"]?.ToString();
                        Userdata.CityName = row["CityName"]?.ToString();
                        Userdata.StateName = row["StateName"]?.ToString();
                        Userdata.CountryName = row["CountryName"]?.ToString();
                        Userdata.DepartmentName = row["DepartmentName"]?.ToString();
                        Userdata.JoiningDate = row["JoiningDate"] != DBNull.Value ? (DateTime)row["JoiningDate"] : DateTime.MinValue; ;
                        Userdata.Pincode = row["Pincode"]?.ToString();
                        Userdata.Designation = row["Designation"]?.ToString();
                        Userdata.DepartmentId = row["DepartmentId"] != DBNull.Value ? (Int32)row["DepartmentId"] : 0;
                        Userdata.CityId = row["CityId"] != DBNull.Value ? (Int32)row["CityId"] : 0;
                        Userdata.StateId = row["StateId"] != DBNull.Value ? (Int32)row["StateId"] : 0;
                        Userdata.CountryId = row["CountryId"] != DBNull.Value ? (Int32)row["CountryId"] : 0;
                        Userdata.RoleId = row["RoleId"] != DBNull.Value ? (Guid)row["RoleId"] : Guid.Empty; ;
                        Userdata.ProjectId = row["ProjectId"] != DBNull.Value ? (Guid)row["ProjectId"] : Guid.Empty;
                    };
                }
                return Userdata;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserResponceModel> UpdateUserDetails(UserEditViewModel employee)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var Userdata = await Context.TblUsers.FirstOrDefaultAsync(a => a.Id == employee.Id);
                if (Userdata != null)
                {
                    Userdata.Id = employee.Id;
                    Userdata.FirstName = employee.FirstName;
                    Userdata.LastName = employee.LastName;
                    Userdata.Gender = employee.Gender;
                    Userdata.DateOfBirth = employee.DateOfBirth;
                    Userdata.Email = employee.Email;
                    Userdata.PhoneNumber = employee.PhoneNumber;
                    Userdata.Address = employee.Address;
                    Userdata.DepartmentId = employee.DepartmentId;
                    Userdata.RoleId = employee.RoleId;
                    Userdata.UpdatedBy = employee.UpdatedBy;
                    Userdata.UpdatedOn = DateTime.Now;

                    Context.TblUsers.Update(Userdata);
                    await Context.SaveChangesAsync();
                }
                response.Message = "User data updated successfully";
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "Error in upadting user deatils.";
            }
            return response;
        }

        public async Task<IEnumerable<EmpDetailsView>> GetUsersNameList()
        {
            try
            {
                string dbConnectionStr = _configuration.GetConnectionString("EMPDbconn");

                var DS = DbHelper.GetDataSet("GetActiveDeactiveUserList", CommandType.StoredProcedure, new SqlParameter[] { }, dbConnectionStr);

                List<EmpDetailsView> UserList = new List<EmpDetailsView>();

                if (DS != null && DS.Tables.Count > 0)
                {
                    foreach (DataRow row in DS.Tables[0].Rows)
                    {
                        var isActive = row["IsActive"] != DBNull.Value && (bool)row["IsActive"];
                        if (isActive)
                        {
                            var userDetails = new EmpDetailsView
                            {
                                Id = row["Id"] != DBNull.Value ? (Guid)row["Id"] : Guid.Empty,
                                FirstName = row["FirstName"]?.ToString(),
                                LastName = row["LastName"]?.ToString(),
                                UserName = row["UserName"]?.ToString(),
                            };
                            UserList.Add(userDetails);
                        }
                    }
                }
                return UserList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<EmpDetailsView>> GetUsersDetails()
        {
            try
            {
                string dbConnectionStr = _configuration.GetConnectionString("EMPDbconn");

                var DS = DbHelper.GetDataSet("GetActiveDeactiveUserList", CommandType.StoredProcedure, new SqlParameter[] { }, dbConnectionStr);

                List<EmpDetailsView> UserList = new List<EmpDetailsView>();

                if (DS != null && DS.Tables.Count > 0)
                {
                    foreach (DataRow row in DS.Tables[0].Rows)
                    {
                        var userDetails = new EmpDetailsView
                        {
                            Id = row["Id"] != DBNull.Value ? (Guid)row["Id"] : Guid.Empty,
                            Image = row["Image"]?.ToString(),
                            Gender = row["Gender"]?.ToString(),
                            FirstName = row["FirstName"]?.ToString(),
                            LastName = row["LastName"]?.ToString(),
                            Email = row["Email"]?.ToString(),
                            PhoneNumber = row["PhoneNumber"]?.ToString(),
                            Address = row["Address"]?.ToString(),
                            CityName = row["CityName"]?.ToString(),
                            CountryName = row["CountryName"]?.ToString(),
                            StateName = row["StateName"]?.ToString(),
                            RoleName = row["RoleName"]?.ToString(),
                            IsActive = (bool)row["IsActive"],
                            RoleId = row["RoleId"] != DBNull.Value ? (Guid)row["RoleId"] : Guid.Empty,
                            DepartmentId = row["DepartmentId"] != DBNull.Value ? (int)row["DepartmentId"] : 0,
                            DepartmentName = row["DepartmentName"]?.ToString(),
                            DateOfBirth = row["DateOfBirth"] != DBNull.Value ? (DateTime)row["DateOfBirth"] : DateTime.MinValue

                        };
                        UserList.Add(userDetails);
                    }
                }
                return UserList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<EmpDetailsView>> GetSearchEmpList(EmpDetailsModel EmpList)
        {
            try
            {
                IEnumerable<EmpDetailsView> empDetails = null;
                if (EmpList.Id != null)
                {
                    empDetails = from a in Context.TblUsers
                                 join b in Context.TblDepartments on a.DepartmentId equals b.Id
                                 where (a.Id == EmpList.Id)
                                 select new EmpDetailsView
                                 {
                                     UserName = a.FirstName + ' ' + a.LastName,
                                     DepartmentName = b.Department,
                                     Id = a.Id,
                                     Image = a.Image,
                                     IsActive = a.IsActive,
                                     Gender = a.Gender,
                                     DateOfBirth = a.DateOfBirth,
                                     Email = a.Email,
                                     PhoneNumber = a.PhoneNumber,
                                 };
                    return empDetails;
                }
                else
                {
                    if (EmpList.DepartmentId != null)
                    {
                        empDetails = from a in Context.TblUsers
                                     join b in Context.TblDepartments on a.DepartmentId equals b.Id
                                     where (a.DepartmentId == EmpList.DepartmentId)
                                     select new EmpDetailsView
                                     {
                                         UserName = a.FirstName + ' ' + a.LastName,
                                         DepartmentName = b.Department,
                                         Id = a.Id,
                                         Image = a.Image,
                                         IsActive = a.IsActive,
                                         Gender = a.Gender,
                                         DateOfBirth = a.DateOfBirth,
                                         Email = a.Email,
                                         PhoneNumber = a.PhoneNumber,
                                     };
                        return empDetails;
                    }
                    else
                    {
                        empDetails = from a in Context.TblUsers
                                     join b in Context.TblDepartments on a.DepartmentId equals b.Id
                                     join c in Context.TblCountries on a.CountryId equals c.Id
                                     join s in Context.TblStates on a.StateId equals s.Id
                                     join ct in Context.TblCities on a.CityId equals ct.Id
                                     select new EmpDetailsView
                                     {
                                         Id = a.Id,
                                         IsActive = a.IsActive,
                                         UserName = a.UserName,
                                         FirstName = a.FirstName,
                                         LastName = a.LastName,
                                         Image = a.Image,
                                         Gender = a.Gender,
                                         DateOfBirth = a.DateOfBirth,
                                         Email = a.Email,
                                         PhoneNumber = a.PhoneNumber,
                                         Address = a.Address,
                                         CityName = ct.City,
                                         StateName = s.State,
                                         CountryName = c.Country,
                                         DepartmentName = b.Department
                                     };
                        return empDetails;
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<EmpDetailsView>> GetActiveDeactiveUserList()
        {
            try
            {
                string dbConnectionStr = _configuration.GetConnectionString("EMPDbconn");

                var DS = DbHelper.GetDataSet("GetActiveDeactiveUserList", CommandType.StoredProcedure, new SqlParameter[] { }, dbConnectionStr);

                List<EmpDetailsView> UserList = new List<EmpDetailsView>();

                if (DS != null && DS.Tables.Count > 0)
                {
                    foreach (DataRow row in DS.Tables[0].Rows)
                    {
                        var userDetails = new EmpDetailsView
                        {
                            Id = row["Id"] != DBNull.Value ? (Guid)row["Id"] : Guid.Empty,
                            Image = row["Image"]?.ToString(),
                            Gender = row["Gender"]?.ToString(),
                            FirstName = row["FirstName"]?.ToString(),
                            LastName = row["LastName"]?.ToString(),
                            Email = row["Email"]?.ToString(),
                            PhoneNumber = row["PhoneNumber"]?.ToString(),
                            Address = row["Address"]?.ToString(),
                            CityName = row["CityName"]?.ToString(),
                            CountryName = row["CountryName"]?.ToString(),
                            StateName = row["StateName"]?.ToString(),
                            RoleName = row["RoleName"]?.ToString(),
                            IsActive = (bool)row["IsActive"],
                            RoleId = row["RoleId"] != DBNull.Value ? (Guid)row["RoleId"] : Guid.Empty,
                            DepartmentId = row["DepartmentId"] != DBNull.Value ? (int)row["DepartmentId"] : 0,
                            DepartmentName = row["DepartmentName"]?.ToString(),
                            DateOfBirth = row["DateOfBirth"] != DBNull.Value ? (DateTime)row["DateOfBirth"] : DateTime.MinValue

                        };
                        UserList.Add(userDetails);
                    }
                }
                return UserList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<UserResponceModel> UpdateUserExeperience(EmpDetailsView UpdateDate)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var Userdata = await Context.TblUsers.FirstOrDefaultAsync(a => a.Id == UpdateDate.Id);
                if (Userdata != null)
                {
                    Userdata.LastDate = UpdateDate.LastDate;
                    Userdata.IsActive = false;

                    Context.TblUsers.Update(Userdata);
                    await Context.SaveChangesAsync();
                }
                response.Message = "User data updated successfully";
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "Error in upadting user deatils.";
            }
            return response;
        }

        public async Task<UserResponceModel> UserProfilePhoto(EmpDetailsView Profile)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var UploadImage = await Context.TblUsers.FirstOrDefaultAsync(a => a.Id == Profile.Id);
                if (UploadImage != null)
                {
                    UploadImage.Image = Profile.Image;

                    Context.TblUsers.Update(UploadImage);
                    await Context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                response.Code = (int)HttpStatusCode.InternalServerError;
                response.Message = "Error in upadting user deatils.";
            }
            return response;
        }
    }
}



