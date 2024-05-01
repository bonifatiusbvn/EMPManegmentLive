using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblUserDocument
{
    public int Id { get; set; }

    public Guid? UserId { get; set; }

    public int? DocumentTypeId { get; set; }

    public string? DocumentName { get; set; }

    public DateTime? CreatedOn { get; set; }

    public string? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual TblDocumentMaster? DocumentType { get; set; }

    public virtual TblUser? User { get; set; }
}
