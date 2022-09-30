using FHIR.DataModel.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.Administration
{
    public class CareTeam : DomainResource
    {
        public List<Identifier> identifier { get; set; }
        public string status { get; set; }
        public List<CodeableConcept> category { get; set; }
        public string name { get; set; }
        public Reference subject { get; set; }
        public Reference encounter { get; set; }
        public Period period { get; set; }
        public List<Participant> participant { get; set; }
        public List<CodeableConcept> reasonCode { get; set; }
        public List<Reference> reasonReference { get; set; }
        public List<Reference> managingOrganization { get; set; }
        public List<ContactPoint> telecom { get; set; }
        public List<Annotation> note { get; set; }
    }
}
