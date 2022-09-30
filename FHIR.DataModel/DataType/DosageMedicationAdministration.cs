using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class DosageMedicationAdministration:BackboneElement
    {
        public string text { get; set; }
        public CodeableConcept site { get; set; }
        public CodeableConcept route { get; set; }
        public CodeableConcept method { get; set; }
        public decimal? dose { get; set; }
        public RateMedicationAdministration rate { get; set; }
    }
}
