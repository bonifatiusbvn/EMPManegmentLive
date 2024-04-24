using EMPManagment.API;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.UserModels;
using EMPManegment.Inretface.Interface.CSC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Repository.CSCRepository
{
    public class MasterListRepo : IMasterList
    {
        public MasterListRepo (BonifatiusEmployeesContext context)
        {
            Context = context;
        }

        public BonifatiusEmployeesContext Context { get; }

        public async Task<IEnumerable<CityView>> GetCities(int cityid)
        {
            try
            {
                IEnumerable<CityView> cities = Context.TblCities.Where(e => e.State.Id == cityid).ToList().Select(a => new CityView
                {
                    Id = a.Id,
                    CityName = a.City,
                });
                return cities;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<CountryView>> GetCountries()
        {
            try
            {
                IEnumerable<CountryView> countries = Context.TblCountries.ToList().Select(a => new CountryView
                {
                    Id = a.Id,
                    CountryName = a.Country
                });
                return countries;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<QuestionView>> GetQuestion()
        {
            try
            {
                IEnumerable<QuestionView> questions = Context.TblQuestions.ToList().Select(a => new QuestionView
                {
                    Id = a.Id,
                    Questions = a.Question
                });
                return questions;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<StateView>> GetStates(int stateid)
        {
            try
            {
                IEnumerable<StateView> states = Context.TblStates.Where(e => e.Country.Id == stateid).ToList().Select(a => new StateView
                {
                    Id = a.Id,
                    StateName = a.State
                });
                return states;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IEnumerable<Department>> GetDepartment()
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
        public async Task<IEnumerable<UserRoleModel>> GetUserRole()
        {

            try
            {
                IEnumerable<UserRoleModel> role = Context.TblRoleMasters.ToList().Select(a => new UserRoleModel
                {
                    Id = a.Id,
                    Role = a.Role,
                });
                return role;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
