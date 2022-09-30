using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class ValueUsageContext
    {
        public CodeableConcept valueCodeableConcept { get; set; }
        public decimal? valueQuantity { get; set; }
        public Range valueRange { get; set; }
        public Reference valueReference { get; set; }
    }
}
