using EMPManegment.EntityModels.ViewModels;
using EMPManegment.Inretface.Interface.CSC;
using EMPManegment.Services.CSC;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EMPManagment.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CSCController : ControllerBase
    {
        public CSCController(ICSC cSC)
        {
            CSC = cSC;
        }

        public ICSC CSC { get; }

        [HttpGet]
        [Route("GetCountries")]
        public async Task<IActionResult> GetCountries()
        {
            IEnumerable<CountryView> getCountries = await CSC.GetCountries();
            return Ok(new { code = 200, data = getCountries.ToList() });
        }



        [HttpGet]
        [Route("GetState")]
        public async Task<IActionResult> GetState(int StateId)
        {
            IEnumerable<StateView> getStates = await CSC.GetStates(StateId);
            return Ok(new { code = 200, data = getStates.ToList() });
        }

        [HttpGet]
        [Route("GetCities")]
        public async Task<IActionResult> GetCities(int CityId)
        {
            IEnumerable<CityView> getCities = await CSC.GetCities(CityId);
            return Ok(new { code = 200, data = getCities.ToList() });
        }

        [HttpGet]
        [Route("GetQuestion")]
        public async Task<IActionResult> GetQuestion()
        {
            IEnumerable<QuestionView> getQuestion = await CSC.GetQuestion();
            return Ok(new { code = 200, data = getQuestion.ToList() });
        }

    }
}
