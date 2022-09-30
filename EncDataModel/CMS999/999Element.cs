using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EncDataModel.CMS999
{
    public class _999Element
    {
        [Key]
        public int ElementId { get; set; }

        public int FileId { get; set; }
        public string TransactionControlNumber { get; set; }
        public string PositionInTransaction { get; set; }
        public string PositionInSegment { get; set; }
        public string ElementReferenceNumber { get; set; }
        public string ElementErrorCode { get; set; }
        public string ElementBadDataCopy { get; set; }
        public string ElementSegmentCode { get; set; }
        public string ElementSegmentPositionInTransaction { get; set; }
        public string ElementLoopCode { get; set; }
        public string ElementPositionInSegment { get; set; }
        public string ElementReferenceInSegment { get; set; }
    }
}
