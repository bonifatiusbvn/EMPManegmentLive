using EMPManegment.EntityModels.View_Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels.TaskModels
{
    public class TaskDetailsView
    {
        public Guid Id { get; set; }

        public int? TaskType { get; set; }
        public string? TaskTypeName { get; set; }

        public string? TaskTitle { get; set; }

        public string? TaskDetails { get; set; }

        public DateTime? TaskDate { get; set; }
        public Guid? ProjectId { get; set; }

        public Guid? UserId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? IsCompleted { get; set; }
        public Guid? CompletedBy { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? TaskEndDate { get; set; }
        public string? TaskStatus { get; set; }
        public string? Document { get; set; }
        public string? UserProfile { get; set; }
        public string? UserName { get; set; }
        public string? FirstName { get; set; }
        public string? FullName { get; set; }
        public string? LastName { get; set; }
        public string? ProjectHead { get; set; }
        public string? Role { get; set; }
        public Guid? UpdatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }

    }
    public class TaskImageDetailsModel
    {
        public Guid? ProjectId { get; set; }
        public Guid? UserId { get; set; }
        public IFormFile? Image { get; set; }
        public int? TaskType { get; set; }
        public string? TaskTypeName { get; set; }

        public string? TaskTitle { get; set; }

        public string? TaskDetails { get; set; }

        public DateTime? TaskDate { get; set; }
        public Guid? CreatedBy { get; set; }
        public string? TaskStatus { get; set; }
        public DateTime? TaskEndDate { get; set; }   
    }
        
}
 