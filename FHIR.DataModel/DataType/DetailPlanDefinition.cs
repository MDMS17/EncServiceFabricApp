using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class DetailPlanDefinition
    {
        public decimal? detailQuantity { get; set; }
        public Range detailRange { get; set; }
        public CodeableConcept detailCodeableConcept { get; set; }
    }
}
