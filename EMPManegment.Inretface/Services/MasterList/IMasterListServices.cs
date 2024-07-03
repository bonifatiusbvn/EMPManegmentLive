using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.Services.CSC
{
    public interface IMasterListServices
    {
        Task<IEnumerable<CountryView>> GetCountries();
        Task<IEnumerable<StateView>> GetStates(int StateId);
        Task<IEnumerable<CityView>> GetCities(int CityId);
        Task<IEnumerable<QuestionView>> GetQuestion();
        Task<IEnumerable<Department>> GetDepartment();
        Task<IEnumerable<UserRoleModel>> GetUserRole();
        Task<IEnumerable<CityView>> GetAllCities();
    }
}
