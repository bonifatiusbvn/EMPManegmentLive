using System;
using System.Collections.Generic;

namespace EMPManagment.API;

public partial class TblCountry
{
    public int Id { get; set; }

    public string? CountryCode { get; set; }

    public string? Country { get; set; }

    public virtual ICollection<TblState> TblStates { get; set; } = new List<TblState>();
}
