using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels.FormMaster
{
    public class FormMasterModel
    {
        public int FormId { get; set; }

        public string? FormGroup { get; set; }

        public string FormName { get; set; } = null!;

        public string? Controller { get; set; }

        public int? OrderId { get; set; }

        public string? Action { get; set; }

        public bool IsActive { get; set; }
    }
}
