using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class PriceComponent : BackboneElement
    {
        public string type { get; set; }
        public CodeableConcept code { get; set; }
        public decimal? factor { get; set; }
        public Money amount { get; set; }
    }
}
