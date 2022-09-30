using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class TaskInputOutput : BackboneElement
    {
        public CodeableConcept type { get; set; }
        public string value { get; set; }
    }
}
