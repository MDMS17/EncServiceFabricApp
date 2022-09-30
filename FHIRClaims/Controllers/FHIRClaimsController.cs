using FHIR.DataModel.Financial;
using FHIR.DataModel.DataType;
using FHIRClaims.Data;
using EncDataModel.Submission837;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FHIRClaims.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class FHIRClaimsController:ControllerBase
    {
        private readonly ILogger<FHIRClaimsController> _logger;
        private readonly Sub837HistoryContext _context;
        public FHIRClaimsController(ILogger<FHIRClaimsController> logger, Sub837HistoryContext context)
        {
            _logger = logger;
            _context = context;
        }
        [HttpPost]
        public FHIR.DataModel.Financial.Claim RetrieveClaim([FromBody] List<string> parameters)
        {
            FHIR.DataModel.Financial.Claim result = new FHIR.DataModel.Financial.Claim();
            string ClaimId="";
            if (parameters[0] == "MemberId")
            {
                ClaimId = _context.ClaimSBRs.OrderByDescending(x => x.ID).FirstOrDefault(x => x.IDCode == parameters[1]).ClaimID;
            }
            else if (parameters[0] == "ProviderId") 
            {
                ClaimId = _context.ClaimProviders.OrderByDescending(x => x.ID).FirstOrDefault(x => x.ProviderID == parameters[1]).ClaimID;
            }
            result.identifier = new List<Identifier>();
            result.identifier.Add(new Identifier 
            {
                value=ClaimId
            });
            result.status = "Paid";
            result.type = new CodeableConcept 
            {
                text="FFS"
            };
            ClaimSBR sbr = _context.ClaimSBRs.OrderByDescending(x => x.ID).FirstOrDefault(x => x.ClaimID == ClaimId && x.SubscriberSequenceNumber == "S");
            result.patient = new Reference 
            {
                display=sbr.FirstName+" " + sbr.LastName
            };
            string serviceStartDate="", serviceEndDate="";
            ClaimHeader header = _context.ClaimHeaders.FirstOrDefault(x => x.ClaimID == ClaimId);
            if (header.ClaimType == "A")
            {
                serviceStartDate = header.StatementFromDate;
                serviceEndDate = header.StatementToDate;
            }
            else if (header.ClaimType == "B") 
            {
                serviceStartDate = _context.ServiceLines.Where(x => x.ClaimID == ClaimId).Min(x => x.ServiceFromDate);
                serviceEndDate = _context.ServiceLines.Where(x => x.ClaimID == ClaimId).Max(x => x.ServiceToDate);
            }
            result.billablePeriod = new Period
            {
                start = DateTime.Parse(serviceStartDate.Substring(4, 2) + "/" + serviceStartDate.Substring(6, 2) + "/" + serviceStartDate.Substring(0, 4)),
                end=DateTime.Parse(serviceEndDate.Substring(4, 2) + "/" + serviceEndDate.Substring(6, 2) + "/" + serviceEndDate.Substring(0, 4))
            };
            ClaimProvider provider_Billing = _context.ClaimProviders.FirstOrDefault(x => x.ClaimID == ClaimId && x.ProviderQualifier == "85");
            result.provider = new Reference 
            {
                 display= string.IsNullOrEmpty(provider_Billing.ProviderFirstName) ? provider_Billing.ProviderLastName : provider_Billing.ProviderFirstName + " " + provider_Billing.ProviderLastName
            };
            result.payee = new Payee 
            {
                party=new Reference 
                {
                    display=string.IsNullOrEmpty(provider_Billing.ProviderFirstName)?provider_Billing.ProviderLastName:provider_Billing.ProviderFirstName + " "+provider_Billing.ProviderLastName
                }
            };
            ClaimProvider provider_Referring = _context.ClaimProviders.FirstOrDefault(x => x.ClaimID == ClaimId && x.ProviderQualifier == "DN");
            if (provider_Referring != null) 
            {
                result.referral = new Reference 
                {
                    display= string.IsNullOrEmpty(provider_Referring.ProviderFirstName) ? provider_Referring.ProviderLastName : provider_Referring.ProviderFirstName + " " + provider_Referring.ProviderLastName
                };
            }
            ClaimProvider provider_Facility = _context.ClaimProviders.FirstOrDefault(x => x.ClaimID == ClaimId && x.ProviderQualifier == "77");
            if (provider_Facility != null) 
            {
                result.facility = new Reference 
                {
                    display= string.IsNullOrEmpty(provider_Facility.ProviderFirstName) ? provider_Facility.ProviderLastName : provider_Facility.ProviderFirstName + " " + provider_Facility.ProviderLastName
                };
            }
            ClaimHI hi = _context.ClaimHIs.FirstOrDefault(x => x.ClaimID == ClaimId && (x.HIQual == "ABK"||x.HIQual=="BK"));
            result.diagnosis = new List<Diagnosis>();
            if (hi != null) 
            {
                result.diagnosis.Add(new Diagnosis 
                {
                    sequence=1,
                    diagnosis=new DiagnosisClaim 
                    {
                         diagnosisCodeableConcept=new CodeableConcept 
                         {
                             text=hi.HICode
                         }
                    },
                    packageCode=new CodeableConcept 
                    {
                        text=hi.HIQual
                    }
                });
            }
            List<ClaimHI> his = _context.ClaimHIs.Where(x => x.ClaimID == ClaimId && (x.HIQual == "ABF"||x.HIQual=="BF")).ToList();
            if (his.Count > 0) 
            {
                for (int i = 0; i < his.Count; i++) 
                {
                    result.diagnosis.Add(new Diagnosis
                    {
                        sequence = (uint)i + 2,
                        diagnosis=new DiagnosisClaim 
                        {
                            diagnosisCodeableConcept=new CodeableConcept 
                            {
                                text=his[i].HICode
                            }
                        },
                        packageCode=new CodeableConcept 
                        {
                            text=his[i].HIQual
                        }
                    });
                }
            }
            hi = _context.ClaimHIs.FirstOrDefault(x => x.ClaimID == ClaimId && (x.HIQual == "BP"||x.HIQual=="BR"||x.HIQual=="BBR"));
            result.procedure = new List<Procedure>();
            if (hi != null) 
            {
                result.procedure.Add(new Procedure 
                {
                    sequence=1,
                    procedure=new ProcedureClaim 
                    {
                        procedureCodeableConcept=new CodeableConcept 
                        {
                            text=hi.HICode
                        }
                    }
                });
            }
            his = _context.ClaimHIs.Where(x => x.ClaimID == ClaimId && (x.HIQual == "BO" || x.HIQual == "BQ" || x.HIQual == "BBQ")).ToList();
            if (his.Count > 0) 
            {
                for (int i = 0; i < his.Count; i++) 
                {
                    result.procedure.Add(new Procedure
                    {
                        sequence = (uint)i+2,
                        procedure = new ProcedureClaim
                        {
                            procedureCodeableConcept = new CodeableConcept
                            {
                                text = his[i].HICode
                            }
                        }
                    });
                }
            }
            List<ServiceLine> lines = _context.ServiceLines.Where(x => x.ClaimID == ClaimId).ToList();
            result.item = new List<ItemClaim>();
            for (int i = 0; i < lines.Count; i++) 
            {
                ItemClaim item = new ItemClaim
                {
                    sequence = (uint)i + 1,
                    revenue = new CodeableConcept
                    {
                        text = lines[i].RevenueCode
                    },
                    productOrService = new CodeableConcept
                    {
                        text = lines[i].ProcedureCode
                    },
                    modifier = new List<CodeableConcept>(),
                    quantity=decimal.Parse(lines[i].ServiceUnitQuantity),
                    unitPrice=new Money 
                    {
                        value=Math.Round(decimal.Parse(lines[i].LineItemChargeAmount)/decimal.Parse(lines[i].ServiceUnitQuantity),2)
                    },
                    net = new Money 
                    {
                        value = decimal.Parse(lines[i].LineItemChargeAmount)
                    } 
                };
                if (!string.IsNullOrEmpty(lines[i].ProcedureModifier1)) 
                {
                    item.modifier.Add(new CodeableConcept 
                    {
                        text=lines[i].ProcedureModifier1
                    });
                }
                if (!string.IsNullOrEmpty(lines[i].ProcedureModifier2))
                {
                    item.modifier.Add(new CodeableConcept
                    {
                        text = lines[i].ProcedureModifier2
                    });
                }
                if (!string.IsNullOrEmpty(lines[i].ProcedureModifier3))
                {
                    item.modifier.Add(new CodeableConcept
                    {
                        text = lines[i].ProcedureModifier3
                    });
                }
                if (!string.IsNullOrEmpty(lines[i].ProcedureModifier4))
                {
                    item.modifier.Add(new CodeableConcept
                    {
                        text = lines[i].ProcedureModifier4
                    });
                }

                result.item.Add(item);
            }
            result.total = new Money
            {
                value = decimal.Parse(header.ClaimAmount)
            };
            return result;
        }
    }
}
