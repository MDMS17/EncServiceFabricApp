using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EncDataModel.CMS999
{
    public class _999File
    {
        [Key]
        public int FileId { get; set; }

        public string FileName { get; set; }
        public string ReceiverId { get; set; }
        public string SenderId { get; set; }
        public string ICN { get; set; }
        public string GroupControlNumber { get; set; }
        public string TransactionDate { get; set; }
        public string TransactionTime { get; set; }
        public string TransactionsIncluded { get; set; }
        public string TransactionsReceived { get; set; }
        public string TransactionsAccepted { get; set; }
        public string FileAckCode { get; set; }
        public string ProductionFlag { get; set; }
        public string FileError1 { get; set; }
        public string FileError2 { get; set; }
        public string FileError3 { get; set; }
        public string FileError4 { get; set; }
        public string FileError5 { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
