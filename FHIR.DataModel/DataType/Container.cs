using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Container : BackboneElement
    {
        public List<Identifier> identifier { get; set; }
        public string description { get; set; }
        public CodeableConcept type { get; set; }
        public decimal? capacity { get; set; }
        public decimal? specimenQuantity { get; set; }
        public Additive additive { get; set; }
    }
}
