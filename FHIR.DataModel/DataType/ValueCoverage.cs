using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class ValueCoverage
    {
        public decimal? valueQuantity { get; set; }
        public Money valueMoney { get; set; }
    }
}
