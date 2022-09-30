using FHIR.DataModel.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.Administration
{
    public class Location : DomainResource
    {
        public List<Identifier> identifier { get; set; }
        public string status { get; set; }
        public Coding operationalStatus { get; set; }
        public string name { get; set; }
        public List<string> alias { get; set; }
        public string description { get; set; }
        public string mode { get; set; }
        public List<CodeableConcept> type { get; set; }
        public List<ContactPoint> telecom { get; set; }
        public Address address { get; set; }
        public CodeableConcept physicalType { get; set; }
        public Position position { get; set; }
        public Reference managingOrganization { get; set; }
        public Reference partOf { get; set; }
        public List<HoursOfOperation> hoursOfOperation { get; set; }
        public string availabilityExceptions { get; set; }
        public List<Reference> endpoint { get; set; }
    }
}
