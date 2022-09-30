using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Serviced
    {
        public DateTime? servicedDate { get; set; }
        public Period servicedPeriod { get; set; }
    }
}
