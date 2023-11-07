﻿using EMPManegment.EntityModels.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.Services.CSC
{
    public interface ICSCServices
    {
        Task<IEnumerable<CountryView>> GetCountries();
        Task<IEnumerable<StateView>> GetStates(int StateId);
        Task<IEnumerable<CityView>> GetCities(int CityId);
        Task<IEnumerable<QuestionView>> GetQuestion();
    }
}
