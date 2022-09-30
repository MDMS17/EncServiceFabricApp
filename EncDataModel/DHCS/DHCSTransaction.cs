using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EncDataModel.DHCS
{
    public class DHCSTransaction
    {
        [Key]
        public int TransactionId { get; set; }

        public int FileId { get; set; }
        public string TransactionStatus { get; set; }
        public string TransactionNumber { get; set; }
        public string ISAControlNumber { get; set; }
        public string GroupControlNumber { get; set; }
        public string OriginatorTransactionId { get; set; }
    }
}
