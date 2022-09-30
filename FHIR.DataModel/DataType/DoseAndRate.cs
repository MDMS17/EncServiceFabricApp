using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class DoseAndRate:Element
    {
        public CodeableConcept type { get; set; }
        public Dose dose { get; set; }
        public Rate rate { get; set; }
    }
}
