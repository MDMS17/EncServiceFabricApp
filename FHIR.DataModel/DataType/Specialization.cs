using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Specialization : BackboneElement
    {
        public CodeableConcept systemType { get; set; }
        public string version { get; set; }
    }
}
