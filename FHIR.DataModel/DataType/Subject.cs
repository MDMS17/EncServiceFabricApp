using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Subject
    {
        public CodeableConcept subjectCodeableConcept { get; set; }
        public Reference subjectReference { get; set; }
    }
}
