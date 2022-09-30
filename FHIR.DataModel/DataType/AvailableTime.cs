using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class AvailableTime :BackboneElement
    {
        public List<string> daysOfWeek { get; set; }
        public bool? allDay { get; set; }
        public DateTime? availableStartTime { get; set; }
        public DateTime? availableEndTime { get; set; }

    }
}
