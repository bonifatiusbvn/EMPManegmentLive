using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels.UserModels
{
    public class UserRoleModel
    {
        public Guid RoleId { get; set; }

        public string? Role { get; set; }

        public bool? IsActive { get; set; }

        public bool? IsDelete { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }
    public class UserPermissionModel
    {
        public int Id { get; set; }

        public Guid UserId { get; set; }

        public int FormId { get; set; }

        public bool IsAddAllow { get; set; }

        public bool IsViewAllow { get; set; }

        public bool IsEditAllow { get; set; }

        public bool IsDeleteAllow { get; set; }
        public string? UserName { get; set; }
        public string? FormName { get; set; }
        public int RowNumber { get; set; }
        public Guid CreatedBy { get; set; }

        public DateTime? CreatedOn { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

    }
}
