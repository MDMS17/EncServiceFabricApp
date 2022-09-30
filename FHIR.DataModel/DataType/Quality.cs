using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Quality : BackboneElement
    {
        public string type { get; set; }
        public CodeableConcept standardSequence { get; set; }
        public int? start { get; set; }
        public int? end { get; set; }
        public decimal? score { get; set; }
        public CodeableConcept method { get; set; }
        public decimal? truthTP { get; set; }
        public decimal? queryTP { get; set; }
        public decimal? truthFN { get; set; }
        public decimal? queryFP { get; set; }
        public decimal? gtFP { get; set; }
        public decimal? precision { get; set; }
        public decimal? recall { get; set; }
        public decimal? fScore { get; set; }
        public Roc roc { get; set; }
    }
}
