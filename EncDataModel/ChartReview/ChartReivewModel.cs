using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncDataModel.ChartReview
{
    public class ChartReviewRecord
    {
        [Key]
        public int Id { get; set; }
        public string ClaimType { get; set; }
        public string ProviderNpi { get; set; }
        public string MemberHicn { get; set; }
        public string MemberDOB { get; set; }
        public string DosFromDate { get; set; }
        public string DosToDate { get; set; }
        public string DiagnosisCode { get; set; }
        public string DeleteIndicator { get; set; }
        public string ProcedureCode { get; set; }
        public string RevenueCode { get; set; }
    }
    public class ChartReviewData
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Address { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string LastItem { get; set; }
    }
}
