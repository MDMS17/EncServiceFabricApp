using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class OccurrenceImmunization
    {
        public DateTime? occurrenceDateTime { get; set; }
        public string occurrenceString { get; set; }
    }
}
