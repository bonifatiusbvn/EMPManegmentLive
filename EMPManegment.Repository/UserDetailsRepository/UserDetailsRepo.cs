using Azure;
using EMPManagment.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.Inretface.Interface.UserList;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

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
            IEnumerable<EmpDetailsView> result = from e in Context.TblUsers
                                                 join d in Context.TblDepartments on e.DepartmentId equals d.Id
                                                 join c in Context.TblCountries on e.CountryId equals c.Id
                                                 join s in Context.TblStates on e.StateId equals s.Id
                                                 join ct in Context.TblCities on e.CityId equals ct.Id
                                                 select new EmpDetailsView
                                                 {
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
                                                     return result;
        }
       
    }
}
