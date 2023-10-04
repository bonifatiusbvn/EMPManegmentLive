using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblUserDocument
{
    public int Id { get; set; }

    public Guid? UserId { get; set; }

    public int? DocumentType { get; set; }

    public string? DocumentName { get; set; }

    public virtual TblUser? User { get; set; }
}
