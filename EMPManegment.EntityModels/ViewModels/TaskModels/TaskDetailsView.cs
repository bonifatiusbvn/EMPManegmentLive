using EMPManegment.EntityModels.View_Model;
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
        
        public Guid? UserId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string? IsCompleted { get; set; }
        public Guid? CompletedBy { get; set; }
        public Guid? CreatedBy { get; set; }
        public DateTime? TaskEndDate { get; set; }
        public string? TaskStatus { get; set; }
        public string? UserProfile { get; set; }
        public string? UserName { get; set; }
        public string? Role { get; set; }
    }
}
