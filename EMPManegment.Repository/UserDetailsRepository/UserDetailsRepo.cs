using Azure;
using Azure.Core;
using EMPManagment.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.DataTableParameters;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.Inretface.Interface.UserAttendance;
using EMPManegment.Inretface.Interface.UserList;
using EMPManegment.Web.Helper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
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
        public IWebHostEnvironment Environment { get; }
        public UserDetailsRepo(BonifatiusEmployeesContext context, IWebHostEnvironment environment)
        {
            Context = context;
            Environment = environment;
        }


        public async Task<jsonData> GetUsersList(DataTableRequstModel dataTable)
        {

            var result = from e in Context.TblUsers
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
                result = result.OrderBy(dataTable.sortColumn + " " + dataTable.sortColumnDir);
            }

            if (!string.IsNullOrEmpty(dataTable.searchValue))
            {
                result = result.Where(e=>e.UserName.Contains(dataTable.searchValue) || e.DepartmentName.Contains(dataTable.searchValue) || e.Gender.Contains(dataTable.searchValue));
            }

            int totalRecord = result.Count();

            var cData = result.Skip(dataTable.skip).Take(dataTable.pageSize).ToList();

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
            var data = Context.TblUsers.Where(a => a.UserName == UserName).FirstOrDefault();

            if (data != null)
            {

                if (data.IsActive == true)
                {
                    data.IsActive = false;
                    Context.TblUsers.Update(data);
                    Context.SaveChanges();
                    response.Code = 200;
                    response.Data = data; 
                    response.Message = "User" + " "+ data.UserName +" " +"Is Deactive Succesfully";
                }

                else
                {
                    data.IsActive = true;
                    Context.TblUsers.Update(data);
                    Context.SaveChanges();
                    response.Code = 200;
                    response.Data = data;
                    response.Message = "User" + " "+ data.UserName + " "+ "Is Active Succesfully";
                }


            }
            return response;
        }

        public async Task<UserResponceModel> EnterInTime(UserAttendanceModel userAttendance)
        {
            UserResponceModel response = new UserResponceModel();
            var data = Context.TblAttendances.Where(a => a.UserId == userAttendance.UserId).OrderByDescending(a => a.Date).FirstOrDefault();

            if (data != null)
            {
                if (data.OutTime == null && data.Date != DateTime.Today)
                {
                    response.Message = "You Missed Out-Time of " + data.Date.ToShortDateString() + " " + "Kindly Contact Your Admin";
                    response.Icone = "warning";
                } 
                

               else 
                {
                    if (data.Date == DateTime.Today && data.Intime != null)
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
            var data = Context.TblAttendances.Where(a => a.UserId == userAttendance.UserId).OrderByDescending(a => a.Date).FirstOrDefault();;
            if (data.OutTime == null && data.Date != DateTime.Today)
            {
                response.Message = "You Missed Out-Time of " + data.Date.ToShortDateString() + " " + "Kindly Contact Your Admin";
                response.Icone = "warning";

            }

           else
            {
               if(data.Date != DateTime.Today)
                {
                    response.Message = "Please Enter In-Time First";
                    response.Icone = "warning";
                }

               else 
                {

                    if (data.Date == DateTime.Today && data.OutTime == null)
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

        public async Task<UserResponceModel> ResetPassword(PasswordResetView emp)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var data = Context.TblUsers.FirstOrDefault(x => x.UserName == emp.UserName);
                    if(data !=null) 
                {
                   
                    data.PasswordHash = emp.PasswordHash;
                    data.PasswordSalt = emp.PasswordSalt;
                }
                Context.TblUsers.Update(data);
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
                IEnumerable<EmpDocumentView> document = Context.TblDocumentMasters.ToList().Select(a => new EmpDocumentView
                {
                    Id = a.Id,
                    DocumentType = a.DocumentType,
                });
                return document;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<DocumentInfoView>> GetDocumentList()
        {
            //var document = await Context.TblUserDocuments.ToListAsync();
            //List<DocumentInfoView> model = document.Select(a => new DocumentInfoView
            //{
            //    Id = a.Id,
            //    UserId = a.UserId,
            //    DocumentTypeId = a.DocumentTypeId,
            //    DocumentName = a.DocumentName,
            //    CreatedOn = DateTime.Now,
            //    CreatedBy=a.CreatedBy,
            //}).ToList();
            //return model;

            IEnumerable<DocumentInfoView> result = from a in Context.TblUserDocuments
                                                   join b in Context.TblDocumentMasters on a.DocumentTypeId equals b.Id
                                                   select new DocumentInfoView
                                                   {
                                                       Id = a.Id,
                                                       UserId = a.UserId,
                                                       DocumentType = b.DocumentType,
                                                       DocumentName = a.DocumentName,
                                                       CreatedOn = DateTime.Now,
                                                       CreatedBy = a.CreatedBy,
                                                   };
            return result;
        }

        public async Task<DocumentInfoView> UploadDocument(DocumentInfoView doc)
        {
            var model = new TblUserDocument()
            {
                Id = doc.Id,
                UserId = doc.UserId,
                DocumentTypeId = doc.DocumentTypeId,
                DocumentName = doc.DocumentName,
                CreatedOn = DateTime.Now,
                CreatedBy= doc.CreatedBy,
            };
            Context.TblUserDocuments.Add(model);
            Context.SaveChanges();
            return doc;
        }

        public async Task<UserResponceModel> UserLockScreen(LoginRequest request)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                var tblUser = Context.TblUsers.Where(p => p.UserName == request.UserName).SingleOrDefault();
                if(tblUser != null)
                {
                    if (tblUser.UserName == request.UserName && Crypto.VarifyHash(request.Password, tblUser.PasswordHash, tblUser.PasswordSalt))
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
            var data = Context.TblUsers.Where(e => e.Id == UserId).FirstOrDefault();

            try
            {
                if (data != null)
                {

                    string today = DateTime.Now.ToString("dd/MM");
                    string checkdate = data.DateOfBirth.ToString("dd/MM");
                    if (today == checkdate)
                    {
                        response.Message = "Bonifatius Wish You a Very Happy Birthday.." +" " + data.FirstName + " " + data.LastName +" "+ "Enjoy Your Day";
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
            IEnumerable<EmpDetailsView> data = Context.TblUsers.ToList().Select(a => new EmpDetailsView
            {
                Id = a.Id,
                UserName = a.UserName,
                FirstName = a.FirstName,
                LastName = a.LastName,
                Image = a.Image,
                Gender = a.Gender,
                DateOfBirth = a.DateOfBirth,
                Email = a.Email,
                PhoneNumber = a.PhoneNumber,
                Address = a.Address,
                CityId = a.CityId,
                DepartmentId = a.DepartmentId,
                StateId = a.StateId,
                CountryId = a.CountryId,
                IsActive = a.IsActive,
            });
            return data;
        }

        public async Task<EmpDetailsView> GetById(Guid id)
        {
            var employee = await Context.TblUsers.SingleOrDefaultAsync(x => x.Id == id);
            EmpDetailsView model = new EmpDetailsView
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Gender = employee.Gender,
                DateOfBirth = employee.DateOfBirth,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                Address = employee.Address,
                CityId = employee.CityId,
                DepartmentId = employee.DepartmentId,
                StateId = employee.StateId,
                CountryId = employee.CountryId,
            };
            return model;
        }

        public async Task<Guid> UpdateUser(UserEditViewModel employee)
        {
            try
            {
                var data = await Context.TblUsers.FirstOrDefaultAsync(a => a.Id == employee.Id);
                if (data != null)
                {
                    data.Id = employee.Id;
                    data.FirstName = employee.FirstName;
                    data.LastName = employee.LastName;
                    data.Gender = employee.Gender;
                    data.DateOfBirth = employee.DateOfBirth;
                    data.Email = employee.Email;
                    data.PhoneNumber = employee.PhoneNumber;
                    data.Address = employee.Address;
                    data.DepartmentId = employee.DepartmentId;
                    data.CreatedOn = DateTime.Now;

                    Context.TblUsers.Update(data);
                    await Context.SaveChangesAsync();
                }
                return employee.Id;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}

      

