using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Used
    {
        public uint? usedUnsignedInt { get; set; }
        public Money usedMoney { get; set; }
    }
}
