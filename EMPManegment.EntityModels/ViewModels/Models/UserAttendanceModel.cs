using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels.Models
{
    public class UserAttendanceModel
    {
        public Guid UserId { get; set; }
     
        public string? UserName { get; set; }
        public DateTime Date { get; set; }
        public int? AttendanceId { get; set;}

        public DateTime Intime { get; set;}
        public DateTime? OutTime { get; set;}

        public decimal? TotalHours { get; set;}
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
    }

    
}
