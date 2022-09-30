using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class DetailGoal
    {
        public decimal? detailQuantity { get; set; }
        public Range detailRange { get; set; }
        public CodeableConcept detailCodeableConcept { get; set; }
        public string detailString { get; set; }
        public bool? detailBoolean { get; set; }
        public int? detailInteger { get; set; }
        public Ratio detailRatio { get; set; }
    }
}
