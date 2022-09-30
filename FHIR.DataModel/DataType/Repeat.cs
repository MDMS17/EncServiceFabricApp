using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FHIR.DataModel.DataType
{
    public class Repeat : Element
    {
        public Bounds bounds { get; set; }
        public uint? count { get; set; }
        public uint? countMax { get; set; }
        public decimal? duration { get; set; }
        public decimal? durationMax { get; set; }
        public string durationUnit { get; set; }
        public uint? frequency { get; set; }
        public uint? frequencyMax { get; set; }
        public decimal? period { get; set; }
        public decimal? periodMax { get; set; }
        public string periodUnit { get; set; }
        public List<string> dayOfWeek { get; set; }
        public List<string> timeOfDay { get; set; }
        public List<string> when { get; set; }
        public uint? offset { get; set; }
    }
}
