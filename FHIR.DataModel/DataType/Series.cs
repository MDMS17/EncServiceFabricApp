using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Series : BackboneElement
    {
        public string uid { get; set; }
        public uint? number { get; set; }
        public string modality { get; set; }
        public string description { get; set; }
        public uint? numberOfInstances { get; set; }
        public List<Reference> endpoint { get; set; }
        public Coding bodySite { get; set; }
        public Coding laterality { get; set; }
        public List<Reference> specimen { get; set; }
        public DateTime? started { get; set; }
        public List<Performer> performer { get; set; }
        public List<Instance> instance { get; set; }
    }
}
