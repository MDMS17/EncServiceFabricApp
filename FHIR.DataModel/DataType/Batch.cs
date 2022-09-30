using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Batch:BackboneElement 
    {
        public string lotNumber { get; set; }
        public DateTime? expirationDate { get; set; }
    }
}
