using FHIR.DataModel.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.Diagnostics
{
    public class DiagnosticReport : DomainResource
    {
        public List<Identifier> identifier { get; set; }
        public List<Reference> basedOn { get; set; }
        public string status { get; set; }
        public List<CodeableConcept> category { get; set; }
        public CodeableConcept code { get; set; }
        public Reference subject { get; set; }
        public Reference encounter { get; set; }
        public Effective effective { get; set; }
        public DateTime? issued { get; set; }
        public List<Reference> performer { get; set; }
        public List<Reference> resultInterpreter { get; set; }
        public List<Reference> specimen { get; set; }
        public List<Reference> result { get; set; }
        public List<Reference> imagingStudy { get; set; }
        public List<Media> media { get; set; }
        public string conclusion { get; set; }
        public List<CodeableConcept> conclusionCode { get; set; }
        public List<Attachment> presentedFrom { get; set; }
    }
}
