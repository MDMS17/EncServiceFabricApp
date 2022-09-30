using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Detail : BackboneElement
    {
        public string kind { get; set; }
        public List<string> instantiatesCanonical { get; set; }
        public List<string> instantiatesUri { get; set; }
        public CodeableConcept code { get; set; }
        public List<CodeableConcept> reasonReference { get; set; }
        public List<Reference> goal { get; set; }
        public string status { get; set; }
        public CodeableConcept statusReason { get; set; }
        public bool? doNotPerform { get; set; }
        public Schedule schedule { get; set; }
        public Reference location { get; set; }
        public List<Reference> performer { get; set; }
        public Product product { get; set; }
        public decimal? dailyAmount { get; set; }
        public decimal? quantity { get; set; }
        public List<Annotation> description { get; set; }
    }
}
