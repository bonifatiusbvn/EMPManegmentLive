using EMPManegment.EntityModels.View_Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels
{
    public  class PasswordResetView
    {
        public string? UserName { get; set; }
        public byte[]? PasswordHash { get; set; }

        public byte[]? PasswordSalt { get; set; }
        public string? Email { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

    }
    public class PasswordResetResponseModel
    {
        public string? UserName { get; set; }

        [Required(ErrorMessage = "Please Enter Password..!")]
        public string? Password { get; set; }

        [Required(ErrorMessage = "Please Enter the Confirm Password..!")]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = ("Confirm Password Dose Not Match..!!"))]
        public string? ConfirmPassword { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

    }
 }
