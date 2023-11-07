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
        string CheckEmloyess();
        Task <UserResponceModel> AddEmployee(EmpDetailsView emp);
        Task<IEnumerable<Department>> EmpDepartment();
       
    }
}
