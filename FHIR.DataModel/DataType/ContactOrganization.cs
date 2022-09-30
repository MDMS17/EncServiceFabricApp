using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class ContactOrganization : BackboneElement
    {
        public CodeableConcept purpose { get; set; }
        public HumanName name { get; set; }
        public List<ContactPoint> telecom { get; set; }
        public Address address { get; set; }
    }
}
