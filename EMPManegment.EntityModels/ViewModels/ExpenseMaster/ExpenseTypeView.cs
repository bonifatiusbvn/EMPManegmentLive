using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels.ExpenseMaster
{
    public class ExpenseTypeView
    {
        public int Id { get; set; }

        public string Type { get; set; } = null!;

        public DateTime CreatedOn { get; set; }
    }
}
