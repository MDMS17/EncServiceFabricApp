﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Narrative : Element
    {
        public string status { get; set; }
        public string div { get; set; }
    }
}
