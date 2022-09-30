using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Rate
    {
        public Ratio rateRatio { get; set; }
        public Range rateRange { get; set; }
        public decimal? rateQuantity { get; set; }
    }
}
