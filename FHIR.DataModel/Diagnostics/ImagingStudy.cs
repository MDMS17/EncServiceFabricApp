using FHIR.DataModel.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.Diagnostics
{
    public class ImagingStudy : DomainResource
    {
        public List<Identifier> identifier { get; set; }
        public string status { get; set; }
        public List<string> modality { get; set; }
        public Reference subject { get; set; }
        public Reference encounter { get; set; }
        public DateTime? started { get; set; }
        public List<Reference> basedOn { get; set; }
        public Reference referrer { get; set; }
        public List<Reference> interpreter { get; set; }
        public List<Reference> endpoint { get; set; }
        public uint? numberOfSeries { get; set; }
        public uint? numberOfInstances { get; set; }
        public Reference procedureReference { get; set; }
        public List<CodeableConcept> procedureCode { get; set; }
        public Reference location { get; set; }
        public List<CodeableConcept> reasonCode { get; set; }
        public List<Reference> reasonReference { get; set; }
        public List<Annotation> note { get; set; }
        public string description { get; set; }
        public List<Series> series { get; set; }
    }
}
