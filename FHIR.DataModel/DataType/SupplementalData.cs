using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class SupplementalData:BackboneElement 
    {
        public CodeableConcept code { get; set; }
        public List<CodeableConcept> usage { get; set; }
        public string description { get; set; }
        public Expression criteria { get; set; }
    }
}
