using EMPManegment.EntityModels.View_Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels.VendorModels
{
    public class VendorDetailsView
    {

        public Guid Vid { get; set; }
        public string VendorFirstName { get; set; } = null!;
        public string VendorLastName { get; set; } = null!;
        public string? VendorEmail { get; set; }
        public string? VendorPhone { get; set; }
        public string? VendorContectNo { get; set; }
        public int VendorCountry { get; set; }
        public string? VendorCountryName { get; set; } 
        public int VendorState { get; set; }
        public string? VendorStateName { get; set; } 
        public int VendorCity { get; set; }
        public string? VendorCityName { get; set; } 
        public string VendorPinCode { get; set; } = null!;
        public string VendorAddress { get; set; } = null!;
        public string? VendorCompanyType { get; set; }
        public string? VendorCompany { get; set; }
        public string? VendorCompanyEmail { get; set; }
        public string? VendorCompanyNumber { get; set; }
        public string? VendorCompanyLogo { get; set; }
        public string? VendorBankAccountNo { get; set; }
        public string? VendorBankName { get; set; }
        public string VendorBankBranch { get; set; } = null!;
        public string VendorAccountHolderName { get; set; } = null!;
        public string? VendorBankIfsc { get; set; }
        public string? VendorGstnumber { get; set; }
        public int VendorTypeId { get; set; }
        public string? VendorTypeName { get; set; } 
        public DateTime? CreatedOn { get; set; }
        public string? CreatedBy { get; set; }
        public Guid? UpdatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }
        public string? FullAddress { get; set; }
        public string? Image { get; set; }
    }


    public class VendorListDetailsView
    {
        public Guid Id { get; set; }
        public string? VendorCompany { get; set; }  
    }

}
