using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class StratumMeasureReport:BackboneElement 
    {
        public CodeableConcept value { get; set; }
        public List<ComponentMeasureReport> component { get; set; }
        public List<PopulationMeasureReport> population { get; set; }
        public decimal? measureScore { get; set; }
    }
}
