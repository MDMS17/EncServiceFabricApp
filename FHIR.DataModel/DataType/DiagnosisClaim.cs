using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class DiagnosisClaim
    {
        public CodeableConcept diagnosisCodeableConcept { get; set; }
        public Reference diagnosisReference { get; set; }
    }
}
