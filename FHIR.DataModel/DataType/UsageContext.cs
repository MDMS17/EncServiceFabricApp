using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class UsageContext:Element
    {
        public Coding code { get; set; }
        public ValueUsageContext value { get; set; }
    }
}
