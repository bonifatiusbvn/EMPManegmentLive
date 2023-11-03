using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels
{
    public class UserEditViewModel
    {
        public Guid Id { get; set; }
        [Required]


        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        public string? Gender { get; set; }
        [Required]
        public DateTime DateOfBirth { get; set; }
        [Required]
        public string? Email { get; set; }
        [Required]
        public string? Address { get; set; }
        
        public int? CityId { get; set; }

        [Required]
        public string? PhoneNumber { get; set; }

        public int? DepartmentId { get; set; }

        public DateTime? CreatedOn { get; set; }

        
        public int? StateId { get; set; }
        
        public int? CountryId { get; set; }

    }
}
