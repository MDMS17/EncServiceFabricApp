using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class CodeFilter:Element
    {
        public string path { get; set; }
        public string searchParam { get; set; }
        public string valueSet { get; set; }
        public List<Coding> code { get; set; }
    }
}
