﻿
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.Services.AddEmployeeServies
{
    public interface IAddEmpDetailsServices
    {
        Task<EmpDetailsView> AddLogin(EmpDetailsView emp);
        Task<EmpDetailsView> GetById(string EId);
        string CheckEmloyess();
        Task<EmpDetailsResponseModel> AddEmployee(EmpDetailsView emp);
        void UploadFile(IFormFile file, string path);

        Task<IEnumerable<Department>> EmpDepartment();
    }
}
