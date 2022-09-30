using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class FocalDevice : BackboneElement
    {
        public CodeableConcept action { get; set; }
        public Reference manipulated { get; set; }
    }
}
