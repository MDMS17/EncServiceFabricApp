using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Adjudication : BackboneElement 
    {
        public CodeableConcept category { get; set; }
        public CodeableConcept reason { get; set; }
        public Money amount { get; set; }
        public decimal? value { get; set; }
    }
}
