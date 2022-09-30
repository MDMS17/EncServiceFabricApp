using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Deceased
    {
        public bool? deceasedBoolean { get; set; }
        public Age deceasedAge { get; set; }
        public Range deceasedRange { get; set; }
        public DateTime? deceasedDate { get; set; }
        public string deceasedString { get; set; }
    }
}
