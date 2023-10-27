using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
#nullable disable
namespace EMPManegment.EntityModels.View_Model
{
    public class LoginDetailsView
    {

        public int Id { get; set; }

        public string UserName { get; set; }


        [Required(ErrorMessage = "Please Enter FirstName..!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Please Enter LastName..!")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Please Select Gender..!")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "Please Select DateOfBirth..!")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Please Enter Email..!")]
        public string Email { get; set; }

        public bool? EmailConfirmed { get; set; }


        [Required(ErrorMessage = "Please Enter Address..!")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Please Select City..!")]
        public int? CityId { get; set; }

        public byte[]? PasswordHash { get; set; }

        public byte[]? PasswordSalt { get; set; }

        [Required(ErrorMessage = "Please Enter PhoneNumber..!")]
        public string PhoneNumber { get; set; }

        public bool? PhoneNumberConfirmed { get; set; }

        public bool? IsActive { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public bool? IsAdmin { get; set; }

        [Required(ErrorMessage = "Please Select Department..!")]
        public int? DepartmentId { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string? CreatedBy { get; set; }

        [Required(ErrorMessage = "Please Select State..!")]
        public int? StateId { get; set; }


        [Required(ErrorMessage = "Please Select Country..!")]
        public int? CountryId { get; set; }
        [Required(ErrorMessage = "Please Enter Password..!")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Please Enter the Confirm Password..!")]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = ("Confirm Password Dose Not Match..!!"))]
        public string ConfirmPassword { get; set; }

        public IFormFile Image { get; set; }
        public string ProfileImage { get; set; }


    }
}
