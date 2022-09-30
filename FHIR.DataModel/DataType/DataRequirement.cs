using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class DataRequirement : Element
    {
        public string type { get; set; }
        public List<string> profile { get; set; }
        public Subject subject { get; set; }
        public List<string> mustSupport { get; set; }
        public List<CodeFilter> codeFilter { get; set; }
        public List<DataFilter> dataFilter { get; set; }
        public uint? limit { get; set; }
        public List<Sort> sort { get; set; }
    }
}
