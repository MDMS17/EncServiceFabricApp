using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Communication
    {
        public CodeableConcept language { get; set; }
        public bool? preferred { get; set; }
    }
}
