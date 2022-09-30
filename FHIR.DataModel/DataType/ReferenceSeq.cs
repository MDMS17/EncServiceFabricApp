using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class ReferenceSeq : BackboneElement
    {
        public CodeableConcept chromosome { get; set; }
        public string genomeBuild { get; set; }
        public string orientation { get; set; }
        public CodeableConcept referenceSeqId { get; set; }
        public Reference referenceSeqPointer { get; set; }
        public string referenceSeqString { get; set; }
        public string strand { get; set; }
        public int? windowStart { get; set; }
        public int? windowEnd { get; set; }
    }
}
