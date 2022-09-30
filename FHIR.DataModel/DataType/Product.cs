using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Product
    {
        public CodeableConcept productCodeableConcept { get; set; }
        public Reference productReference { get; set; }
    }
}
