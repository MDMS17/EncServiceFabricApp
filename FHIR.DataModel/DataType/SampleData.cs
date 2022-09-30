using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class SampleData : Element
    {
        public decimal origin { get; set; }
        public decimal period { get; set; }
        public decimal? factor { get; set; }
        public decimal? lowerLimit { get; set; }
        public decimal? upperLimit { get; set; }
        public uint dimensions { get; set; }
        public string data { get; set; }
    }
}
