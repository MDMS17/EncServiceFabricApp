using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Abatement
    {
        public DateTime abatementtDateTime { get; set; }
        public uint abatementAge { get; set; }
        public Period abatementPeriod { get; set; }
        public Range abatementRange { get; set; }
        public string abatementString { get; set; }
    }
}
