using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Probability
    {
        public decimal? probabilityDecimal { get; set; }
        public Range probabilityRange { get; set; }
    }
}
