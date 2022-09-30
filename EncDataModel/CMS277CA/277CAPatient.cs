using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EncDataModel.CMS277CA
{
    public class _277CAPatient
    {
        [Key]
        public long Id { get; set; }

        public int FileId { get; set; }
        public string ClaimId { get; set; }
        public string BillProvId { get; set; }
        public string PatientLastName { get; set; }
        public string PatientFirstName { get; set; }
        public string PatientMI { get; set; }
        public string PatientIdQual { get; set; }
        public string PatientId { get; set; }
        public string PayerClaimControlNumber { get; set; }
        public string ClearingHouseTraceNumber { get; set; }
        public string BillType { get; set; }
        public string ServiceDateFrom { get; set; }
        public string ServiceDateTo { get; set; }
    }
}
