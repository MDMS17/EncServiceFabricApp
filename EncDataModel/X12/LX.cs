﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncDataModel.X12
{
    public class LX : X12SegmentBase
    {
        public string ServiceLineNumber { get; set; }
        public LX()
        {
            SegmentCode = "LX";
            LoopName = "2400";
        }
        public override bool Valid()
        {
            return true;
        }
        public override string ToX12String()
        {
            return "LX*" + ServiceLineNumber + "~";
        }
    }
}
