using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels
{
    public class DocumentInfoView
    {
        public int Id { get; set; }
        public Guid? UserId { get; set; }
        [Required]
        public int? DocumentTypeId { get; set; }
        [Required]
        [RegularExpression("^.+(.pdf|.PDF)$",ErrorMessage ="You can Only add pdf files")]
        public string DocumentName { get; set; } = "";
        public string DocumentType { get; set; } = "";
        public DateTime? CreatedOn { get; set; }
        public string CreatedBy { get; set; } = "";
        public string UserName { get; set; } = "";
    }
}
