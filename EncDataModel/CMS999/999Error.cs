using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EncDataModel.CMS999
{
    public class _999Error
    {
        [Key]
        public int ErrorId { get; set; }

        public int FileId { get; set; }
        public string TransactionControlNumber { get; set; }
        public string SegmentCode { get; set; }
        public string PositionInTransaction { get; set; }
        public string LoopCode { get; set; }
        public string ErrorCode { get; set; }
        public string BusinessUnitName { get; set; }
        public string BusinessUnitCode { get; set; }
        public string CtxSegmentCode { get; set; }
        public string CtxPositionInTransaction { get; set; }
        public string CtxLoopCode { get; set; }
        public string CtxPositionInSegment { get; set; }
        public string CtxReferenceInSegment { get; set; }
    }
}
