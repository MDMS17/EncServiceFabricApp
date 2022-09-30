using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class RelatedAction:BackboneElement
    {
        public string actionId { get; set; }
        public string relationship { get; set; }
        public Offset offset { get; set; }
    }
}
