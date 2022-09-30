using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Age
    {
        public uint? ageAge { get; set; }
        public Range ageRange { get; set; }
        public string ageString { get; set; }
    }
}
