using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class NotAvailable : BackboneElement
    {
        public string description { get; set; }
        public Period during { get; set; }
    }
}
