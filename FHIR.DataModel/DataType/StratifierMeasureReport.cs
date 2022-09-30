using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class StratifierMeasureReport:BackboneElement 
    {
        public CodeableConcept code { get; set; }
        public List<StratumMeasureReport> stratum { get; set; }
    }
}
