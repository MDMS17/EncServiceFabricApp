using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Module
    {
        public string moduleUri { get; set; }
        public string moduleCanonical { get; set; }
        public CodeableConcept moduleCodeableConcept { get; set; }
    }
}
