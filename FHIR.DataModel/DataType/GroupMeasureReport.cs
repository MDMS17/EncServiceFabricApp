using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class GroupMeasureReport:BackboneElement 
    {
        public CodeableConcept code { get; set; }
        public List<PopulationMeasureReport> population { get; set; }
        public decimal? measureScore { get; set; }
        public List<StratifierMeasureReport> stratifier { get; set; }
    }
}
