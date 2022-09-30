using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncDataModel.X12
{
    public class NTE : X12SegmentBase
    {
        public string NoteCode { get; set; }
        public string NoteText { get; set; }
        public NTE()
        {
            SegmentCode = "NTE";
        }
        public override bool Valid()
        {
            return !string.IsNullOrEmpty(NoteCode) && !string.IsNullOrEmpty(NoteText);
        }
        public override string ToX12String()
        {
            return "NTE*" + NoteCode + "*" + NoteText + "~";
        }
    }
}
