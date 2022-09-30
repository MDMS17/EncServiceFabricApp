using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class ConditionAction:BackboneElement
    {
        public string kind { get; set; }
        public Expression expression { get; set; }
    }
}
