using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class InitialFill:BackboneElement
    {
        public decimal? quantity { get; set; }
        public TimeSpan duration { get; set; }
    }
}
