using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace EMPManegment.EntityModels.ViewModels.ProjectModels
{
    public class ProjectDetailView
    {
        public Guid? ProjectId { get; set; }

        public string? ProjectType { get; set; }

        public string? ProjectTitle { get; set; }

        public string? ShortName { get; set; }

        public string? ProjectHead { get; set; }

        public string? ProjectDescription { get; set; }

        public string? BuildingName { get; set; }

        public string? Area { get; set; }

        public int? State { get; set; }

        public int? City { get; set; }

        public int? Country { get; set; }

        public string? PinCode { get; set; }

        public string? ProjectPriority { get; set; }

        public string? ProjectStatus { get; set; }

        public DateTime ProjectStartDate { get; set; }

        public DateTime ProjectEndDate { get; set; }

        public DateTime ProjectDeadline { get; set; }

        public string? ProjectImage { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public string? ProjectPath { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string? CreatedBy { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public Guid? UserId { get; set; }

        public Guid? Id { get; set; }

        public int? TaskCount { get; set; }

        public string? CountryName { get; set; }

        public string? StateName { get; set; }

        public string? CityName { get; set; }
        public string? ProjectImageName { get; set; }
        public bool? IsDeleted { get; set; }

    }
    public class ProjectDetailRequestModel
    {
        public Guid ProjectId { get; set; }

        public string? ProjectType { get; set; }

        public string? ProjectTitle { get; set; }

        public string? ShortName { get; set; }

        public string? ProjectHead { get; set; }

        public string? ProjectDescription { get; set; }

        public string? BuildingName { get; set; }

        public string? Area { get; set; }

        public int? State { get; set; }

        public int? City { get; set; }

        public int? Country { get; set; }

        public string? PinCode { get; set; }

        public string? ProjectPriority { get; set; }

        public string? ProjectStatus { get; set; }

        public DateTime ProjectStartDate { get; set; }

        public DateTime ProjectEndDate { get; set; }

        public DateTime ProjectDeadline { get; set; }

        public string? ProjectPath { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string? CreatedBy { get; set; }

        public Guid? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
        public Guid? UserId { get; set; }

        public Guid? Id { get; set; }

        public IFormFile? ProjectImage { get; set; }

        public string? ProjectImageName { get; set;}
    }
}
