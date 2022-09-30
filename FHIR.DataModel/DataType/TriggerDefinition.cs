using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class TriggerDefinition:Element
    {
        public string type { get; set; }
        public string name { get; set; }
        public TimingTriggerDefinition timing { get; set; }
        public List<DataRequirement> data { get; set; }
        public Expression condition { get; set; }
    }
}
