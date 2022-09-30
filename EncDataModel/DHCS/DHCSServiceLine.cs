using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EncDataModel.DHCS
{
    public class DHCSServiceLine
    {
        [Key]
        public long ServiceLineId { get; set; }

        public int FileId { get; set; }
        public string TransactionNumber { get; set; }
        public string EncounterReferenceNumber { get; set; }
        public string Line { get; set; }
        public string Severity { get; set; }
        public string Id { get; set; }
        public string Description { get; set; }
    }
}
