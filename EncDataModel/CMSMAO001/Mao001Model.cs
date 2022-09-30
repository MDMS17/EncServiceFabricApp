using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EncDataModel.CMSMAO001
{
    public class Mao001Header
    {
        [Key]
        public int HeaderId { get; set; }
        public string FileName { get; set; }
        public string SenderId { get; set; }
        public string InterchangeControlNumber { get; set; }
        public string InterchangeDate { get; set; }
        public string RecordType { get; set; }
        public string ProductionFlag { get; set; }
        public string TotalDupLines { get; set; }
        public string TotalLines { get; set; }
        public string TotalEncounters { get; set; }
    }
    public class Mao001Detail
    {
        [Key]
        public int DetailId { get; set; }
        public int HeaderId { get; set; }
        public string ContractId { get; set; }
        public string EncounterId { get; set; }
        public string InternalControlNumber { get; set; }
        public string LineNumber { get; set; }
        public string DupEncounterId { get; set; }
        public string DupInternalControlNumber { get; set; }
        public string DupLineNumber { get; set; }
        public string HICN { get; set; }
        public string DateOfService { get; set; }
        public string ErrorCode { get; set; }
    }
}
