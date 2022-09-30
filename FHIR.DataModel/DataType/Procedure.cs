using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Procedure:BackboneElement
    {
        public uint sequence { get; set; }
        public List<CodeableConcept> type { get; set; }
        public DateTime? date { get; set; }
        public ProcedureClaim procedure { get; set; }
        public List<Reference> udi { get; set; }
    }
}
