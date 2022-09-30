using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class ProcedureClaim
    {
        public CodeableConcept procedureCodeableConcept { get; set; }
        public Reference procedureReference { get; set; }
    }
}
