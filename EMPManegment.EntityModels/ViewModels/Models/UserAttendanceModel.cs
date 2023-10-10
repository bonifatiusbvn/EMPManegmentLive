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
     
        public DateTime Date { get; set; }
        public int AttendanceId { get; set;}

        public DateTime InTime { get; set;}
        public DateTime? OutTime { get; set;}

        public decimal? TotalHours { get; set;}
    }

    
}
