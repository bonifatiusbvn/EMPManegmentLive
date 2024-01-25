using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels
{
    public class EmpDocumentView
    {
        public int Id { get; set; }

        [Required]
        public string DocumentType { get; set; }
        public Guid? UserId { get; set; }
        [Required]
        public int? DocumentTypeId { get; set; }
        public IFormFile DocumentName { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }
}
