using FHIR.DataModel.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EncWeb.Models
{
    public class FHIRMemberModel
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public string VerificationCode { get; set; }
        public string ShowVerification { get; set; }
        public string ShowLogin { get; set; }
        public string Message { get; set; }
        public List<string> Parameters { get; set; }
        public Patient patient { get; set; }
        public string showMember { get; set; }
    }
}
