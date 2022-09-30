using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class DomainResource : Resource
    {
        public Narrative text { get; set; }
        public List<Resource> contained { get; set; }
        public List<Extension> extension { get; set; }
        public List<Extension> modifiedExtension { get; set; }
    }
}
