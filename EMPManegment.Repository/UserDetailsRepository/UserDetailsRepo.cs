using EMPManagment.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.Inretface.Interface.UserList;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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


        public async Task<IEnumerable<EmpDetailsView>> GetUsersList()
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
                    data.CityId = employee.CityId;
                    data.DepartmentId = employee.DepartmentId;
                    data.StateId = employee.StateId;
                    data.CountryId = employee.CountryId;
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

