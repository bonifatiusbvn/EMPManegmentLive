using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels.ProjectModels
{
    public class ProjectView
    {
        public Guid Id { get; set; }

        public Guid? ProjectId { get; set; }

        public Guid? UserId { get; set; }

        public string? ProjectType { get; set; }

        public string? ProjectTitle { get; set; }

        public string? UserRole { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int? TotalMember { get; set; }

        public string? Status { get; set; }

        public DateTime? CreatedOn { get; set; }

        public Guid? CreatedBy { get; set; }

        public string? Fullname { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Image { get; set; }
        public string? Designation { get; set; }
        public string? ProjectDescription { get; set; } = null!;
        public bool? IsDeleted { get; set; }

        public int TotalTask { get; set; }
    }
    public class ProjectMemberUpdate
    {
        public Guid? ProjectId { get; set; }
        public Guid? UserId { get; set; }
    }
}
