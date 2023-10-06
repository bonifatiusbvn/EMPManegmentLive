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

        public async Task<string> ActiveDeactiveUsers(string UserName)
        {
            var data = Context.TblUsers.Where(a => a.UserName == UserName).FirstOrDefault();

            if (data != null)
            {

                if(data.IsActive == true)
                {
                    data.IsActive = false;
                    Context.TblUsers.Update(data);
                    Context.SaveChanges();
                }

                else
                {
                    data.IsActive = true;
                    Context.TblUsers.Update(data);
                    Context.SaveChanges();
                }


            }
            return UserName;   
        }
    }
}
