using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Collection : BackboneElement
    {
        public Reference collector { get; set; }
        public Collected collected { get; set; }
        public TimeSpan duration { get; set; }
        public decimal? quantity { get; set; }
        public CodeableConcept method { get; set; }
        public CodeableConcept bodySite { get; set; }
        public FastingStatus fastingStatus { get; set; }
    }
}
