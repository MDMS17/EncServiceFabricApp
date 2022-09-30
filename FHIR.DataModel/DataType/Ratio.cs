using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Ratio : Element
    {
        public decimal? numerator { get; set; }
        public decimal? denominator { get; set; }
    }
}
