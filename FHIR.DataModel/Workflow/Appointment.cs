using FHIR.DataModel.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.Workflow
{
    public class Appointment : DomainResource
    {
        List<Identifier> identifier { get; set; }
        public string status { get; set; }
        public CodeableConcept cancelationReason { get; set; }
        public List<CodeableConcept> serviceCategory { get; set; }
        public List<CodeableConcept> serviceType { get; set; }
        public List<CodeableConcept> specialty { get; set; }
        public CodeableConcept appointmentType { get; set; }
        public List<CodeableConcept> reasonCode { get; set; }
        public List<Reference> reasonReference { get; set; }
        public uint? priority { get; set; }
        public string description { get; set; }
        public List<Reference> supportingInformation { get; set; }
        public DateTime? start { get; set; }
        public DateTime? end { get; set; }
        public uint? minutesDuration { get; set; }
        public List<Reference> slot { get; set; }
        public DateTime? created { get; set; }
        public string comment { get; set; }
        public string patientInstruction { get; set; }
        public List<Reference> basedOn { get; set; }
        public List<ParticipantAppointment> participant { get; set; }
        public List<Period> requestedPeriod { get; set; }
    }
}
