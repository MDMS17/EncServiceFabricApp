using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Dose
    {
        public Range doseRange { get; set; }
        public decimal? doseQuantity { get; set; }
    }
}
