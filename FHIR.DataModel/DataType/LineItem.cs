using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class LineItem : BackboneElement
    {
        public uint? sequence { get; set; }
        public ChargeItemInvoice chargeItem { get; set; }
        public List<PriceComponent> priceComponent { get; set; }
    }
}
