using EMPManegment.EntityModels.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.View_Model
{
    public class EmpDetailsView
    {
        public Guid? Id { get; set; }

        public string? UserName { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? Gender { get; set; }

        public DateTime DateOfBirth { get; set; }
        public DateTime? JoiningDate { get; set; }
        public string? Designation { get; set; }
        public string? Email { get; set; }

        public bool? EmailConfirmed { get; set; }

        public string? Address { get; set; }

        public int? CityId { get; set; }

        public string? Pincode { get; set; }

        public byte[]? PasswordHash { get; set; }

        public byte[]? PasswordSalt { get; set; }

        public string? PhoneNumber { get; set; }

        public bool? PhoneNumberConfirmed { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public bool? IsAdmin { get; set; }

        public int? DepartmentId { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string? CreatedBy { get; set; }

        public int? StateId { get; set; }

        public int? CountryId { get; set; }

        public string? Image { get; set; }
        public string? CountryName { get; set; }
        public string? StateName { get; set; }
        public string? CityName { get; set; }
        public string? DepartmentName { get; set; }

        public string? DocumentType { get; set; }
        public Guid? RoleId { get; set; }
        public string? RoleName { get; set; }
    }
    public class EmpDetailsModel
    {
        public Guid? Id { get; set; }
        public int? DepartmentId { get; set; }
    }
}
