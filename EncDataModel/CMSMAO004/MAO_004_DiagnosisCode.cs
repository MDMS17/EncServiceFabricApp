using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace EncDataModel.CMSMAO004
{
    public class MAO_004_DiagnosisCode
    {
        [Key]
        public int Id { get; set; }
        public int DetailId { get; set; }
        public string Code { get; set; }
        public string ICD { get; set; }
        public string Flag { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
