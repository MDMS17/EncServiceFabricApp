using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Born
    {
        public Period bornPeriod { get; set; }
        public DateTime? bornDate { get; set; }
        public string bornString { get; set; }
    }
}
