using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Start 
    {
        public DateTime startDate { get; set; }
        public CodeableConcept startCodeableConcept { get; set; }
    }
}
