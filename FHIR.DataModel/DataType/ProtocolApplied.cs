using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class ProtocolApplied:BackboneElement
    {
        public string series { get; set; }
        public Reference authority { get; set; }
        public List<CodeableConcept> targetDisease { get; set; }
        public DoseNumber doseNumber { get; set; }
        public SeriesDoses seriesDoses { get; set; }
    }
}
