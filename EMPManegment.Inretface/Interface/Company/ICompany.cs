﻿using EMPManagment.Web.Models.API;
using EMPManegment.EntityModels.ViewModels.Company;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.Interface.CompanyMaster
{
    public interface ICompany
    {
        Task<IEnumerable<CompanyModel>> GetCompanyNameList();
        Task<CompanyModel> GetCompanyDetailsById(Guid CompanyId);
        Task<ApiResponseModel> AddCompany(CompanyModel AddCompany);
    }
}
