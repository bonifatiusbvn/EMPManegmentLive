
using EMPManagment.API;

using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.Models;
using EMPManegment.Inretface.EmployeesInterface.AddEmployee;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace EMPManegment.Repository.AddEmpRepository
{
    public class AddEmpRepo : IAddEmpDetails
    {

        public AddEmpRepo(BonifatiusEmployeesContext context)
        {
            Context = context;
        }

        public BonifatiusEmployeesContext Context { get; }

        public async Task<UserResponceModel> AddEmployee(EmpDetailsView addemp)
        {
            UserResponceModel response = new UserResponceModel();
            try
            {
                bool isEmailAlredyExists = Context.TblUsers.Any(x => x.Email == addemp.Email);
                if (isEmailAlredyExists == true)
                {
                    response.Message = "User with this email already exists";
                    response.Code = (int)HttpStatusCode.NotFound;
                }
                else
                {
                    var model = new TblUser()
                    {
                        Id = Guid.NewGuid(),
                        UserName = addemp.UserName,
                        DepartmentId = addemp.DepartmentId,
                        FirstName = addemp.FirstName,
                        LastName = addemp.LastName,
                        Address = addemp.Address,
                        CityId = addemp.CityId,
                        StateId = addemp.StateId,
                        CountryId = addemp.CountryId,
                        DateOfBirth = addemp.DateOfBirth,
                        Email = addemp.Email,
                        Gender = addemp.Gender,
                        PhoneNumber = addemp.PhoneNumber,
                        CreatedOn = DateTime.Now,
                        PasswordHash = addemp.PasswordHash,
                        PasswordSalt = addemp.PasswordSalt,   
                        Image = addemp.Image,
                        IsActive = true,
                        JoiningDate = DateTime.Now,
                        IsAdmin = false,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,

                    };
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


        public string CheckEmloyess()
        {
            try
            {
                var LastEmp = Context.TblUsers.OrderByDescending(e => e.CreatedOn).FirstOrDefault();
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
    }   
}
