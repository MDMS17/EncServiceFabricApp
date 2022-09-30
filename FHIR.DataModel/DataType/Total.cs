using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Total : BackboneElement 
    {
        public CodeableConcept category { get; set; }
        public Money amount { get; set; }
    }
}
