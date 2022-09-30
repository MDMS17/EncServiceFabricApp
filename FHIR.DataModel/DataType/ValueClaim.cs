using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class ValueClaim
    {
        public bool? valueBoolean { get; set; }
        public string valueString { get; set; }
        public decimal? valueQuantity { get; set; }
        public Attachment valueAttachment { get; set; }
        public Reference valueReference { get; set; }
    }
}
