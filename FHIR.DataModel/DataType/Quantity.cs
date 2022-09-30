using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Quantity : Element
    {
        public decimal value { get; set; }
        public string comparator { get; set; }
        public string unit { get; set; }
        public string system { get; set; }
        public string code { get; set; }
    }
}
