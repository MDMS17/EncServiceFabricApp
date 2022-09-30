using FHIR.DataModel.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.Workflow
{
    public class Schedule : DomainResource
    {
        public List<Identifier> identifier { get; set; }
        public bool? active { get; set; }
        public List<CodeableConcept> serviceCategory { get; set; }
        public List<CodeableConcept> serviceType { get; set; }
        public List<CodeableConcept> specialty { get; set; }
        public List<Reference> actor { get; set; }
        public Period planningHorizon { get; set; }
        public string comment { get; set; }
    }
}
