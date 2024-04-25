using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels.UserModels
{
    public class FromPermission
    {
        public string FormName { get; set; }
        public string GroupName { get; set; }
        public string Controller { get; set; }
        public string Action { get; set; }
        public bool Add { get; set; }
        public bool View { get; set; }
        public bool Edit { get; set; }
        public bool Delete { get; set; }
    }
}
