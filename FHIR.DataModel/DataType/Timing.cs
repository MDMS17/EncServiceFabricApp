using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Timing : BackboneElement
    {
        public List<DateTime> _event { get;set; }
        public Repeat repeat { get; set; }
        public CodeableConcept code { get; set; }
    }
}
