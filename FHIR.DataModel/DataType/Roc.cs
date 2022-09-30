using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Roc : BackboneElement
    {
        public int? score { get; set; }
        public int? numTP { get; set; }
        public int? numFP { get; set; }
        public int? numFN { get; set; }
        public decimal? precision { get; set; }
        public decimal? sensitivity { get; set; }
        public decimal? fMeasure { get; set; }
    }
}
