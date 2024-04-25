using EMPManegment.EntityModels.ViewModels.FormMaster;
using EMPManegment.EntityModels.ViewModels.UserModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels
{
    public class LoginView
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ProfileImage { get; set; }
        public string UserName { get; set;}
        public string Password { get; set;}

        public DateTime LastLoginDate { get; set; }

        public string FullName { get; set; }
        public string Role { get; set; }

        public Guid RoleId { get; set; }
        public List<FromPermission> FromPermissionData { get; set; }
    }
    public class LoginResponseModel
    {
        public string Message { get; set; }

        public int Code { get; set; }

        public LoginView Data { get; set; }

    }

    public class UserRole
    {
        public string Role { get; set; }

    }
}
