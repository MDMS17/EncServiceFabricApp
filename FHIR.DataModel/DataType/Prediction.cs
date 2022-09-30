using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Prediction : BackboneElement
    {
        public CodeableConcept outcome { get; set; }
        public Probability probability { get; set; }
        public CodeableConcept qualitativeRisk { get; set; }
        public decimal? relativeRisk { get; set; }
        public When when { get; set; }
        public string rationale { get; set; }
    }
}
