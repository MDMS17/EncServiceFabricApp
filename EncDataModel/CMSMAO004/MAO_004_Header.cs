using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace EncDataModel.CMSMAO004
{
    public class MAO_004_Header
    {
        [Key]
        public int Id { get; set; }
        public long TransmissionId { get; set; }
        public string ReportId { get; set; }
        public string ContractId { get; set; }
        public DateTime ReportDate { get; set; }
        public string ReportDescription { get; set; }
        public string SubmissionFileType { get; set; }
        public int RecordCount { get; set; }
        public string Phase { get; set; }
        public string Version { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
        public string FileName { get; set; }
    }
}
