using FHIR.DataModel.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.Administration
{
    public class HealthCareService : DomainResource
    {
        public List<Identifier> identifier { get; set; }
        public bool? active { get; set; }
        public Reference providedBy { get; set; }
        public List<CodeableConcept> category { get; set; }
        public List<CodeableConcept> type { get; set; }
        public List<CodeableConcept> specialty { get; set; }
        public List<Reference> location { get; set; }
        public string name { get; set; }
        public string comment { get; set; }
        public string extraDetails { get; set; }
        public Attachment photo { get; set; }
        public List<ContactPoint> telecom { get; set; }
        public List<Reference> coverageArea { get; set; }
        public List<CodeableConcept> serviceProviderCode { get; set; }
        public List<Eligibility> eligibility { get; set; }
        public List<CodeableConcept> program { get; set; }
        public List<CodeableConcept> characteristic { get; set; }
        public List<CodeableConcept> communication { get; set; }
        public List<CodeableConcept> referralMethod { get; set; }
        public bool? appointmentRequired { get; set; }
        public List<AvailableTime> availableTime { get; set; }
        public List<NotAvailable> notAvailable { get; set; }
        public string availabilityExceptions { get; set; }
        public List<Reference> endpoint { get; set; }
    }
}
