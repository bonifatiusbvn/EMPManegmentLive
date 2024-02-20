using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblExpenseType
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public DateTime CreatedOn { get; set; }

    public virtual ICollection<TblExpenseMaster> TblExpenseMasters { get; set; } = new List<TblExpenseMaster>();
}
