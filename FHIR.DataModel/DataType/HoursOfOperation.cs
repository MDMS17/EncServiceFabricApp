using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class HoursOfOperation : BackboneElement
    {
        public List<string> daysOfWeek { get; set; }
        public bool? allDay { get; set; }
        public DateTime? openingTime { get; set; }
        public DateTime? closingTime { get; set; }
    }
}
