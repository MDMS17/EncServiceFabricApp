using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EncDataModel.CMS999
{
    public class _999Transaction
    {
        [Key]
        public int TransactionId { get; set; }

        public int FileId { get; set; }
        public string TransactionControlNumber { get; set; }
        public string TransactionAckCode { get; set; }
        public string TransactionError1 { get; set; }
        public string TransactionError2 { get; set; }
        public string TransactionError3 { get; set; }
        public string TransactionError4 { get; set; }
        public string TransactionError5 { get; set; }
    }
}
