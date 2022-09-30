using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class CodeableConcept : Element
    {
        public List<Coding> coding { get; set; }
        public string text { get; set; }
    }
}
