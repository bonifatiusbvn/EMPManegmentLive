using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels.ProjectModels
{
    public class ProjectDetailView
    {
        public Guid? ProjectId { get; set; }

        public string? ProjectType { get; set; } = null!;

        public string? ProjectTitle { get; set; } = null!;

        public string? ProjectHead { get; set; } = null!;

        public string? ProjectDescription { get; set; } = null!;

        public string? ProjectLocation { get; set; } = null!;

        public string? ProjectPriority { get; set; } = null!;

        public string? ProjectStatus { get; set; } = null!;

        public DateTime ProjectStartDate { get; set; }

        public DateTime ProjectEndDate { get; set; }

        public DateTime ProjectDeadline { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string? CreatedBy { get; set; }
        public Guid? UserId { get; set; }
        public Guid? Id { get; set; }
    }
}
