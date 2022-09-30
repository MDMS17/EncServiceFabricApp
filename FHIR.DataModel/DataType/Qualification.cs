using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Qualification : BackboneElement
    {
        public List<Identifier> identifier { get; set; }
        public CodeableConcept code { get; set; }
        public Period period { get; set; }
        public Reference issuer { get; set; }
    }
}
