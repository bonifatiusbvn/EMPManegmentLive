using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels.Models
{
    public class UserResponceModel
    {
        public string  UserName { get; set; }
        public Guid? Id { get; set; }
        public int Code { get; set; }
        public dynamic Data { get; set; }
        public string Message { get; set; }
        public string Icone { get; set; }
    }
}
