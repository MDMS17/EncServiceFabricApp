﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Author
    {
        public Reference authorReference { get; set; }
        public string authorString { get; set; }
    }
}
