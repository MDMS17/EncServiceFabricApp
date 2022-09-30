using FHIR.DataModel.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.Administration
{
    public class Patient : DomainResource
    {
        public List<Identifier> identifier { get; set; }
        public bool? active { get; set; }
        public List<HumanName> name { get; set; }
        public List<ContactPoint> telecom { get; set; }
        public string gender { get; set; }
        public DateTime? birthDate { get; set; }
        public bool? deceased { get; set; }
        public List<Address> address { get; set; }
        public CodeableConcept maritalStatus { get; set; }
        public int? multipleBirth { get; set; }
        public List<Attachment> photo { get; set; }
        public List<Contact> contact { get; set; }
        public List<Communication> communication { get; set; }
        public List<Reference> generalPractitioner { get; set; }
        public Reference managingOrganization { get; set; }
        public List<Link> link { get; set; }

    }
}
