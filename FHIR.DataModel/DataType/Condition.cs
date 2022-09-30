using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Condition : BackboneElement
    {
        public CodeableConcept code { get; set; }
        public CodeableConcept outcome { get; set; }
        public bool? contributedToDeath { get; set; }
        public OnsetCondition onset { get; set; }
        public List<Annotation> note { get; set; }
    }
}
