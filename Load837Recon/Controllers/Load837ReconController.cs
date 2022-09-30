using EncDataModel.RunStatus;
using EncDataModel.Submission837;
using EncDataModel.CMS277CA;
using EncDataModel.CMSMAO002;
using EncDataModel.DHCS;
using Load837Recon.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Load837Recon.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Load837ReconController : ControllerBase
    {
        private readonly ILogger<Load837ReconController> _logger;
        private readonly SubHistoryContext _context;
        private readonly ReconContext _contextRecon;
        private readonly CMS277CAContext _context277CA;
        private readonly CMSMAO002Context _contextMao002;
        private readonly DHCSContext _contextDhcs;
        public Load837ReconController(ILogger<Load837ReconController> logger, SubHistoryContext context, ReconContext contextRecon, CMS277CAContext context277CA, CMSMAO002Context contextMao002, DHCSContext contextDhcs)
        {
            _logger = logger;
            _context = context;
            _contextRecon = contextRecon;
            _context277CA = context277CA;
            _contextMao002 = contextMao002;
            _contextDhcs = contextDhcs;
        }
        //Load837Recon
        [HttpGet]
        public List<Tuple<string,string>> GetSubmittedFilesNeedRecon()
        {
            _logger.Log(LogLevel.Information, "inquiry submitted 837 files need recon");
            List<string> unProcessedFiles = _context.SubmissionLogs.Select(x=>x.FileName).ToList().Except(_contextRecon.LoadLogs.Where(x => !(x.ReloadNeeded ?? false)).Select(x => x.FileName)).ToList();
            List<Tuple<String, string>> result = _context.SubmissionLogs.Where(x => unProcessedFiles.Contains(x.FileName)).Select(x => Tuple.Create(x.FileName, Math.Round((DateTime.Today-x.CreatedDate ).TotalDays,0).ToString())).ToList();
            return result;
        }
        //Load837Recon/1
        [HttpGet("{id}")]
        public List<string> LoadReconForSubmittedFiles(long id)
        {
            List<string> result = new List<string>();
            _logger.Log(LogLevel.Information, "Load recon for submitted 837 files");
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string operationPath = configuration["OperationFolder"];
            string OperationFile = Path.Combine(operationPath, "Load837Recon", "Op" + DateTime.Today.ToString("yyyyMMdd") + ".json");
            RunStatus_LoadFileModel runStatus = JsonConvert.DeserializeObject<RunStatus_LoadFileModel>(System.IO.File.ReadAllText(OperationFile));
            List<int> fileIds = _context.SubmissionLogs.Select(x => x.FileID).ToList().Except(_contextRecon.LoadLogs.Where(x => !(x.ReloadNeeded ?? false)).Select(x => x.FileId)).ToList();
            int totalFiles = fileIds.Count;
            int goodFiles = 0;
            foreach (int fileId in fileIds)
            {
                SubmissionLog historySubmission = _context.SubmissionLogs.Find(fileId);
                _277CAFile response277CA = _context277CA.Files.FirstOrDefault(x => x.GroupControlNumber == historySubmission.InterchangeControlNumber);
                MAO2File responseMao002 = _contextMao002.Files.FirstOrDefault(x => x.ICN == historySubmission.InterchangeControlNumber);
                DHCSFile responseDhcs = _contextDhcs.Files.FirstOrDefault(x => x.ValidationStatus=="Accepted" && x.FileName == historySubmission.FileName);
                if (historySubmission.FileName.Contains("MEDS")) 
                {
                    //CMS
                    if (response277CA != null && responseMao002 != null) 
                    {
                        _logger.Log(LogLevel.Information, "Loading recon for CMS file " + historySubmission.FileName + " now...");
                        List<string> errorClaims = _context277CA.Stcs.Where(x => x.FileId == response277CA.FileId && x.ActionCode == "U").Select(x => x.ClaimId).Distinct().ToList();
                        errorClaims.AddRange(_contextMao002.Details.Where(x => x.FileId == responseMao002.FileId && !string.IsNullOrEmpty(x.ErrorCode)).Select(x => x.ClaimId).Distinct());
                        if (errorClaims.Count > 0) 
                        {
                            _contextRecon.ClaimHeaders.AddRange(_context.ClaimHeaders.Where(x=>x.FileID==historySubmission.FileID&&errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimCAS.AddRange(_context.ClaimCAS.Where(x=>x.FileID==historySubmission.FileID&&errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimCRCs.AddRange(_context.ClaimCRCs.Where(x=>x.FileID==historySubmission.FileID&&errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimHIs.AddRange(_context.ClaimHIs.Where(x=>x.FileID==historySubmission.FileID&&errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimK3s.AddRange(_context.ClaimK3s.Where(x=>x.FileID==historySubmission.FileID&&errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimLineFRMs.AddRange(_context.ClaimLineFRMs.Where(x=>x.FileID==historySubmission.FileID&&errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimLineLQs.AddRange(_context.ClaimLineLQs.Where(x=>x.FileID==historySubmission.FileID&&errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimLineMEAs.AddRange(_context.ClaimLineMEAs.Where(x=>x.FileID==historySubmission.FileID&&errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimLineSVDs.AddRange(_context.ClaimLineSVDs.Where(x=>x.FileID==historySubmission.FileID&&errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimNtes.AddRange(_context.ClaimNtes.Where(x=>x.FileID==historySubmission.FileID&&errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimPatients.AddRange(_context.ClaimPatients.Where(x=>x.FileID==historySubmission.FileID&&errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimProviders.AddRange(_context.ClaimProviders.Where(x=>x.FileID==historySubmission.FileID&&errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimPWKs.AddRange(_context.ClaimPWKs.Where(x=>x.FileID==historySubmission.FileID && errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimSBRs.AddRange(_context.ClaimSBRs.Where(x=>x.FileID==historySubmission.FileID&&errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimSecondaryIdentifications.AddRange(_context.ClaimSecondaryIdentifications.Where(x=>x.FileID==historySubmission.FileID&&errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ProviderContacts.AddRange(_context.ProviderContacts.Where(x=>x.FileID==historySubmission.FileID&&errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ServiceLines.AddRange(_context.ServiceLines.Where(x=>x.FileID==historySubmission.FileID&&errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ToothStatus.AddRange(_context.ToothStatus.Where(x=>x.FileID==historySubmission.FileID&&errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ResponseErrors.AddRange(_context277CA.Stcs.Where(x => x.FileId == response277CA.FileId && x.ActionCode == "U").Select(x => new ResponseError
                            {
                                FileId=historySubmission.FileID,
                                AddedDate=DateTime.Now,
                                ClaimId=x.ClaimId,
                                ErrorCategory="277CA",
                                ErrorId1=x.ClaimStatusCategory1,
                                ErrorId2=x.ClaimStatusCode1,
                                ErrorId3=x.EntityIdentifier1
                            }));
                            _contextRecon.ResponseErrors.AddRange(_contextMao002.Details.Where(x => x.FileId == responseMao002.FileId && !string.IsNullOrEmpty(x.ErrorCode)).Select(x => new ResponseError
                            {
                                FileId=historySubmission.FileID,
                                AddedDate=DateTime.Now,
                                ClaimId=x.ClaimId,
                                LineNumber=x.LineNumber,
                                ErrorCategory="MAO002",
                                ErrorId1=x.ErrorCode,
                                ErrorDescription=x.ErrorDescription
                            }));
                        }
                        LoadLog loadLog = new LoadLog
                        {
                            FileName = historySubmission.FileName,
                            FileId = historySubmission.FileID,
                            ReconLoadedDate = DateTime.Today
                        };
                        _contextRecon.LoadLogs.Add(loadLog);
                        _contextRecon.SaveChanges();
                    }
                }
                if (historySubmission.FileName.Contains("MMCD")) 
                {
                    //CMS Dual
                    if (response277CA != null && responseDhcs != null) 
                    {
                        _logger.Log(LogLevel.Information, "Loading recon for Dual file " + historySubmission.FileName + " now...");
                        List<string> errorClaims = _context277CA.Stcs.Where(x => x.FileId == response277CA.FileId && x.ActionCode == "U").Select(x => x.ClaimId).Distinct().ToList();
                        errorClaims.AddRange(_contextDhcs.Encounters.Where(x => x.FileId == responseDhcs.FileId && x.EncounterStatus == "Denied").Select(x => x.EncounterReferenceNumber));
                        if (errorClaims.Count > 0)
                        {
                            _contextRecon.ClaimHeaders.AddRange(_context.ClaimHeaders.Where(x => x.FileID == historySubmission.FileID && errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimCAS.AddRange(_context.ClaimCAS.Where(x => x.FileID == historySubmission.FileID && errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimCRCs.AddRange(_context.ClaimCRCs.Where(x => x.FileID == historySubmission.FileID && errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimHIs.AddRange(_context.ClaimHIs.Where(x => x.FileID == historySubmission.FileID && errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimK3s.AddRange(_context.ClaimK3s.Where(x => x.FileID == historySubmission.FileID && errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimLineFRMs.AddRange(_context.ClaimLineFRMs.Where(x => x.FileID == historySubmission.FileID && errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimLineLQs.AddRange(_context.ClaimLineLQs.Where(x => x.FileID == historySubmission.FileID && errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimLineMEAs.AddRange(_context.ClaimLineMEAs.Where(x => x.FileID == historySubmission.FileID && errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimLineSVDs.AddRange(_context.ClaimLineSVDs.Where(x => x.FileID == historySubmission.FileID && errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimNtes.AddRange(_context.ClaimNtes.Where(x => x.FileID == historySubmission.FileID && errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimPatients.AddRange(_context.ClaimPatients.Where(x => x.FileID == historySubmission.FileID && errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimProviders.AddRange(_context.ClaimProviders.Where(x => x.FileID == historySubmission.FileID && errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimPWKs.AddRange(_context.ClaimPWKs.Where(x => x.FileID == historySubmission.FileID && errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimSBRs.AddRange(_context.ClaimSBRs.Where(x => x.FileID == historySubmission.FileID && errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimSecondaryIdentifications.AddRange(_context.ClaimSecondaryIdentifications.Where(x => x.FileID == historySubmission.FileID && errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ProviderContacts.AddRange(_context.ProviderContacts.Where(x => x.FileID == historySubmission.FileID && errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ServiceLines.AddRange(_context.ServiceLines.Where(x => x.FileID == historySubmission.FileID && errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ToothStatus.AddRange(_context.ToothStatus.Where(x => x.FileID == historySubmission.FileID && errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ResponseErrors.AddRange(_context277CA.Stcs.Where(x => x.FileId == response277CA.FileId && x.ActionCode == "U").Select(x => new ResponseError
                            {
                                FileId = historySubmission.FileID,
                                AddedDate = DateTime.Now,
                                ClaimId = x.ClaimId,
                                ErrorCategory = "277CA",
                                ErrorId1 = x.ClaimStatusCategory1,
                                ErrorId2 = x.ClaimStatusCode1,
                                ErrorId3 = x.EntityIdentifier1
                            }));
                            _contextRecon.ResponseErrors.AddRange(_contextDhcs.Responses.Where(x=>x.FileId==responseDhcs.FileId&&x.Severity=="Error"&&errorClaims.Contains(x.EncounterReferenceNumber)).Select(x => new ResponseError
                            {
                                FileId = historySubmission.FileID,
                                AddedDate = DateTime.Now,
                                ClaimId = x.EncounterReferenceNumber,
                                ErrorCategory = "DHCS",
                                ErrorId1 = x.IssueId,
                                ErrorDescription = x.IssueDescription
                            }));
                            _contextRecon.ResponseErrors.AddRange(_contextDhcs.ServiceLines.Where(x => x.FileId == responseDhcs.FileId&&x.Severity=="Error" && errorClaims.Contains(x.EncounterReferenceNumber)).Select(x => new ResponseError 
                            {
                                FileId=historySubmission.FileID,
                                AddedDate=DateTime.Now,
                                ClaimId=x.EncounterReferenceNumber,
                                LineNumber=x.Line,
                                ErrorCategory="DHCS",
                                ErrorId1=x.Id,
                                ErrorDescription=x.Description
                            }));
                        }
                        LoadLog loadLog = new LoadLog
                        {
                            FileName = historySubmission.FileName,
                            FileId = historySubmission.FileID,
                            ReconLoadedDate = DateTime.Today
                        };
                        _contextRecon.LoadLogs.Add(loadLog);
                    }
                }
                if (historySubmission.FileName.Contains("MCE")) 
                {
                    //DHCS
                    if (responseDhcs != null) 
                    {
                        _logger.Log(LogLevel.Information, "Loading recon for State file " + historySubmission.FileName + " now...");
                        List<string> errorClaims = _contextDhcs.Encounters.Where(x => x.FileId == responseDhcs.FileId && x.EncounterStatus == "Denied").Select(x => x.EncounterReferenceNumber).ToList();
                        if (errorClaims.Count > 0)
                        {
                            _contextRecon.ClaimHeaders.AddRange(_context.ClaimHeaders.Where(x => x.FileID == historySubmission.FileID && errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimCAS.AddRange(_context.ClaimCAS.Where(x => x.FileID == historySubmission.FileID && errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimCRCs.AddRange(_context.ClaimCRCs.Where(x => x.FileID == historySubmission.FileID && errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimHIs.AddRange(_context.ClaimHIs.Where(x => x.FileID == historySubmission.FileID && errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimK3s.AddRange(_context.ClaimK3s.Where(x => x.FileID == historySubmission.FileID && errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimLineFRMs.AddRange(_context.ClaimLineFRMs.Where(x => x.FileID == historySubmission.FileID && errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimLineLQs.AddRange(_context.ClaimLineLQs.Where(x => x.FileID == historySubmission.FileID && errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimLineMEAs.AddRange(_context.ClaimLineMEAs.Where(x => x.FileID == historySubmission.FileID && errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimLineSVDs.AddRange(_context.ClaimLineSVDs.Where(x => x.FileID == historySubmission.FileID && errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimNtes.AddRange(_context.ClaimNtes.Where(x => x.FileID == historySubmission.FileID && errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimPatients.AddRange(_context.ClaimPatients.Where(x => x.FileID == historySubmission.FileID && errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimProviders.AddRange(_context.ClaimProviders.Where(x => x.FileID == historySubmission.FileID && errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimPWKs.AddRange(_context.ClaimPWKs.Where(x => x.FileID == historySubmission.FileID && errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimSBRs.AddRange(_context.ClaimSBRs.Where(x => x.FileID == historySubmission.FileID && errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ClaimSecondaryIdentifications.AddRange(_context.ClaimSecondaryIdentifications.Where(x => x.FileID == historySubmission.FileID && errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ProviderContacts.AddRange(_context.ProviderContacts.Where(x => x.FileID == historySubmission.FileID && errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ServiceLines.AddRange(_context.ServiceLines.Where(x => x.FileID == historySubmission.FileID && errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ToothStatus.AddRange(_context.ToothStatus.Where(x => x.FileID == historySubmission.FileID && errorClaims.Contains(x.ClaimID)));
                            _contextRecon.ResponseErrors.AddRange(_contextDhcs.Responses.Where(x => x.FileId == responseDhcs.FileId && x.Severity=="Error" && errorClaims.Contains(x.EncounterReferenceNumber)).Select(x => new ResponseError
                            {
                                FileId = historySubmission.FileID,
                                AddedDate = DateTime.Now,
                                ClaimId = x.EncounterReferenceNumber,
                                ErrorCategory = "DHCS",
                                ErrorId1 = x.IssueId,
                                ErrorDescription = x.IssueDescription
                            }));
                            _contextRecon.ResponseErrors.AddRange(_contextDhcs.ServiceLines.Where(x => x.FileId == responseDhcs.FileId && x.Severity=="Error" && errorClaims.Contains(x.EncounterReferenceNumber)).Select(x => new ResponseError
                            {
                                FileId = historySubmission.FileID,
                                AddedDate = DateTime.Now,
                                ClaimId = x.EncounterReferenceNumber,
                                LineNumber = x.Line,
                                ErrorCategory = "DHCS",
                                ErrorId1 = x.Id,
                                ErrorDescription = x.Description
                            }));
                        }
                        LoadLog loadLog = new LoadLog
                        {
                            FileName = historySubmission.FileName,
                            FileId = historySubmission.FileID,
                            ReconLoadedDate = DateTime.Today
                        };
                        _contextRecon.LoadLogs.Add(loadLog);
                    }
                }
                runStatus.FileNames.Add(historySubmission.FileName);
                goodFiles++;
            }
            _contextRecon.SaveChanges();
            result.Add(totalFiles.ToString());
            result.Add(goodFiles.ToString());
            runStatus.CurrentRunStatus = "0";
            System.IO.File.WriteAllText(OperationFile, JsonConvert.SerializeObject(runStatus, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            return result;
        }
    }
}