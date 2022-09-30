using FHIR.DataModel.Administration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EncWeb.Models
{
    public class FHIRProviderModel
    {
        public string UserId { get; set; }
        public string Password { get; set; }
        public string VerificationCode { get; set; }
        public string ShowVerification { get; set; }
        public string ShowLogin { get; set; }
        public string Message { get; set; }
        public List<string> Parameters { get; set; }
        public Practitioner practitioner { get; set; }
        public string showProvider { get; set; }
    }
}
