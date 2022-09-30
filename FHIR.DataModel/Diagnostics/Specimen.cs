using FHIR.DataModel.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.Diagnostics
{
    public class Specimen : DomainResource
    {
        public List<Identifier> identifier { get; set; }
        public Identifier accessionIDentifier { get; set; }
        public string status { get; set; }
        public CodeableConcept type { get; set; }
        public Reference subject { get; set; }
        public DateTime? receivedTime { get; set; }
        public List<Reference> parent { get; set; }
        public List<Reference> request { get; set; }
        public Collection collection { get; set; }
        public List<Processing> processing { get; set; }
        public List<Container> container { get; set; }
        public List<CodeableConcept> condition { get; set; }
        public List<Annotation> note { get; set; }
    }
}
