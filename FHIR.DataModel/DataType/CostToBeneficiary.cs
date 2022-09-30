using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class CostToBeneficiary:BackboneElement
    {
        public CodeableConcept type { get; set; }
        public ValueCoverage value { get; set; }
        public List<Exception> exception { get; set; }
    }
}
