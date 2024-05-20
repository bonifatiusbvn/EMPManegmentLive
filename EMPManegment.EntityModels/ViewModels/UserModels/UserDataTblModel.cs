using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels.UserModels
{
    public class UserDataTblModel
    {
        public Guid? Id { get; set; }
        public string? UserName { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Gender { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string? Email { get; set; }
        public string? Image { get; set; }
        public string? Address { get; set; }
        public bool? IsActive { get; set; }
        public string? PhoneNumber { get; set; }
        public string? CountryName { get; set; }
        public string? StateName { get; set; }
        public string? CityName { get; set; }
        public string? DepartmentName { get; set; }
        public Guid? RoleId { get; set; }
        public string? RoleName { get; set; }
    }
}
