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

            var GetUsersList = from e in Context.TblUsers
                               join d in Context.TblDepartments on e.DepartmentId equals d.Id
                               join c in Context.TblCountries on e.CountryId equals c.Id
                               join s in Context.TblStates on e.StateId equals s.Id
                               join ct in Context.TblCities on e.CityId equals ct.Id
                               join r in Context.TblRoleMasters on e.RoleId equals r.RoleId
                               select new UserDataTblModel
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
                                   DepartmentName = d.Department,
                                   RoleId = e.RoleId,
                                   RoleName = r.Role,
                                   FullName = e.FirstName + " " + e.LastName,
                               };

            if (!string.IsNullOrEmpty(dataTable.sortColumn) && !string.IsNullOrEmpty(dataTable.sortColumnDir))
            {
                GetUsersList = GetUsersList.OrderBy(dataTable.sortColumn + " " + dataTable.sortColumnDir);
            }

            if (!string.IsNullOrEmpty(dataTable.searchValue))
            {
                GetUsersList = GetUsersList.Where(e => e.UserName.Contains(dataTable.searchValue) || e.DepartmentName.Contains(dataTable.searchValue) || e.Gender.Contains(dataTable.searchValue) || e.Email.Contains(dataTable.searchValue) || e.FullName.Contains(dataTable.searchValue));
            }

            int totalRecord = GetUsersList.Count();

            var cData = GetUsersList.Skip(dataTable.skip).Take(dataTable.pageSize).ToList();

            jsonData jsonData = new jsonData
            {
                draw = dataTable.draw,
                recordsFiltered = totalRecord,
                recordsTotal = totalRecord,
                data = cData
            };


            return jsonData;


        }

        public async Task<UserResponceModel> ActiveDeactiveUsers(string UserName, Guid UpdatedBy)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var GetUserdta = Context.TblUsers.Where(a => a.UserName == UserName).FirstOrDefault();

                if (GetUserdta != null)
                {

                    if (GetUserdta.IsActive == true)
                    {
                        GetUserdta.IsActive = false;
                        GetUserdta.UpdatedOn = DateTime.Now;
                        GetUserdta.UpdatedBy = UpdatedBy;
                        Context.TblUsers.Update(GetUserdta);
                        Context.SaveChanges();
                        response.Code = 200;
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
                IEnumerable<DocumentInfoView> DocumentList = from a in Context.TblUserDocuments
                                                             join b in Context.TblUsers on a.User.Id equals b.Id
                                                             join c in Context.TblDocumentMasters on a.DocumentTypeId equals c.Id
                                                             where b.Id == Userid
                                                             select new DocumentInfoView
                                                             {
                                                                 Id = a.Id,
                                                                 UserId = a.UserId,
                                                                 DocumentType = c.DocumentType,
                                                                 DocumentName = a.DocumentName,
                                                                 CreatedOn = a.CreatedOn,
                                                                 CreatedBy = a.CreatedBy,
                                                             };
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
                        response.Code = (int)HttpStatusCode.OK;
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
                        response.Code = (int)HttpStatusCode.OK;
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
                response.Code = (int)HttpStatusCode.OK;
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
            IEnumerable<EmpDetailsView> GetUserNameList = Context.TblUsers.Where(a => a.IsActive == true).ToList().Select(a => new EmpDetailsView
            {
                Id = a.Id,
                UserName = a.UserName,
                FirstName = a.FirstName,
                LastName = a.LastName,

            }).ToList();
            return GetUserNameList;
        }

        public async Task<IEnumerable<EmpDetailsView>> GetUsersDetails()
        {
            IEnumerable<EmpDetailsView> GetUsersList = from e in Context.TblUsers
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
            return GetUsersList;
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
            IEnumerable<EmpDetailsView> GetUsersList = from e in Context.TblUsers
                                                       join d in Context.TblDepartments on e.DepartmentId equals d.Id
                                                       join c in Context.TblCountries on e.CountryId equals c.Id
                                                       join s in Context.TblStates on e.StateId equals s.Id
                                                       join ct in Context.TblCities on e.CityId equals ct.Id
                                                       join r in Context.TblRoleMasters on e.RoleId equals r.RoleId
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
                                                           DepartmentName = d.Department,
                                                           DepartmentId = e.DepartmentId,
                                                           RoleId = e.RoleId,
                                                           RoleName = r.Role,
                                                       };
            return GetUsersList;
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
                response.Code = (int)HttpStatusCode.OK;
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
                response.Code = (int)HttpStatusCode.OK;
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



