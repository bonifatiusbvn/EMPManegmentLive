using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblDocumentMaster
{
    public int Id { get; set; }

    public string DocumentType { get; set; } = null!;

    public virtual ICollection<TblUserDocument> TblUserDocuments { get; set; } = new List<TblUserDocument>();
}
