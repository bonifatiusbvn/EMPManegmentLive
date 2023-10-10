﻿using EMPManagment.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.Inretface.EmployeesInterface.AddEmployee;
using EMPManegment.Inretface.Services.AddEmployeeServies;
using EMPManegment.Repository.AddEmpRepository;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Services.AddEmployee
{
    public class EmpService : IAddEmpDetailsServices
    {
        public IAddEmpDetails EmpDetails { get; }
        public EmpService(IAddEmpDetails _empDetails)
        {
            EmpDetails = _empDetails;
        }

        public async Task<EmpDetailsResponseModel> AddEmployee(EmpDetailsView emp)
        {
            return await EmpDetails.AddEmployee(emp);
        }

        public string CheckEmloyess()
        {
            return EmpDetails.CheckEmloyess();
        }

        public async Task<IEnumerable<Department>> EmpDepartment()
        {
            return await EmpDetails.EmpDepartment();
        }

        

       
        public async Task<EmpDetailsView> GetById(string UserName)
        {
            return await EmpDetails.GetById(UserName);
        }

        public async Task<IEnumerable<EmpDocumentView>> EmpDocument()
        {
            return await EmpDetails.EmpDocument();
        }
    }
}
