using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels
{
    public class UserEditViewModel
    {
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Gender { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime? JoiningDate { get; set; }
        public string? Designation { get; set; }
        public string? Email { get; set; }
        public string? Address { get; set; }
        
        public int? CityId { get; set; }

        public string? PhoneNumber { get; set; }
        public int? DepartmentId { get; set; }

        public DateTime? CreatedOn { get; set; }
       
        public int? StateId { get; set; }
        
        public int? CountryId { get; set; }
        public Guid? RoleId { get; set; }

    }
}
