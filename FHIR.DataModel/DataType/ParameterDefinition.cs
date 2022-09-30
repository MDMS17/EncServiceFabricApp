using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class ParameterDefinition:Element
    {
        public string name { get; set; }
        public string use { get; set; }
        public int? min { get; set; }
        public string max { get; set; }
        public string documentation { get; set; }
        public string type { get; set; }
        public string profile { get; set; }
    }
}
