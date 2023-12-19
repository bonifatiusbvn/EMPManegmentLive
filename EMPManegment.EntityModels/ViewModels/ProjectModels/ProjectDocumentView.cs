using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels.ProjectModels
{
    public class ProjectDocumentView
    {
        public Guid Id { get; set; }

        public Guid ProjectId { get; set; }

        public Guid UserId { get; set; }

        public DateTime Date { get; set; }

        public string DocumentName { get; set; } = null!;

        public DateTime? CreadetOn { get; set; }

        public Guid? CreatdBy { get; set; }
        public string? FullName { get; set; }
    }
}
