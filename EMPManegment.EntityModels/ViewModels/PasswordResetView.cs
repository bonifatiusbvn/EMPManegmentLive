using EMPManegment.EntityModels.View_Model;
using System;
using System.Collections.Generic;
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

    }
    public class PasswordResetResponseModel
    {
        public string? UserName { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
        

    }
}
