using FHIR.DataModel.DataType;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.Diagnostics
{
    public class Genomics : DomainResource
    {
        public List<Identifier> identifier { get; set; }
        public string type { get; set; }
        public int coordinateSystem { get; set; }
        public Reference patient { get; set; }
        public Reference specimen { get; set; }
        public Reference device { get; set; }
        public Reference performer { get; set; }
        public decimal? quantity { get; set; }
        public ReferenceSeq referenceSeq { get; set; }
        public List<Variant> variant { get; set; }
        public string observedSeq { get; set; }
        public List<Quality> quality { get; set; }
        public int? readCoverage { get; set; }
        public List<Repository> repository { get; set; }
        public List<Reference> pointer { get; set; }
        public List<StructureVariant> structureVariant { get; set; }
    }
}
