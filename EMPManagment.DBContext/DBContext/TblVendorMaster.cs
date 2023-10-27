using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblVendorMaster
{
    public int Id { get; set; }

    public int? VendorTypeId { get; set; }

    public string? VendorName { get; set; }

    public string? VendorEmail { get; set; }

    public string? VendorPhone { get; set; }

    public string? VendorAddress { get; set; }

    public string? VendorBankAccountNo { get; set; }

    public string? VendorBankName { get; set; }

    public string? VendorBankIfsc { get; set; }

    public string? VendorGstnumber { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public virtual TblVendorType? VendorType { get; set; }
}
