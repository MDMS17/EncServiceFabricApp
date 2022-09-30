using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Coding : Element
    {
        public string system { get; set; }
        public string version { get; set; }
        public string code { get; set; }
        public string display { get; set; }
        public bool? userSelected { get; set; }
    }
}
