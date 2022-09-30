using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace EncDataModel.CMSMAO004
{
    public class MAO_004_Detail
    {
        [Key]
        public int Id { get; set; }
        public int HeaderId { get; set; }
        public string ReportId { get; set; }
        public string MedicareContractId { get; set; }
        public string BeneficiaryHICN { get; set; }
        public string EncounterICN { get; set; }
        public string EncounterTypeSwitch { get; set; }
        public string OriginalEncounterICN { get; set; }
        public string OriginalEncounterStatus { get; set; }
        public string AllowDisallowFlag { get; set; }
        public string AllowDisallowReasonCode { get; set; }
        public DateTime SubmissionDate { get; set; }
        public DateTime ServiceStartDate { get; set; }
        public DateTime ServiceEndDate { get; set; }
        public string ClaimType { get; set; }
        public string CreateUser { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
