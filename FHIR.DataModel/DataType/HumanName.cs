using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class HumanName : Element
    {
        public string use { get; set; }
        public string text { get; set; }
        public string family { get; set; }
        public List<string> given { get; set; }
        public List<string> prefix { get; set; }
        public List<string> suffix { get; set; }
        public Period period { get; set; }
    }
}
