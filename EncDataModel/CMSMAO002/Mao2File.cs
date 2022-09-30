using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EncDataModel.CMSMAO002
{
    public class MAO2File
    {
        [Key]
        public int FileId { get; set; }

        public string FileName { get; set; }
        public string SenderId { get; set; }
        public string ICN { get; set; }
        public string TransactionDate { get; set; }
        public string RecordType { get; set; }
        public string ProductionFlag { get; set; }
        public string TotalErrors { get; set; }
        public string TotalLinesAccepted { get; set; }
        public string TotalLinesRejected { get; set; }
        public string TotalLinesSubmitted { get; set; }
        public string TotalEncountersAccepted { get; set; }
        public string TotalEncountersRejected { get; set; }
        public string TotalEncountersSubmitted { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
