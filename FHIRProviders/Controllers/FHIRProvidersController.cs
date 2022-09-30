using EncDataModel.Submission837;
using FHIR.DataModel.Administration;
using FHIR.DataModel.DataType;
using FHIRProviders.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FHIRProviders.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FHIRProvidersController : ControllerBase
    {
        private readonly ILogger<FHIRProvidersController> _logger;
        private readonly Sub837HistoryContext _context;
        public FHIRProvidersController(ILogger<FHIRProvidersController> logger, Sub837HistoryContext context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpPost]
        public Practitioner RetrieveProvider([FromBody] List<string> parameters)
        {
            Practitioner result = new Practitioner();
            result.identifier = new List<Identifier>();
            result.identifier.Add(new Identifier
            {
                value = parameters[0]
            });
            result.active = true;
            result.name = new List<HumanName>();
            ClaimProvider provider = _context.ClaimProviders.OrderByDescending(x=>x.ID).FirstOrDefault(x => x.ProviderID == parameters[0] && x.ProviderQualifier == "85");
            if (provider == null) 
            {
                return result;
            }
            HumanName humanName = new HumanName 
            {
                family=provider.ProviderLastName
            };
            if (!string.IsNullOrEmpty(provider.ProviderFirstName)) 
            {
                humanName.given = new List<string>();
                humanName.given.Add(provider.ProviderFirstName);
            }
            if (!string.IsNullOrEmpty(provider.ProviderSuffix)) 
            {
                humanName.suffix = new List<string>();
                humanName.suffix.Add(provider.ProviderSuffix);
            }
            result.name.Add(humanName);
            ProviderContact providerContact = _context.ProviderContacts.FirstOrDefault(x => x.ClaimID == provider.ClaimID&&x.LoopName=="2010AA");
            if (providerContact!=null) 
            {
                result.telecom = new List<ContactPoint>();
                uint i = 1;
                if (!string.IsNullOrEmpty(providerContact.Phone)) 
                {
                    result.telecom.Add(new ContactPoint
                    {
                        system="Phone",
                        value = providerContact.Phone,
                        rank = i
                    });
                    i++;
                }
                if (!string.IsNullOrEmpty(providerContact.Email)) 
                {
                    result.telecom.Add(new ContactPoint 
                    {
                        system="Email",
                        value=providerContact.Email,
                        rank=i
                    });
                    i++;
                }
                if (!string.IsNullOrEmpty(providerContact.Fax)) 
                {
                    result.telecom.Add(new ContactPoint 
                    {
                        system="Fax",
                        value=providerContact.Fax,
                        rank=i
                    });
                }
            }
            result.address = new List<Address>();
            Address addr = new Address 
            {
                type="Office",
                city=provider.ProviderCity,
                state=provider.ProviderState,
                postalCode=provider.ProviderZip,
                country=provider.ProviderCountry
            };
            addr.line = new List<string>();
            addr.line.Add(provider.ProviderAddress);
            if (!string.IsNullOrEmpty(provider.ProviderAddress2)) 
            {
                addr.line.Add(provider.ProviderAddress2);
            }
            result.address.Add(addr);
            
            return result;
        }
    }
}
