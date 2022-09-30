using FHIR.DataModel.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.Administration
{
    public class Practitioner : DomainResource
    {
        public List<Identifier> identifier { get; set; }
        public bool? active { get; set; }
        public List<HumanName> name { get; set; }
        public List<ContactPoint> telecom { get; set; }
        public List<Address> address { get; set; }
        public string gender { get; set; }
        public DateTime? birthDate { get; set; }
        public List<Attachment> photo { get; set; }
        public List<Qualification> qualification { get; set; }
        public List<CodeableConcept> communication { get; set; }
    }
}
