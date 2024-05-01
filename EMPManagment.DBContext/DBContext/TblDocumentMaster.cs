using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblDocumentMaster
{
    public int Id { get; set; }

    public string DocumentType { get; set; } = null!;

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual ICollection<TblUserDocument> TblUserDocuments { get; set; } = new List<TblUserDocument>();
}
