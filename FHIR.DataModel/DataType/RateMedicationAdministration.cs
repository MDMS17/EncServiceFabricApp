using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class RateMedicationAdministration
    {
        public Ratio rateRatio { get; set; }
        public decimal? rateQuantity { get; set; }
    }
}
