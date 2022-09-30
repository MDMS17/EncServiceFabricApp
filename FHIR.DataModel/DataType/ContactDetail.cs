using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class ContactDetail:Element
    {
        public string name { get; set; }
        public List<ContactPoint> telecom { get; set; }
    }
}
