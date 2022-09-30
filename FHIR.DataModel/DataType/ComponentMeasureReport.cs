using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class ComponentMeasureReport:BackboneElement 
    {
        public CodeableConcept code { get; set; }
        public CodeableConcept value { get; set; }
    }
}
