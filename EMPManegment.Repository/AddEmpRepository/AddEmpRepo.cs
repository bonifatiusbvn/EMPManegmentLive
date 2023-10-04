
using EMPManagment.API;
using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.Inretface.EmployeesInterface.AddEmployee;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;


namespace EMPManegment.Repository.AddEmpRepository
{
    public class AddEmpRepo : IAddEmpDetails
    {

        public AddEmpRepo(BonifatiusEmployeesContext context)
        {
            Context = context;
        }

        public BonifatiusEmployeesContext Context { get; }

        public async Task<EmpDetailsResponseModel> AddEmployee(EmpDetailsView emp)
        {
            EmpDetailsResponseModel response = new EmpDetailsResponseModel();
            try
            {
                bool isEmailAlredyExists = Context.TblUsers.Any(x => x.Email == emp.Email);
                if (isEmailAlredyExists == true)
                {
                    response.Message = "User with this email already exists";
                    response.Data = emp;
                    response.Code = (int)HttpStatusCode.NotFound;
                }
                else
                {
                    var model = new TblUser()
                    {
                        Id = Guid.NewGuid(),
                        UserName = emp.EmpId,
                        DepartmentId = emp.Department,
                        FirstName = emp.FirstName,
                        LastName = emp.LastName,
                        Address = emp.Address,
                        CityId = emp.City,
                        StateId = emp.State,
                        CountryId = emp.Country,
                        DateOfBirth = emp.DateOfBirth,
                        Email = emp.Email,
                        Gender = emp.Gender,
                        PhoneNumber = emp.PhoneNumber,
                        CreatedOn = DateTime.Now,

                };
                    response.Data = emp;
                    response.Code = (int)HttpStatusCode.OK;
                    Context.TblUsers.Add(model);
                    Context.SaveChanges();
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return response;
        }

        public async Task<EmpDetailsView> AddLogin(EmpDetailsView log)
        {
            try
            {
                var data = Context.TblUsers.FirstOrDefault(a => a.UserName == log.EmpId);
                if (data != null)
                {
                    data.PasswordHash = log.PasswordHash;
                    data.PasswordSalt = log.PasswordSalt;
                    data.QuestionId = log.QuestionId;
                    data.Answer = log.Answer;
                    data.Image = log.Image;
                    data.IsActive = true;

                    Context.TblUsers.Update(data);
                    Context.SaveChanges();
                }
                return log;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string CheckEmloyess()
        {
            try
            {
                var LastEmp = Context.TblUsers.OrderByDescending(e => e.Id).SingleOrDefault();
                string UserEmpId;
                if (LastEmp == null)
                {
                    UserEmpId = "BONI-UID001";
                }
                else
                {
                    UserEmpId = "BONI-UID" + (Convert.ToUInt32(LastEmp.UserName.Substring(9, LastEmp.UserName.Length - 9)) + 1).ToString("D3");
                }
                return UserEmpId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<Department>> EmpDepartment()
        {
            try
            {
                IEnumerable<Department> dept = Context.TblDepartments.ToList().Select(a => new Department
                {
                    Id = a.Id,
                    Departments = a.Department
                });
                return dept;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Task<EmpDetailsView> GetById(string EId)
        {
            throw new NotImplementedException();
        }
        public void UploadFile(IFormFile file, string path)
        {
            throw new NotImplementedException();
        }
        

        //public EmpDetailsView AddLogin(EmpDetailsView log)
        //{
        //    try
        //    {
        //        var data = Context.TblUsers.FirstOrDefault(a => a.Id == log.Id);
        //        if (data != null)
        //        {
        //            data.Id = log.Id;
        //            data.EmpId = log.EmpId;
        //            data.PasswordHash = log.PasswordHash;
        //            data.PasswordSalt = log.PasswordSalt;
        //            data.Question = log.Question;
        //            data.Answer = log.Answer;
        //            data.Image = log.Image;
        //            data.IsActive = true;
        //            Context.TblUsers.Update(data);
        //            Context.SaveChanges();
        //        }
        //        return log;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //}

    }   
}
