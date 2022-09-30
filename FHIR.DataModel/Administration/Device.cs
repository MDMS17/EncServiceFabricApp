using FHIR.DataModel.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.Administration
{
    public class Device : DomainResource
    {
        public List<Identifier> identifier { get; set; }
        public Reference definition { get; set; }
        public List<UdiCarrier> udiCarrier { get; set; }
        public string status { get; set; }
        public List<CodeableConcept> statusReason { get; set; }
        public string distinctIdentifier { get; set; }
        public string manufacturer { get; set; }
        public DateTime? manufactureDate { get; set; }
        public DateTime? expirationDate { get; set; }
        public string lotNumber { get; set; }
        public string serialNumber { get; set; }
        public List<DeviceName> deviceName { get; set; }
        public string modelNumber { get; set; }
        public string partNumber { get; set; }
        public CodeableConcept type { get; set; }
        public List<Specialization> specialization { get; set; }
        public List<FHIR.DataModel.DataType.Version> version { get; set; }
        public List<Property> property { get; set; }
        public Reference patient { get; set; }
        public Reference owner { get; set; }
        public List<ContactPoint> contact { get; set; }
        public Reference location { get; set; }
        public string url { get; set; }
        public List<Annotation> note { get; set; }
        public List<CodeableConcept> safety { get; set; }
        public Reference parent { get; set; }

    }
}
