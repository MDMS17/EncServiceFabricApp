using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Expression:Element
    {
        public string description { get; set; }
        public string name { get; set; }
        public string language { get; set; }
        public string expression { get; set; }
        public string reference { get; set; }
    }
}
