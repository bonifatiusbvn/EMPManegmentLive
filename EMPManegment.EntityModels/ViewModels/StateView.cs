﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPManegment.EntityModels.ViewModels
{
    public class StateView
    {
        public int Id { get; set; }
        public string StateName { get; set; }
        public int? StateCode { get; set; }
    }
}
