using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Insurance : BackboneElement 
    {
        public uint sequence { get; set; }
        public bool focal { get; set; }
        public Identifier identifier { get; set; }
        public Reference coverage { get; set; }
        public string businessArrangement { get; set; }
        public List<string> preAuthRef { get; set; }
        public Reference claimResponse { get; set; }
    }
}
