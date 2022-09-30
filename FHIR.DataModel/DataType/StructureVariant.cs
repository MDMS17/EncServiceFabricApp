using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class StructureVariant : BackboneElement 
    {
        public CodeableConcept variantType { get; set; }
        public bool? exact { get; set; }
        public int? length { get; set; }
        public Outer outer { get; set; }
        public Inner inner { get; set; }
    }
}
