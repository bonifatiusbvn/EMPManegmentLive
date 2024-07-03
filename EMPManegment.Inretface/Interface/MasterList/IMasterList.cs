using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.Interface.CSC
{
    public interface IMasterList
    {
        Task<IEnumerable<CountryView>> GetCountries();
        Task<IEnumerable<QuestionView>> GetQuestion();
        Task<IEnumerable<StateView>> GetStates(int StateId);
        Task<IEnumerable<CityView>> GetCities(int CityId);
        Task<IEnumerable<Department>> GetDepartment();
        Task<IEnumerable<UserRoleModel>> GetUserRole();
        Task<IEnumerable<CityView>> GetAllCities();
    }
}
