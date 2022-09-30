using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
namespace EncDataModel.DHCS
{
    public class DHCSFile
    {
        [Key]
        public int FileId { get; set; }

        public string FileName { get; set; }
        public string EncounterFileName { get; set; }
        public string SubmitterName { get; set; }
        public string SubmissionDate { get; set; }
        public string ValidationStatus { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
