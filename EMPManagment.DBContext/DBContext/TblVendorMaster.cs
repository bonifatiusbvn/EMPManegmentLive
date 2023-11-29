using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblVendorMaster
{
    public Guid Vid { get; set; }

    public int VendorTypeId { get; set; }

    public string VendorFirstName { get; set; } = null!;

    public string VendorLastName { get; set; } = null!;

    public string VendorEmail { get; set; } = null!;

    public string? VendorContact { get; set; }

    public string VendorPhone { get; set; } = null!;

    public int VendorCountry { get; set; }

    public int VendorState { get; set; }

    public int VendorCity { get; set; }

    public string VendorPinCode { get; set; } = null!;

    public string VendorAddress { get; set; } = null!;

    public string? VendorCompanyType { get; set; }

    public string? VendorCompany { get; set; }

    public string? VendorCompanyEmail { get; set; }

    public string? VendorCompanyNumber { get; set; }

    public string? VendorCompanyLogo { get; set; }

    public string VendorBankName { get; set; } = null!;

    public string VendorBankBranch { get; set; } = null!;

    public string VendorAccountHolderName { get; set; } = null!;

    public string VendorBankAccountNo { get; set; } = null!;

    public string VendorBankIfsc { get; set; } = null!;

    public string VendorGstnumber { get; set; } = null!;

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public virtual TblCity VendorCityNavigation { get; set; } = null!;

    public virtual TblCountry VendorCountryNavigation { get; set; } = null!;

    public virtual TblState VendorStateNavigation { get; set; } = null!;

    public virtual TblVendorType VendorType { get; set; } = null!;
}
