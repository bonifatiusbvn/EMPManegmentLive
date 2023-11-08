using EMPManegment.EntityModels.ViewModels;
using EMPManegment.Inretface.Interface.CSC;
using EMPManegment.Inretface.Services.CSC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Services.CSC
{
    public class CSCService : ICSCServices
    {
        public CSCService(ICSC cSC) 
        {
            CSC = cSC;
        }

        public ICSC CSC { get; }

        public async Task<IEnumerable<CityView>> GetCities(int cityId)
        {
            try
            {
               return await CSC.GetCities(cityId);
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
                return CSC.GetCountries();
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
                return CSC.GetQuestion();

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
                return CSC.GetStates(stateId);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
