using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Instance : BackboneElement
    {
        public string uid { get; set; }
        public Coding sopClass { get; set; }
        public uint? number { get; set; }
        public string title { get; set; }
    }
}
