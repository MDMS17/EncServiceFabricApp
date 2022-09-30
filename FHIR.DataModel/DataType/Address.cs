using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Address : Element
    {
        public string use { get; set; }
        public string type { get; set; }
        public string text { get; set; }
        public List<string> line { get; set; }
        public string city { get; set; }
        public string district { get; set; }
        public string state { get; set; }
        public string postalCode { get; set; }
        public string country { get; set; }
        public Period period { get; set; }
    }
}
