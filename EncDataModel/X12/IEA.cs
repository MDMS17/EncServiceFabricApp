using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncDataModel.X12
{
    public class IEA : X12SegmentBase
    {
        public string NumberofFunctionalGroups { get; set; }
        public string InterchangeControlNumber { get; set; }
        public IEA()
        {
            SegmentCode = "IEA";
            LoopName = "Trailer";
        }
        public override bool Valid()
        {
            return true;
        }
        public override string ToX12String()
        {
            return "IEA*" + NumberofFunctionalGroups + "*" + InterchangeControlNumber + "~";
        }
    }
}
