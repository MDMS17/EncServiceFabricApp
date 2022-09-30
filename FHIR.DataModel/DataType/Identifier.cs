using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Identifier : Element
    {
        public string use { get; set; }
        public CodeableConcept type { get; set; }
        public string system { get; set; }
        public string value { get; set; }
        public Period period { get; set; }
        public Reference assigner { get; set; }
    }
}
