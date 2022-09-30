using EncDataModel.Submission837;
using Load837File.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using X12Lib;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Load837File.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Load837FileController : ControllerBase
    {
        private readonly ILogger<Load837FileController> _logger;
        private readonly Sub837CacheContext _context;
        private readonly Sub837ErrorContext _contextError;
        private readonly MemberContext _contextMember;
        public Load837FileController(ILogger<Load837FileController> logger, Sub837CacheContext context, Sub837ErrorContext contextError, MemberContext contextMember)
        {
            _logger = logger;
            _context = context;
            _contextError = contextError;
            _contextMember = contextMember;
        }
        //Load837File
        [HttpGet]
        public List<string> GetSource837Files()
        {
            _logger.Log(LogLevel.Information, "inquiry source 837 files");
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string sourcePath = configuration["Source_837File"];
            string archivePath = configuration["Archive_837File"];
            List<string> result = Directory.EnumerateFiles(sourcePath).Select(x => Path.GetFileName(x)).ToList();
            result.Insert(0, archivePath);
            result.Insert(0, sourcePath);
            return result;
        }
        //Load837File/1
        [HttpGet("{id}")]
        public List<string> Load837Files(long id)
        {
            List<string> result = new List<string>();
            _logger.Log(LogLevel.Information, "Load 837 files to database");
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string sourcePath = configuration["Source_837File"];
            string archivePath = configuration["Archive_837File"];
            string outputPath = configuration["Output_837File"];
            DirectoryInfo di = new DirectoryInfo(sourcePath);
            FileInfo[] fis = di.GetFiles();
            int totalFiles = fis.Length;
            int goodFiles = 0;
            foreach (FileInfo fi in fis)
            {
                _logger.Log(LogLevel.Information, "Processing file " + fi.Name + " now...");
                string s837 = System.IO.File.ReadAllText(fi.FullName);
                s837 = s837.Replace("\r", "").Replace("\n", "");
                string[] s837Lines = s837.Split('~');
                s837 = null;
                int encounterCount = s837Lines.Count(x => x.StartsWith("CLM*"));
                if (encounterCount > 0)
                {
                    List<Claim> claims = new List<Claim>();
                    SubmissionLog submittedFile = new SubmissionLog 
                    {
                        FileName = fi.Name,
                        FilePath = sourcePath,
                        EncounterCount = encounterCount,
                        CreatedDate = DateTime.Now
                    };
                    _context.SubmissionLogs.Add(submittedFile);
                    _context.SaveChanges();
                    X12Parser.Parse837File(ref s837Lines, ref claims, ref submittedFile);
                    List<MemberDetail> allMembers = new List<MemberDetail>();
                    List<Claim> validClaims = new List<Claim>();
                    List<Claim> errorClaims = new List<Claim>();
                    List<ClaimValidationError> validationErrors = new List<ClaimValidationError>();
                    //CheckMemberEligibility(ref allMembers, ref claims, ref validClaims, ref errorClaims, ref submittedFile, ref validationErrors);
                    //EnrichClaim(ref allMembers, ref claims, ref validClaims, ref errorClaims, ref submittedFile, ref validationErrors);
                    ValidateClaim(ref allMembers, ref claims, ref validClaims, ref errorClaims, ref submittedFile, ref validationErrors);
                    SaveValidClaims(ref validClaims);
                    if (errorClaims.Count > 0) 
                    {
                        SaveErrorClaims(ref errorClaims);
                    }
                    int transactionCount = s837Lines.Count(x => x.StartsWith("ST*"));
                    string destinationPath999 = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(fi.Name) + "_999.txt");
                    Generate999(ref submittedFile, transactionCount, encounterCount, destinationPath999);
                    string destinationPath = Path.Combine(outputPath, Path.GetFileNameWithoutExtension(fi.Name) + "_EVR.txt");
                    GenerateEvr(ref submittedFile, ref validationErrors, encounterCount, validClaims.Select(x => x.Header).Count(), destinationPath);
                    goodFiles++;
                }
                MoveFile(archivePath, fi);
            }
            result.Add(totalFiles.ToString());
            result.Add(goodFiles.ToString());
            return result;
        }
        private void CheckMemberEligibility(ref List<MemberDetail> allMembers, ref List<Claim> claims, ref List<Claim> validClaims, ref List<Claim> errorClaims, ref SubmissionLog submittedFile, ref List<ClaimValidationError> validationErrors) 
        {
            //check member eligibility (90)
            foreach (Claim c1 in claims)
            {

                string serviceDate = c1.Header.StatementFromDate;
                if (!string.IsNullOrEmpty(c1.Lines.Select(x => x.ServiceFromDate).Min()))
                {
                    serviceDate = c1.Lines.Select(x => x.ServiceFromDate).Min();
                }
                string memberNumber = c1.Subscribers.Select(x => x.IDCode).OrderByDescending(x => x).FirstOrDefault();
                List<MemberDetail> memberDetails = _contextMember.MemberDetails.FromSqlRaw(SqlMember + $" @MemberNumber={memberNumber}, @ServiceDate={serviceDate}").ToList();
                if (memberDetails.Count == 0)
                {
                    c1.Header.ExceptionCode = "90";
                    validationErrors.Add(new ClaimValidationError
                    {
                        ClaimId = c1.Header.ClaimID,
                        ErrorSequencePerEncounter = 1,
                        ErrorId = "90",
                        ErrorSeverityName = "Error",
                        ErrorDescription = "Member not eligible in date of service",
                        LoopNumber = "2320",
                        ElementName = "NM109"
                    });
                    continue;
                }
                foreach (MemberDetail detail in memberDetails) detail.ClaimId = c1.Header.ClaimID;
                if (memberDetails.Count == 1) allMembers.Add(memberDetails.FirstOrDefault());
                else allMembers.Add(memberDetails.FirstOrDefault(x => x.LineOfBusiness == "CCI"));
            }
        }
        private void EnrichClaim(ref List<MemberDetail> allMembers, ref List<Claim> claims, ref List<Claim> validClaims, ref List<Claim> errorClaims, ref SubmissionLog submittedFile, ref List<ClaimValidationError> validationErrors) 
        {
            foreach (Claim c1 in claims)
            {
                c1.Header.ClaimID = allMembers.FirstOrDefault(x => x.ClaimId == c1.Header.ClaimID).HCP + c1.Header.ClaimID;
                
                foreach (var v1 in c1.Cases) v1.ClaimID = c1.Header.ClaimID;
                foreach (var v1 in c1.CRCs) v1.ClaimID = c1.Header.ClaimID;
                foreach (var v1 in c1.His) v1.ClaimID = c1.Header.ClaimID;
                foreach (var v1 in c1.K3s) v1.ClaimID = c1.Header.ClaimID;
                foreach (var v1 in c1.FRMs) v1.ClaimID = c1.Header.ClaimID;
                foreach (var v1 in c1.LQs) v1.ClaimID = c1.Header.ClaimID;
                foreach (var v1 in c1.Meas) v1.ClaimID = c1.Header.ClaimID;
                foreach (var v1 in c1.SVDs) v1.ClaimID = c1.Header.ClaimID;
                foreach (var v1 in c1.Notes) v1.ClaimID = c1.Header.ClaimID;
                foreach (var v1 in c1.Patients) v1.ClaimID = c1.Header.ClaimID;
                foreach (var v1 in c1.Providers) v1.ClaimID = c1.Header.ClaimID;
                foreach (var v1 in c1.PWKs) v1.ClaimID = c1.Header.ClaimID;
                foreach (var v1 in c1.Subscribers) v1.ClaimID = c1.Header.ClaimID;
                foreach (var v1 in c1.SecondaryIdentifications) v1.ClaimID = c1.Header.ClaimID;
                foreach (var v1 in c1.ProviderContacts) v1.ClaimID = c1.Header.ClaimID;
                foreach (var v1 in c1.Lines) v1.ClaimID = c1.Header.ClaimID;
                foreach (var v1 in c1.ToothStatuses) v1.ClaimID = c1.Header.ClaimID;
            }
        }
        private void ValidateClaim(ref List<MemberDetail> allMembers, ref List<Claim> claims, ref List<Claim> validClaims, ref List<Claim> errorClaims, ref SubmissionLog submittedFile, ref List<ClaimValidationError> validationErrors) 
        {
            //validation, check same file dups(53)
            bool hasError = false;
            if (submittedFile.FileType == "005010X223A2") //institutional
            {
                List<DupLineI> allLines = new List<DupLineI>();
                foreach (Claim c1 in claims.Where(x => x.Header.ExceptionCode != "90"))
                {
                    string memberid = "", servicedatefrom = "", servicedateto = "", admissiondate = "", dischargehour = "", attendingprovid = "", renderingprovid = "", revenuecode = "", procedurecode = "", modifier1 = "", modifier2 = "", modifier3 = "", modifier4 = "", ndc = "";
                    var v1 = allMembers.FirstOrDefault(x => x.ClaimId == c1.Header.ClaimID);
                    if (v1 != null)
                    {
                        memberid = string.IsNullOrEmpty(v1.HIC) ? v1.CIN : v1.HIC;
                    }
                    else 
                    {
                        memberid = c1.Subscribers.Select(x => x.IDCode).OrderByDescending(x => x).FirstOrDefault();
                    }
                    var v2 = c1.Providers.FirstOrDefault(x => x.ProviderQualifier == "82");
                    if (v2 != null)
                    {
                        renderingprovid = v2.ProviderID;
                    }
                    else
                    {
                        var v3 = c1.Providers.FirstOrDefault(x => x.ProviderQualifier == "85");
                        if (v3 != null) renderingprovid = v3.ProviderID;
                    }
                    admissiondate = c1.Header.AdmissionDate;
                    dischargehour = c1.Header.DischargeDate;
                    var v4 = c1.Providers.FirstOrDefault(x => x.ProviderQualifier == "71");
                    if (v4 != null) attendingprovid = v4.ProviderID;
                    foreach (ServiceLine l1 in c1.Lines)
                    {
                        servicedatefrom = l1.ServiceFromDate;
                        if (string.IsNullOrEmpty(servicedatefrom)) servicedatefrom = c1.Header.StatementFromDate;
                        servicedateto = l1.ServiceToDate;
                        if (string.IsNullOrEmpty(servicedateto)) servicedateto = c1.Header.StatementToDate;
                        revenuecode = l1.RevenueCode;
                        procedurecode = l1.ProcedureCode;
                        modifier1 = l1.ProcedureModifier1;
                        modifier2 = l1.ProcedureModifier2;
                        modifier3 = l1.ProcedureModifier3;
                        modifier4 = l1.ProcedureModifier4;
                        ndc = l1.NationalDrugCode;
                        string temp = "";
                        if (string.Compare(modifier1, modifier2) < 0) { temp = modifier1; modifier1 = modifier2; modifier2 = temp; }
                        if (string.Compare(modifier3, modifier4) < 0) { temp = modifier3; modifier3 = modifier4; modifier4 = temp; }
                        if (string.Compare(modifier1, modifier3) < 0) { temp = modifier1; modifier1 = modifier3; modifier3 = temp; }
                        if (string.Compare(modifier2, modifier4) < 0) { temp = modifier2; modifier2 = modifier4; modifier4 = temp; }
                        if (string.Compare(modifier2, modifier3) < 0) { temp = modifier2; modifier2 = modifier3; modifier3 = temp; }

                        allLines.Add(new DupLineI
                        {
                            MemberId = memberid,
                            ServiceDateFrom = servicedatefrom,
                            ServiceDateTo = servicedateto,
                            AdmissionDate = admissiondate,
                            DischargeHour = dischargehour,
                            AttendingProvId = attendingprovid,
                            RenderingProvId = renderingprovid,
                            RevenueCode = revenuecode,
                            ProcedureCode = procedurecode,
                            Modifier1 = modifier1,
                            Modifier2 = modifier2,
                            Modifier3 = modifier3,
                            Modifier4 = modifier4,
                            NDC = ndc
                        });
                    }
                }
                foreach (Claim c1 in claims.Where(x => x.Header.ExceptionCode != "90"))
                {
                    hasError = false;
                    foreach (ServiceLine l1 in c1.Lines)
                    {
                        string cin = allMembers.Where(x => x.ClaimId == c1.Header.ClaimID).Select(x => string.IsNullOrEmpty(x.CIN) ? x.HIC : x.CIN).FirstOrDefault();
                        if (string.IsNullOrEmpty(cin)) 
                        {
                            cin = c1.Subscribers.Select(x => x.IDCode).OrderByDescending(x => x).FirstOrDefault();
                        }
                        string servicefrom = l1.ServiceFromDate;
                        if (string.IsNullOrEmpty(servicefrom)) servicefrom = c1.Header.StatementFromDate;
                        string serviceto = l1.ServiceToDate;
                        if (string.IsNullOrEmpty(serviceto)) serviceto = c1.Header.StatementToDate;
                        string admission = c1.Header.AdmissionDate;
                        string discharge = c1.Header.DischargeDate;
                        string attending = c1.Providers.Where(x => x.ProviderQualifier == "71").Select(x => x.ProviderID).FirstOrDefault();
                        string rendering = c1.Providers.Where(x => x.ProviderQualifier == "82").Select(x => x.ProviderID).FirstOrDefault();
                        if (string.IsNullOrEmpty(rendering)) rendering = c1.Providers.Where(x => x.ProviderQualifier == "85").Select(x => x.ProviderID).FirstOrDefault();
                        string revenue = l1.RevenueCode;
                        string procedure = l1.ProcedureCode;
                        string modifier1 = l1.ProcedureModifier1;
                        string modifier2 = l1.ProcedureModifier2;
                        string modifier3 = l1.ProcedureModifier3;
                        string modifier4 = l1.ProcedureModifier4;
                        string ndc = l1.NationalDrugCode;
                        string temp = "";
                        if (string.Compare(modifier1, modifier2) < 0) { temp = modifier1; modifier1 = modifier2; modifier2 = temp; }
                        if (string.Compare(modifier3, modifier4) < 0) { temp = modifier3; modifier3 = modifier4; modifier4 = temp; }
                        if (string.Compare(modifier1, modifier3) < 0) { temp = modifier1; modifier1 = modifier3; modifier3 = temp; }
                        if (string.Compare(modifier2, modifier4) < 0) { temp = modifier2; modifier2 = modifier4; modifier4 = temp; }
                        if (string.Compare(modifier2, modifier3) < 0) { temp = modifier2; modifier2 = modifier3; modifier3 = temp; }
                        if (allLines.Count(x => x.MemberId == cin && x.ServiceDateFrom == servicefrom && x.ServiceDateTo == serviceto && x.AdmissionDate == admission && x.DischargeHour == discharge && x.AttendingProvId == attending && x.RenderingProvId == rendering && x.RevenueCode == revenue && x.ProcedureCode == procedure && x.Modifier1 == modifier1 && x.Modifier2 == modifier2 && x.Modifier3 == modifier3 && x.Modifier4 == modifier4 && (string.IsNullOrEmpty(x.NDC) || string.IsNullOrEmpty(ndc) || (!string.IsNullOrEmpty(x.NDC) && !string.IsNullOrEmpty(ndc) && x.NDC == ndc))) > 1)
                        {
                            c1.Header.ExceptionCode = "53";
                            validationErrors.Add(new ClaimValidationError
                            {
                                ClaimId = l1.ClaimID,
                                ErrorSequencePerEncounter = int.Parse(l1.ServiceLineNumber),
                                ErrorId = "53",
                                ErrorSeverityName = "Error",
                                ErrorDescription = "Duplicate in the same file",
                                LoopNumber = "2400",
                                ElementName = "SV2"
                            });
                            hasError = true;
                        }
                    }
                    if (hasError)
                    {
                        errorClaims.Add(c1);
                    }
                    else
                    {
                        validClaims.Add(c1);
                    }
                }
            }
            else                                          //professional
            {
                List<DupLineP> allLines = new List<DupLineP>();
                foreach (Claim c1 in claims.Where(x => x.Header.ExceptionCode != "90"))
                {
                    string memberid = "", servicedatefrom = "", servicedateto = "", renderingprovid = "", procedurecode = "", modifier1 = "", modifier2 = "", modifier3 = "", modifier4 = "", ndc = "";
                    var v1 = allMembers.FirstOrDefault(x => x.ClaimId == c1.Header.ClaimID);
                    if (v1 != null)
                    {
                        memberid = string.IsNullOrEmpty(v1.HIC) ? v1.CIN : v1.HIC;
                    }
                    else 
                    {
                        memberid = c1.Subscribers.Select(x => x.IDCode).OrderByDescending(x => x).FirstOrDefault();
                    }
                    var v2 = c1.Providers.FirstOrDefault(x => x.ProviderQualifier == "82");
                    if (v2 != null)
                    {
                        renderingprovid = v2.ProviderID;
                    }
                    else
                    {
                        var v3 = c1.Providers.FirstOrDefault(x => x.ProviderQualifier == "85");
                        if (v3 != null) renderingprovid = v3.ProviderID;
                    }
                    foreach (ServiceLine l1 in c1.Lines)
                    {
                        servicedatefrom = l1.ServiceFromDate;
                        servicedateto = l1.ServiceToDate;
                        procedurecode = l1.ProcedureCode;
                        modifier1 = l1.ProcedureModifier1;
                        modifier2 = l1.ProcedureModifier2;
                        modifier3 = l1.ProcedureModifier3;
                        modifier4 = l1.ProcedureModifier4;
                        ndc = l1.NationalDrugCode;
                        string temp = "";
                        if (string.Compare(modifier1, modifier2) < 0) { temp = modifier1; modifier1 = modifier2; modifier2 = temp; }
                        if (string.Compare(modifier3, modifier4) < 0) { temp = modifier3; modifier3 = modifier4; modifier4 = temp; }
                        if (string.Compare(modifier1, modifier3) < 0) { temp = modifier1; modifier1 = modifier3; modifier3 = temp; }
                        if (string.Compare(modifier2, modifier4) < 0) { temp = modifier2; modifier2 = modifier4; modifier4 = temp; }
                        if (string.Compare(modifier2, modifier3) < 0) { temp = modifier2; modifier2 = modifier3; modifier3 = temp; }

                        allLines.Add(new DupLineP
                        {
                            MemberId = memberid,
                            ServiceDateFrom = servicedatefrom,
                            ServiceDateTo = servicedateto,
                            RenderingProvId = renderingprovid,
                            ProcedureCode = procedurecode,
                            Modifier1 = modifier1,
                            Modifier2 = modifier2,
                            Modifier3 = modifier3,
                            Modifier4 = modifier4,
                            NDC = ndc
                        });
                    }
                }

                foreach (Claim c1 in claims.Where(x => x.Header.ExceptionCode != "90"))
                {
                    hasError = false;
                    foreach (ServiceLine l1 in c1.Lines)
                    {
                        string cin = allMembers.Where(x => x.ClaimId == c1.Header.ClaimID).Select(x => string.IsNullOrEmpty(x.CIN) ? x.HIC : x.CIN).FirstOrDefault();
                        if (string.IsNullOrEmpty(cin))
                        {
                            cin = c1.Subscribers.Select(x => x.IDCode).OrderByDescending(x => x).FirstOrDefault();
                        }
                        string servicefrom = l1.ServiceFromDate;
                        string serviceto = l1.ServiceToDate;
                        string rendering = c1.Providers.Where(x => x.ProviderQualifier == "82").Select(x => x.ProviderID).FirstOrDefault();
                        if (string.IsNullOrEmpty(rendering)) rendering = c1.Providers.Where(x => x.ProviderQualifier == "85").Select(x => x.ProviderID).FirstOrDefault();
                        string procedure = l1.ProcedureCode;
                        string modifier1 = l1.ProcedureModifier1;
                        string modifier2 = l1.ProcedureModifier2;
                        string modifier3 = l1.ProcedureModifier3;
                        string modifier4 = l1.ProcedureModifier4;
                        string ndc = l1.NationalDrugCode;
                        string temp = "";
                        if (string.Compare(modifier1, modifier2) < 0) { temp = modifier1; modifier1 = modifier2; modifier2 = temp; }
                        if (string.Compare(modifier3, modifier4) < 0) { temp = modifier3; modifier3 = modifier4; modifier4 = temp; }
                        if (string.Compare(modifier1, modifier3) < 0) { temp = modifier1; modifier1 = modifier3; modifier3 = temp; }
                        if (string.Compare(modifier2, modifier4) < 0) { temp = modifier2; modifier2 = modifier4; modifier4 = temp; }
                        if (string.Compare(modifier2, modifier3) < 0) { temp = modifier2; modifier2 = modifier3; modifier3 = temp; }
                        if (allLines.Count(x => x.MemberId == cin && x.ServiceDateFrom == servicefrom && x.ServiceDateTo == serviceto && x.RenderingProvId == rendering && x.ProcedureCode == procedure && x.Modifier1 == modifier1 && x.Modifier2 == modifier2 && x.Modifier3 == modifier3 && x.Modifier4 == modifier4 && (string.IsNullOrEmpty(x.NDC) || string.IsNullOrEmpty(ndc) || (!string.IsNullOrEmpty(x.NDC) && !string.IsNullOrEmpty(ndc) && x.NDC == ndc))) > 1)
                        {
                            c1.Header.ExceptionCode = "53";
                            validationErrors.Add(new ClaimValidationError
                            {
                                ClaimId = l1.ClaimID,
                                ErrorSequencePerEncounter = int.Parse(l1.ServiceLineNumber),
                                ErrorId = "53",
                                ErrorSeverityName = "Error",
                                ErrorDescription = "Duplicate in the same file",
                                LoopNumber = "2400",
                                ElementName = "SV1"
                            });
                            hasError = true;
                        }
                    }
                    //check duplicates (52)

                    //check critical data elements (63)
                    if (hasError)
                    {
                        errorClaims.Add(c1);
                    }
                    else
                    {
                        validClaims.Add(c1);
                    }
                }
            }
        }
        private void SaveValidClaims(ref List<Claim> validClaims) 
        {
            _context.ClaimHeaders.AddRange(validClaims.Select(x => x.Header).ToList());
            _context.ClaimCAS.AddRange(validClaims.SelectMany(x => x.Cases).ToList());
            _context.ClaimCRCs.AddRange(validClaims.SelectMany(x => x.CRCs).ToList());
            _context.ClaimHIs.AddRange(validClaims.SelectMany(x => x.His).ToList());
            _context.ClaimK3s.AddRange(validClaims.SelectMany(x => x.K3s).ToList());
            _context.ClaimLineFRMs.AddRange(validClaims.SelectMany(x => x.FRMs).ToList());
            _context.ClaimLineLQs.AddRange(validClaims.SelectMany(x => x.LQs).ToList());
            _context.ClaimLineMEAs.AddRange(validClaims.SelectMany(x => x.Meas).ToList());
            _context.ClaimLineSVDs.AddRange(validClaims.SelectMany(x => x.SVDs).ToList());
            _context.ClaimNtes.AddRange(validClaims.SelectMany(x => x.Notes).ToList());
            _context.ClaimPatients.AddRange(validClaims.SelectMany(x => x.Patients).ToList());
            _context.ClaimProviders.AddRange(validClaims.SelectMany(x => x.Providers).ToList());
            _context.ClaimPWKs.AddRange(validClaims.SelectMany(x => x.PWKs).ToList());
            _context.ClaimSBRs.AddRange(validClaims.SelectMany(x => x.Subscribers).ToList());
            _context.ClaimSecondaryIdentifications.AddRange(validClaims.SelectMany(x => x.SecondaryIdentifications).ToList());
            _context.ProviderContacts.AddRange(validClaims.SelectMany(x => x.ProviderContacts).ToList());
            _context.ServiceLines.AddRange(validClaims.SelectMany(x => x.Lines).ToList());
            _context.ToothStatus.AddRange(validClaims.SelectMany(x => x.ToothStatuses).ToList());
            _context.SaveChanges();

        }
        private void SaveErrorClaims(ref List<Claim> errorClaims) 
        {
            _contextError.ClaimHeaders.AddRange(errorClaims.Select(x => x.Header).ToList());
            _contextError.ClaimCAS.AddRange(errorClaims.SelectMany(x => x.Cases).ToList());
            _contextError.ClaimCRCs.AddRange(errorClaims.SelectMany(x => x.CRCs).ToList());
            _contextError.ClaimHIs.AddRange(errorClaims.SelectMany(x => x.His).ToList());
            _contextError.ClaimK3s.AddRange(errorClaims.SelectMany(x => x.K3s).ToList());
            _contextError.ClaimLineFRMs.AddRange(errorClaims.SelectMany(x => x.FRMs).ToList());
            _contextError.ClaimLineLQs.AddRange(errorClaims.SelectMany(x => x.LQs).ToList());
            _contextError.ClaimLineMEAs.AddRange(errorClaims.SelectMany(x => x.Meas).ToList());
            _contextError.ClaimLineSVDs.AddRange(errorClaims.SelectMany(x => x.SVDs).ToList());
            _contextError.ClaimNtes.AddRange(errorClaims.SelectMany(x => x.Notes).ToList());
            _contextError.ClaimPatients.AddRange(errorClaims.SelectMany(x => x.Patients).ToList());
            _contextError.ClaimProviders.AddRange(errorClaims.SelectMany(x => x.Providers).ToList());
            _contextError.ClaimPWKs.AddRange(errorClaims.SelectMany(x => x.PWKs).ToList());
            _contextError.ClaimSBRs.AddRange(errorClaims.SelectMany(x => x.Subscribers).ToList());
            _contextError.ClaimSecondaryIdentifications.AddRange(errorClaims.SelectMany(x => x.SecondaryIdentifications).ToList());
            _contextError.ProviderContacts.AddRange(errorClaims.SelectMany(x => x.ProviderContacts).ToList());
            _contextError.ServiceLines.AddRange(errorClaims.SelectMany(x => x.Lines).ToList());
            _contextError.ToothStatus.AddRange(errorClaims.SelectMany(x => x.ToothStatuses).ToList());
            _contextError.SaveChanges();
        }
        private void Generate999(ref SubmissionLog submittedFile, int transactionCount, int recordCount, string destinationPath999) 
        {
            System.IO.File.WriteAllText(destinationPath999, X12Exporter.Export999(ref submittedFile, transactionCount, recordCount));
        }
        private void GenerateEvr(ref SubmissionLog submittedFile, ref List<ClaimValidationError> errors, int RecordCount, int validCount, string destinationPath) 
        {
            CalculatedCounts evrCount = new CalculatedCounts();
            evrCount.ValidationErrors = errors;
            evrCount.RecordCount = RecordCount;
            evrCount.ProcessedCount = validCount;
            evrCount.TransmissionmName = submittedFile.FileName;
            evrCount.RejectedCount = RecordCount - validCount;
            evrCount.Now = DateTime.Now;
            evrCount.ExemptDuplicateCount = errors.Where(x => x.ErrorId=="52" || x.ErrorId=="53").Select(x=>x.ClaimId).Distinct().Count();
            evrCount.ExemptMemberNotEligibleCount = errors.Where(x => x.ErrorId=="90").Select(x=>x.ClaimId).Distinct().Count();
            evrCount.ExemptInProgressCount = errors.Where(x => x.ErrorId=="63").Select(x=>x.ClaimId).Distinct().Count();
            evrCount.EligibileForIehpEditChecks = RecordCount - evrCount.ExemptDuplicateCount - evrCount.ExemptMemberNotEligibleCount - evrCount.ExemptInProgressCount;
            evrCount.InvalidCount = errors.Select(x => x.ClaimId).Distinct().Count() - evrCount.ExemptDuplicateCount - evrCount.ExemptMemberNotEligibleCount - evrCount.ExemptInProgressCount;
            evrCount.ValidCount = validCount;
            evrCount.Validity = evrCount.ValidCount / (float)evrCount.EligibileForIehpEditChecks * 100;
            System.IO.File.WriteAllText(destinationPath, X12Exporter.EVRReport(evrCount));
        }
        private void MoveFile(string archivePath, FileInfo fi)
        {
            if (System.IO.File.Exists(Path.Combine(archivePath, fi.Name))) System.IO.File.Delete(Path.Combine(archivePath, fi.Name));
            fi.MoveTo(Path.Combine(archivePath, fi.Name));
        }
        private const string SqlMember = @";with cte as (
select c1.MemberId,c1.MemberNumber,c1.Gender,c2.GroupNumber,c1.HICN,c3.MedicaidID,c1.PolicyNumber,c1.MBI,c1.DateOfBirth,c2.EffectiveDate,c2.ExpirationDate,c1.SocialSecurityNumber 
from meditrac.member c1 
cross apply(select top 1 * from meditrac.MemberBenefitCoverage where memberid=c1.memberid order by groupnumber desc) c2 
left join diamond.member c3 on c3.membernumber=c1.membernumber)
select left(isnull(coalesce(m1.membernumber,m2.membernumber,m3.membernumber),isnull(m4.membernumber,m5.membernumber)),12) as SubNumber,
right(isnull(coalesce(m1.membernumber,m2.membernumber,m3.membernumber),isnull(m4.membernumber,m5.membernumber)),2) as PersNumber,
isnull(coalesce(m1.Gender,m2.Gender,m3.Gender),isnull(m4.Gender,m5.Gender)) as Gender,
CASE WHEN (LEFT(isnull(coalesce(m1.GroupNumber,m2.groupnumber,m3.groupnumber),isnull(m4.groupnumber,m5.groupnumber)), 3) = 'H53') THEN 'CMC' 
WHEN (LEFT(isnull(coalesce(m1.GroupNumber,m2.groupnumber,m3.groupnumber),isnull(m4.groupnumber,m5.groupnumber)), 3) = '305') THEN 'MED' 
WHEN (LEFT(isnull(coalesce(m1.GroupNumber,m2.groupnumber,m3.groupnumber),isnull(m4.groupnumber,m5.groupnumber)), 3) = '306') THEN 'MED' 
WHEN (LEFT(isnull(coalesce(m1.GroupNumber,m2.groupnumber,m3.groupnumber),isnull(m4.groupnumber,m5.groupnumber)), 3) = '810') THEN 'CCI' 
WHEN (LEFT(isnull(coalesce(m1.GroupNumber,m2.groupnumber,m3.groupnumber),isnull(m4.groupnumber,m5.groupnumber)), 3) = '812') THEN 'CCI' 
ELSE 'EXX' END AS LineOfBusiness,
left(isnull(coalesce(m1.GroupNumber,m2.groupnumber,m3.groupnumber),isnull(m4.groupnumber,m5.groupnumber)),3) as HCP,
isnull(coalesce(m1.hicn,m2.hicn,m3.hicn),isnull(m4.hicn,m5.hicn)) as HIC,
isnull(coalesce(m1.MedicaidID,m3.medicaidid,m3.medicaidid),isnull(m4.MedicaidID,m5.MedicaidID)) as MedsId,
case WHEN LEFT(isnull(coalesce(m1.GroupNumber,m2.groupnumber,m3.groupnumber),isnull(m4.groupnumber,m5.groupnumber)), 3) IN ('305', '810') THEN '33' 
WHEN LEFT(isnull(coalesce(m1.GroupNumber,m2.groupnumber,m3.groupnumber),isnull(m4.groupnumber,m5.groupnumber)), 3) IN ('306', '812') THEN '36' 
ELSE NULL END AS FacilityCounty,
left(isnull(coalesce(m1.policynumber,m2.policynumber,m3.policynumber),isnull(m4.policynumber,m5.policynumber)),9) as CIN,
isnull(coalesce(m1.MBI,m2.mbi,m3.mbi),isnull(m4.mbi,m5.mbi)) as MBI,
isnull(coalesce(m1.DateOfBirth,m2.dateofbirth,m3.dateofbirth),isnull(m4.dateofbirth,m5.dateofbirth)) as DateOfBirth,
isnull(coalesce(m1.GroupNumber,m2.groupnumber,m3.groupnumber),isnull(m4.groupnumber,m5.groupnumber)) as GroupNumber
from (select @MemberNumber as MemberNumber,@ServiceDate as ServiceDate)y 
left join cte m1 on LEFT(m1.MemberNumber, 12) = LEFT(y.MemberNumber, 12) AND y.ServiceDate BETWEEN m1.EffectiveDate AND m1.ExpirationDate 
left join cte m2 on LEFT(m2.PolicyNumber, 9) = LEFT(y.MemberNumber, 9) AND y.ServiceDate BETWEEN m2.EffectiveDate AND m2.ExpirationDate 
left join cte m3 on m3.HICN = y.MemberNumber AND y.ServiceDate BETWEEN m3.EffectiveDate AND m3.ExpirationDate 
left join cte m4 on m4.MBI = y.MemberNumber AND y.ServiceDate BETWEEN m4.EffectiveDate AND m4.ExpirationDate 
left join cte m5 on m5.SocialSecurityNumber = y.MemberNumber AND y.ServiceDate BETWEEN m5.EffectiveDate AND m5.ExpirationDate
";
    }
}
