using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Range : Element
    {
        public decimal? low { get; set; }
        public decimal? high { get; set; }
    }
}
