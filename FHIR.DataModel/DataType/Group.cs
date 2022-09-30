using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Group:BackboneElement 
    {
        public CodeableConcept code { get; set; }
        public string description { get; set; }
        public List<Population> population { get; set; }
        public List<Stratifier> stratifier { get; set; }
    }
}
