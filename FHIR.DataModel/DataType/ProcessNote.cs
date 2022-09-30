using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class ProcessNote:BackboneElement 
    {
        public uint? number { get; set; }
        public string type { get; set; }
        public string text { get; set; }
        public CodeableConcept language { get; set; }
    }
}
