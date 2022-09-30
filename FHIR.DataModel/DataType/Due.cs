using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Due
    {
        public DateTime? dueDate { get; set; }
        public TimeSpan dueDuration { get; set; }
    }
}
