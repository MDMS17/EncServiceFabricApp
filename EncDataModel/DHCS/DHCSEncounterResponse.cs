using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EncDataModel.DHCS
{
    public class DHCSEncounterResponse
    {
        [Key]
        public long EncounterResponseId { get; set; }

        public int FileId { get; set; }
        public string TransactionNumber { get; set; }
        public string EncounterReferenceNumber { get; set; }
        public string Severity { get; set; }
        public string IssueId { get; set; }
        public string IsSNIP { get; set; }
        public string IssueDescription { get; set; }
    }
}
