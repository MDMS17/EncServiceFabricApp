using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Money : Element
    {
        public decimal value { get; set; }
        public string currency { get; set; }
    }
}
