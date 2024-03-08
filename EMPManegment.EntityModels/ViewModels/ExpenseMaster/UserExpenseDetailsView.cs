using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels.ExpenseMaster
{
    public class UserExpenseDetailsView
    {


        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }

        public string Image { get; set; }
        public DateTime Date { get; set; }
        public decimal TotalAmount { get; set; }


    }
}
