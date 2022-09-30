using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class QuantityServiceRequest
    {
        public decimal? quantityQuantity { get; set; }
        public Ratio quantityRatio { get; set; }
        public Range quantityRange { get; set; }
    }
}
