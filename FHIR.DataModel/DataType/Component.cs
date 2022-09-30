using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Component : BackboneElement
    {
        public CodeableConcept code { get; set; }
        public Value value { get; set; }
        public CodeableConcept dataAbsentReason { get; set; }
        public List<CodeableConcept> interpretation { get; set; }
        public List<ReferenceRange> referenceRange { get; set; }
    }
}
