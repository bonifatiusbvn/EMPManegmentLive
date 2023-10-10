
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
                        UserName = emp.UserName,
                        DepartmentId = emp.DepartmentId,
                        FirstName = emp.FirstName,
                        LastName = emp.LastName,
                        Address = emp.Address,
                        CityId = emp.CityId,
                        StateId = emp.StateId,
                        CountryId = emp.CountryId,
                        DateOfBirth = emp.DateOfBirth,
                        Email = emp.Email,
                        Gender = emp.Gender,
                        PhoneNumber = emp.PhoneNumber,
                        CreatedOn = DateTime.Now,
                        PasswordHash = emp.PasswordHash,
                        PasswordSalt = emp.PasswordSalt,   
                        Image = emp.Image,
                        IsActive = true,
                        JoiningDate = DateTime.Now,
                        IsAdmin = false,
                        EmailConfirmed = true,
                        PhoneNumberConfirmed = true,

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

        public async Task<IEnumerable<EmpDocumentView>> EmpDocument()
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

        public async Task<EmpDetailsView> GetById(string UserName)
        {
            var employee = await Context.TblUsers.SingleOrDefaultAsync(x => x.UserName == UserName);
            EmpDetailsView model = new EmpDetailsView
            {
                UserName = employee.UserName,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                Image = employee.Image,
                Gender = employee.Gender,
                DateOfBirth = employee.DateOfBirth,
                Email = employee.Email,
                PhoneNumber = employee.PhoneNumber,
                Address = employee.Address,
                CityId = employee.CityId,
                DepartmentId = employee.DepartmentId,
                StateId = employee.StateId,
                CountryId = employee.CountryId,
                IsActive = employee.IsActive,
                JoiningDate= employee.JoiningDate,
            };
            return model; 
        }
    }   
}
