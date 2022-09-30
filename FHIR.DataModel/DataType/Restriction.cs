using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Restriction : BackboneElement 
    {
        public uint? repetitions { get; set; }
        public Period period { get; set; }
        public List<Reference> recipient { get; set; }
    }
}
