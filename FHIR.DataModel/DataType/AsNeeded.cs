using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class AsNeeded
    {
        public bool? asNeededBoolean { get; set; }
        public CodeableConcept asNeededCodeableConcept { get; set; }
    }
}
