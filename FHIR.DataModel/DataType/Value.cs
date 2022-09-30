using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Value
    {
        public decimal? valueQuantity { get; set; }
        public CodeableConcept valueCodeableConcept { get; set; }
        public string valueString { get; set; }
        public bool? valueBoolean { get; set; }
        public int? valueInteger { get; set; }
        public Range valueRange { get; set; }
        public Ratio valueRatio { get; set; }
        public SampleData sampleData { get; set; }
        public DateTime? valueTime { get; set; }
        public DateTime? valueDateTime { get; set; }
        public Period valuePeriod { get; set; }
    }
}
