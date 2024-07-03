using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.UserModels;
using EMPManegment.Inretface.Interface.CSC;
using EMPManegment.Inretface.Services.CSC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Services.CSC
{
    public class MasterListService : IMasterListServices
    {
        public IMasterList MasterList { get; }

        public MasterListService(IMasterList masterList) 
        {
            MasterList = masterList;
        }

      

        public async Task<IEnumerable<CityView>> GetCities(int cityId)
        {
            try
            {
               return await MasterList.GetCities(cityId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Task<IEnumerable<CountryView>> GetCountries()
        {
            try
            {
                return MasterList.GetCountries();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Task<IEnumerable<QuestionView>> GetQuestion()
        {
            try
            {
                return MasterList.GetQuestion();

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Task<IEnumerable<StateView>> GetStates(int stateId)
        {
            try
            {
                return MasterList.GetStates(stateId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public async Task<IEnumerable<Department>> GetDepartment()
        {
            return await MasterList.GetDepartment();
        }

        public async Task<IEnumerable<UserRoleModel>> GetUserRole()
        {
            try
            {
                return await MasterList.GetUserRole();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public async Task<IEnumerable<CityView>> GetAllCities()
        {
            try
            {
                return await MasterList.GetAllCities();
            }
            catch(Exception ex) 
            {
                throw ex;
            }
        }
    }
}
