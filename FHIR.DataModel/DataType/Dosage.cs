using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Dosage : BackboneElement
    {
        public int? sequence { get; set; }
        public string text { get; set; }
        public List<CodeableConcept> additionalInstruction { get; set; }
        public string patientInstruction { get; set; }
        public Timing timing { get; set; }
        public AsNeeded asNeeded { get; set; }
        public CodeableConcept site { get; set; }
        public CodeableConcept route { get; set; }
        public CodeableConcept method { get; set; }
        public List<DoseAndRate> doseAndRate { get; set; }
        public Ratio maxDosePerPeriod { get; set; }
        public decimal? maxDosePerAdministration { get; set; }
        public decimal? maxDosePerLifetime { get; set; }
    }
}
