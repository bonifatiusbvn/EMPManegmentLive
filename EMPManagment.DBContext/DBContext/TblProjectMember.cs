using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblProjectMember
{
    public Guid Id { get; set; }

    public Guid? ProjectId { get; set; }

    public Guid? UserId { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedOn { get; set; }

    public Guid? CreatedBy { get; set; }

    public Guid? UpdatedBy { get; set; }

    public DateTime? UpdatedOn { get; set; }

    public virtual TblProjectMaster? Project { get; set; }

    public virtual TblUser? User { get; set; }
}
