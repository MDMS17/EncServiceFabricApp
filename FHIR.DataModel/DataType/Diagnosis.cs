using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Diagnosis:BackboneElement
    {
        public uint sequence { get; set; }
        public DiagnosisClaim diagnosis { get; set; }
        public List<CodeableConcept> type { get; set; }
        public CodeableConcept onAdmission { get; set; }
        public CodeableConcept packageCode { get; set; }
    }
}
