﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Period
    {
        public DateTime? start { get; set; }
        public DateTime? end { get; set; }
    }
}
