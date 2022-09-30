using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FHIR.DataModel.Financial;

namespace EncWeb.Models
{
    public class FHIRClaimModel
    {
        public Claim claim { get; set; }
        public List<string> Parameters { get; set; }
    }
}
