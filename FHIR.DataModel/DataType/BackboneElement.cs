﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class BackboneElement : Element
    {
        public List<Extension> modifiedExtension { get; set; }
    }
}
