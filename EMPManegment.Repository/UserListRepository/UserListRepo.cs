using EMPManagment.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.Inretface.Interface.UserList;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Repository.UserListRepository
{
    public class UserListRepo : IUserList
    {
        public BonifatiusEmployeesContext Context { get; }
        public IWebHostEnvironment Environment { get; }
        public UserListRepo(BonifatiusEmployeesContext context, IWebHostEnvironment environment)
        {
            Context = context;
            Environment = environment;
        }


        public async Task<IEnumerable<EmpDetailsView>> GetUsersList()
        {
            IEnumerable<EmpDetailsView> data = Context.TblUsers.ToList().Select(a => new EmpDetailsView
            {
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
            });
            return data;
        }
    }
}
