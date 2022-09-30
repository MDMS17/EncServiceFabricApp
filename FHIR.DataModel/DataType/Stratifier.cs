using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Stratifier:BackboneElement 
    {
        public CodeableConcept code { get; set; }
        public string description { get; set; }
        public Expression criteria { get; set; }
        public List<ComponentMeasure> component { get; set; }
    }
}
