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
        public TimeSpan? TotalHours { get; set;}
        public string? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }
 
    }

    public class UserAttendanceRequestModel
    {
        public Guid UserId { get; set; }
        public DateTime Date { get; set; }
    }
    

    public class UserAttendanceResponseModel
    {
        public string Message { get; set; }

        public int Code { get; set; }

        public UserAttendanceModel Data { get; set; }

    }
    public class SearchAttendanceModel
    {
        public Guid? UserId { get; set;}
        public DateTime? Cmonth { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
    public class searchAttendanceListModel
    {
        public Guid? UserId { get; set; }
        public DateTime Date { get; set; }
    }
}
