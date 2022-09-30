using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EncDataModel.CMSMAO002
{
    public class MAO2Detail
    {
        [Key]
        public long DetailId { get; set; }

        public int FileId { get; set; }
        public string ClaimId { get; set; }
        public string InternalControlNumber { get; set; }
        public string LineNumber { get; set; }
        public string EncounterStatus { get; set; }
        public string ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
    }
}
