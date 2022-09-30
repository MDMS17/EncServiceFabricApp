using FHIR.DataModel.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.Administration
{
    public class Organization : DomainResource
    {
        public List<Identifier> identifier { get; set; }
        public bool? active { get; set; }
        public List<CodeableConcept> type { get; set; }
        public string name { get; set; }
        public List<string> alias { get; set; }
        public List<ContactPoint> telecom { get; set; }
        public List<Address> address { get; set; }
        public Reference partof { get; set; }
        public List<ContactOrganization> contact { get; set; }
        public List<Reference> endpoint { get; set; }

    }
}
