using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels.Company
{
    public class CompanyModel
    {
        public Guid Id { get; set; }

        public string? CompnyName { get; set; }

        public string? Address { get; set; }

        public int? City { get; set; }

        public int? State { get; set; }

        public int? Country { get; set; }

        public string? Gst { get; set; }

        public string? Email { get; set; }

        public string? ContactNumber { get; set; }

        public string? CompanyLogo { get; set; }

        public DateTime? CreatedOn { get; set; }

        public Guid? CreatedBy { get; set; }

        public string? StateName { get; set; }

        public string? CityName { get; set; }

        public string? CountryName { get; set; }

        public string? FullAddress { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
    }

    public class CompanyRequestModel
    {
        public Guid Id { get; set; }

        public string? CompnyName { get; set; }

        public string? Address { get; set; }

        public int? City { get; set; }

        public int? State { get; set; }

        public int? Country { get; set; }

        public string? Gst { get; set; }

        public string? Email { get; set; }

        public string? ContactNumber { get; set; }

        public IFormFile? CompanyLogo { get; set; }

        public DateTime? CreatedOn { get; set; }

        public Guid? CreatedBy { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
        public string? CompanyImageName { get; set; }

    }
}
