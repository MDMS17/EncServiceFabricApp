using EncDataModel.Submission837;
using FHIR.DataModel.Administration;
using FHIR.DataModel.DataType;
using FHIRMembers.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FHIRMembers.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FHIRMembersController : ControllerBase
    {
        private readonly ILogger<FHIRMembersController> _logger;
        private readonly Sub837HistoryContext _context;
        public FHIRMembersController(ILogger<FHIRMembersController> logger, Sub837HistoryContext context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpPost]
        public Patient RetrieveMember([FromBody] List<string> parameters)
        {
            Patient result = new Patient();
            result.identifier = new List<Identifier>();
            result.identifier.Add(new Identifier { 
                value=parameters[0]
            });
            result.active = true;
            ClaimSBR sbr = _context.ClaimSBRs.OrderByDescending(x => x.ID).FirstOrDefault(x => x.IDCode == parameters[0] && x.SubscriberSequenceNumber == "S");
            if (sbr == null) return result;
            result.name = new List<HumanName>();
            HumanName humanName = new HumanName();
            humanName.given = new List<string>();
            humanName.prefix = new List<string>();
            humanName.suffix = new List<string>();
            humanName.family = sbr.LastName;
            humanName.given.Add(sbr.FirstName);
            if (!string.IsNullOrEmpty(sbr.NameSuffix)) humanName.suffix.Add(sbr.NameSuffix);
            result.name.Add(humanName);
            result.gender = sbr.SubscriberGender;
            result.birthDate = DateTime.Parse(sbr.SubscriberBirthDate.Substring(4,2)+"/"+sbr.SubscriberBirthDate.Substring(6,2)+"/"+sbr.SubscriberBirthDate.Substring(0,4));
            result.address = new List<Address>();
            Address address = new Address();
            address.line = new List<string>();
            address.line.Add(sbr.SubscriberAddress);
            if (!string.IsNullOrEmpty(sbr.SubscriberAddress2)) address.line.Add(sbr.SubscriberAddress2);
            address.city = sbr.SubscriberCity;
            address.state = sbr.SubscriberState;
            address.postalCode = sbr.SubscriberZip;
            if (!string.IsNullOrEmpty(sbr.SubscriberCountry)) address.country = sbr.SubscriberCountry;
            result.address.Add(address);
            ClaimProvider provider = _context.ClaimProviders.OrderByDescending(x => x.ID).FirstOrDefault(x => x.ClaimID == sbr.ClaimID && x.ProviderQualifier == "85");
            result.generalPractitioner = new List<Reference>();
            result.generalPractitioner.Add(new Reference { 
                reference="Practitioner",
                type="BillProv",
                identifier=new Identifier { 
                    value=provider.ProviderID
                },
                display=provider.ProviderFirstName+" "+provider.ProviderLastName
            });
            return result;
        }
    }
}
