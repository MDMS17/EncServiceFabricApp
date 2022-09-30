using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class ChargeItemInvoice
    {
        public Reference chargeItemReference { get; set; }
        public CodeableConcept chargeItemCodeableConcept { get; set; }
    }
}
