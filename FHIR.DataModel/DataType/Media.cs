using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Media:BackboneElement
    {
        public string comment { get; set; }
        public Reference link { get; set; }
    }
}
