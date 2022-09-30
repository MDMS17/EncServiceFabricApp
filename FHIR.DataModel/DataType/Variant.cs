using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Variant : BackboneElement
    {
        public int? start { get; set; }
        public int? end { get; set; }
        public string observedAllele { get; set; }
        public string referenceAllele { get; set; }
        public string cigar { get; set; }
        public Reference variantPointer { get; set; }
    }
}
