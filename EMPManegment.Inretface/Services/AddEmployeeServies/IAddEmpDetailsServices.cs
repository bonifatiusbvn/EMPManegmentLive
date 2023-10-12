
using EMPManagment.API;
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
        Task<EmpDetailsView> GetById(Guid Id);
        string CheckEmloyess();
        Task<EmpDetailsResponseModel> AddEmployee(EmpDetailsView emp);
        Task<IEnumerable<Department>> EmpDepartment();
       
    }
}
