using EMPManagment.API;
using EMPManegment.EntityModels.View_Model;
using EMPManegment.EntityModels.ViewModels;
using EMPManegment.EntityModels.ViewModels.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.Inretface.EmployeesInterface.AddEmployee
{
    public interface IAddEmpDetails
    {
        Task <EmpDetailsView> GetById(Guid UserId);
        string CheckEmloyess();
        Task <UserResponceModel> AddEmployee(EmpDetailsView AddEmployee);
        Task<IEnumerable<Department>> EmpDepartment();
       
    }
}
