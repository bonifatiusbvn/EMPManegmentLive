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
        [Required]
        public int? TaskType { get; set; }
        [Required]
        public string? TaskTypeName { get; set; }
        [Required]
        public string? TaskTitle { get; set; }
        [Required]
        public string? TaskDetails { get; set; }
        [Required]
        public DateTime? TaskDate { get; set; }
        
        public Guid? UserId { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string? CreatedBy { get; set; }
        [Required]
        public DateTime? TaskEndDate { get; set; }
        [Required]
        public string? TaskStatus { get; set; }
        public string? UserProfile { get; set; }
        public string? UserName { get; set; }
    }
}
