using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class EffectiveMedicationAdministration
    {
        public DateTime? effectiveDateTime { get; set; }
        public Period effectivePEriod { get; set; }
    }
}
