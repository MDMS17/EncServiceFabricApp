using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Position : BackboneElement
    {
        public decimal longitude { get; set; }
        public decimal latitude { get; set; }
        public decimal altitude { get; set; }
    }
}
