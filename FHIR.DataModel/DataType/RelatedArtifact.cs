using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class RelatedArtifact : Element
    {
        public string type { get; set; }
        public string label { get; set; }
        public string display { get; set; }
        public string citation { get; set; }
        public string url { get; set; }
        public Attachment document { get; set; }
        public string resource { get; set; }
    }
}
