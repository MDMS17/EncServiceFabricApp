﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class PopulationMeasureReport:BackboneElement 
    {
        public CodeableConcept code { get; set; }
        public int? count { get; set; }
        public Reference subjectResults { get; set; }
    }
}
