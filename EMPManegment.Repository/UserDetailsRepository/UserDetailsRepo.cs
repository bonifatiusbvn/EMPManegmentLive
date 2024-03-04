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
using System.Linq;
using System.Net;
using System.Linq.Dynamic.Core;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using EMPManegment.EntityModels.ViewModels.UserModels;

namespace EMPManegment.Repository.UserListRepository
{
    public class UserDetailsRepo : IUserDetails
    {
        public BonifatiusEmployeesContext Context { get; }
        
        public UserDetailsRepo(BonifatiusEmployeesContext context)
        {
            Context = context;
           
        }


        public async Task<jsonData> GetUsersList(DataTableRequstModel dataTable)
        {

            var GetUsersList = from e in Context.TblUsers
                                                 join d in Context.TblDepartments on e.DepartmentId equals d.Id
                                                 join c in Context.TblCountries on e.CountryId equals c.Id
                                                 join s in Context.TblStates on e.StateId equals s.Id
                                                 join ct in Context.TblCities on e.CityId equals ct.Id
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
                                                     DepartmentName = d.Department
                                                 };

            if(!string.IsNullOrEmpty(dataTable.sortColumn) && !string.IsNullOrEmpty(dataTable.sortColumnDir))
            {
                GetUsersList = GetUsersList.OrderBy(dataTable.sortColumn + " " + dataTable.sortColumnDir);
            }

            if (!string.IsNullOrEmpty(dataTable.searchValue))
            {
                GetUsersList = GetUsersList.Where(e=>e.UserName.Contains(dataTable.searchValue) || e.DepartmentName.Contains(dataTable.searchValue) || e.Gender.Contains(dataTable.searchValue));
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
        
        public async Task<UserResponceModel> ActiveDeactiveUsers(string UserName)
        {
            UserResponceModel response = new UserResponceModel();
            var GetUserdta = Context.TblUsers.Where(a => a.UserName == UserName).FirstOrDefault();

            if (GetUserdta != null)
            {

                if (GetUserdta.IsActive == true)
                {
                    GetUserdta.IsActive = false;
                    Context.TblUsers.Update(GetUserdta);
                    Context.SaveChanges();
                    response.Code = 200;
                    response.Data = GetUserdta; 
                    response.Message = "User" + " "+ GetUserdta.UserName +" " +"Is Deactive Succesfully";
                }

                else
                {
                    GetUserdta.IsActive = true;
                    Context.TblUsers.Update(GetUserdta);
                    Context.SaveChanges();
                    response.Code = 200;
                    response.Data = GetUserdta;
                    response.Message = "User" + " "+ GetUserdta.UserName + " "+ "Is Active Succesfully";
                }


            }
            return response;
        }

        public async Task<UserResponceModel> EnterInTime(UserAttendanceModel userAttendance)
        {
            UserResponceModel response = new UserResponceModel();
            var Intimedata = Context.TblAttendances.Where(a => a.UserId == userAttendance.UserId).OrderByDescending(a => a.Date).FirstOrDefault();

            if (Intimedata != null)
            {
                if (Intimedata.OutTime == null && Intimedata.Date != DateTime.Today)
                {
                    response.Message = "You Missed Out-Time of " + Intimedata.Date.ToString("dd/MM/yyyy") + " " + "Kindly Contact Your Admin";
                    response.Icone = "warning";
                } 
                

               else 
                {
                    if (Intimedata.Date == DateTime.Today && Intimedata.Intime != null)
                    {
                        response.Message = "Your Already Enter IN-Time";
                        response.Icone = "warning";
                    }

                    else
                    {
                        TblAttendance tblAttendance = new TblAttendance();
                        tblAttendance.UserId = userAttendance.UserId;
                        tblAttendance.Intime = DateTime.Now;
                        tblAttendance.Date = DateTime.Today;
                        tblAttendance.CreatedOn = DateTime.Now;
                        tblAttendance.OutTime = null;
                        Context.TblAttendances.Add(tblAttendance);
                        Context.SaveChanges();
                        response.Code = 200;
                        response.Message = "In-Time Enter Successfully";
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
                tblAttendance.OutTime = null;
                Context.TblAttendances.Add(tblAttendance);
                Context.SaveChanges();
                response.Code = 200;
                response.Message = "In-Time Enter Successfully";
                response.Icone = "success";

            }

            return response;
        }

        public async Task<UserResponceModel> EnterOutTime(UserAttendanceModel userAttendance)
        {
            UserResponceModel response = new UserResponceModel();
            var Outtimedata = Context.TblAttendances.Where(a => a.UserId == userAttendance.UserId).OrderByDescending(a => a.Date).FirstOrDefault();;
            if (Outtimedata.OutTime == null && Outtimedata.Date != DateTime.Today)
            {
                response.Message = "You Missed Out-Time of " + Outtimedata.Date.ToString("dd/MM/yyyy") + " " + "Kindly Contact Your Admin";
                response.Icone = "warning";

            }

           else
            {
               if(Outtimedata.Date != DateTime.Today)
                {
                    response.Message = "Please Enter In-Time First";
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
                        response.Code = 200;
                        response.Message = "Out-Time Enter Successfully";
                        response.Icone = "success";

                    }
                    else
                    {
                        response.Message = "Your Already Enter Out-Time";
                        response.Icone = "warning";

                    }

                }


                }

            return response;
        }

        public async Task<UserResponceModel> ResetPassword(PasswordResetView resetemppass)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var userdata = Context.TblUsers.FirstOrDefault(x => x.UserName == resetemppass.UserName);
                if(userdata != null) 
                {

                    userdata.PasswordHash = resetemppass.PasswordHash;
                    userdata.PasswordSalt = resetemppass.PasswordSalt;
                }
                Context.TblUsers.Update(userdata);
                Context.SaveChanges();
                response.Code=200;
                response.Message = "Password Updated!";
            }
            catch (Exception ex) 
            {
                throw ex;
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
                                                             join c in Context.TblDocumentMasters on a.DocumentTypeId equals c.Id where b.Id==Userid
                                                             select new DocumentInfoView
                                                             {
                                                                 Id = a.Id,
                                                                 UserId = a.UserId,
                                                                 DocumentType = c.DocumentType,                                                                                                                        
                                                                 DocumentName = a.DocumentName,
                                                                 CreatedOn = DateTime.Now,
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
                CreatedBy= docview.CreatedBy,
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
                if(tblUser != null)
                {
                    if (tblUser.UserName == loginrequest.UserName && Crypto.VarifyHash(loginrequest.Password, tblUser.PasswordHash, tblUser.PasswordSalt))
                    {
                        LoginView userModel = new LoginView();
                        userModel.UserName = tblUser.UserName;
                        response.Code = (int)HttpStatusCode.OK;
                    }
                    else
                    {
                        response.Message = "Your Password is Wrong";
                    }
                }
            }catch (Exception ex)
            { 
                throw ex;
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
                        response.Message = "Bonifatius Wish You a Very Happy Birthday.." +" " + userdata.FirstName + " " + userdata.LastName +" "+ "Enjoy Your Day";
                        response.Code = (int)HttpStatusCode.OK; 
                    }
                    else
                    {
                        response.Code = (int)HttpStatusCode.OK;

                    }

                }
                
                return response;

            }
            catch (Exception ex)
            {

                throw ex;
            }
            
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

        public async Task<EmpDetailsView> GetById(Guid Userid)
        {
            var Userdata = await Context.TblUsers.SingleOrDefaultAsync(x => x.Id == Userid);
            EmpDetailsView model = new EmpDetailsView
            {
                Id = Userdata.Id,
                UserName = Userdata.UserName,
                FirstName = Userdata.FirstName,
                LastName = Userdata.LastName,
                Image = Userdata.Image,
                Gender = Userdata.Gender,
                DateOfBirth = Userdata.DateOfBirth,
                Email = Userdata.Email,
                PhoneNumber = Userdata.PhoneNumber,
                Address = Userdata.Address,
                CityId = Userdata.CityId,
                DepartmentId = Userdata.DepartmentId,
                StateId = Userdata.StateId,
                CountryId = Userdata.CountryId,
                IsActive = Userdata.IsActive,
                JoiningDate = Userdata.JoiningDate,
            };
            return model;
        }

        public async Task<UserResponceModel> UpdateUser(UserEditViewModel employee)
        {
            try
            {
                UserResponceModel response = new UserResponceModel();
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
                    Userdata.CreatedOn = DateTime.Now;

                    Context.TblUsers.Update(Userdata);
                    await Context.SaveChangesAsync();
                }
                response.Code = (int)HttpStatusCode.OK;
                response.Message = "User Data Updated Successfully";
                return response;
                
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<EmpDetailsView>> GetUsersNameList()
        {
            IEnumerable<EmpDetailsView> GetUserNameList = Context.TblUsers.ToList().Select(a => new EmpDetailsView
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
    }
}

      

