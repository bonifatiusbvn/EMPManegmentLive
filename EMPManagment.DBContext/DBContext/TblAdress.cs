using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblAdress
{
    public int Id { get; set; }

    public Guid? UserId { get; set; }

    public string? Address { get; set; }
}
