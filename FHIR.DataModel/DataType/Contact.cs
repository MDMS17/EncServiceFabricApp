using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Contact
    {
        public List<CodeableConcept> relationship { get; set; }
        public HumanName name { get; set; }
        public List<ContactPoint> telecom { get; set; }
        public Address address { get; set; }
        public string gender { get; set; }
        public Reference organization { get; set; }
        public Period period { get; set; }
    }
}
