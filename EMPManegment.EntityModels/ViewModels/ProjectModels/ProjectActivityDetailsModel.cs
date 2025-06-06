using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels.ProjectModels
{
    public class ProjectActivityDetailsModel
    {
        public Guid RecordId {  get; set; } 
        public string RecordType {  get; set; } 
        public string Title {  get; set; } 
        public string? TaskType {  get; set; } 
        public string? TaskStatus {  get; set; } 
        public DateTime? TaskDate {  get; set; } 
        public DateTime? TaskEndDate {  get; set; } 
        public string? TaskDetails {  get; set; } 
        public string? UserName {  get; set; } 
        public string? FirstName {  get; set; } 
        public string? LastName {  get; set; } 
        public string? Image {  get; set; } 
        public string? ShippingAddress {  get; set; } 
    }
}
