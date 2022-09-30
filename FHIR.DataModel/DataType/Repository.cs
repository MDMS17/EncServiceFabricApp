using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Repository : BackboneElement
    {
        public string type { get; set; }
        public string url { get; set; }
        public string name { get; set; }
        public string datasetId { get; set; }
        public string variantsetId { get; set; }
        public string readsetId { get; set; }
    }
}
