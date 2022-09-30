using EncDataModel.MCPDIP;
using GenerateMCPDJson.Data;
using JsonLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GenerateMCPDJson.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GenerateMCPDJsonController : ControllerBase
    {
        private readonly ILogger<GenerateMCPDJsonController> _logger;
        private readonly StagingContext _context;
        private readonly ErrorContext _contextError;
        private readonly HistoryContext _contextHistory;
        private readonly LogContext _contextLog;
        public GenerateMCPDJsonController(ILogger<GenerateMCPDJsonController> logger, StagingContext context, ErrorContext contextError, HistoryContext contextHistory, LogContext contextLog)
        {
            _logger = logger;
            _context = context;
            _contextError = contextError;
            _contextHistory = contextHistory;
            _contextLog = contextLog;
        }
        //GenerateMCPDJson, initial query total records
        [HttpGet]
        public List<string> GetMCPDCountsForExport()
        {
            List<string> result = new List<string>();
            _logger.Log(LogLevel.Information, "query total counts for MCPD Json export");
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string DestinationFolder = configuration["Output_MCPDIP"];
            int TotalGrievances = _context.Grievances.Count();
            int TotalAppeals = _context.Appeals.Count();
            int TotalCocs = _context.McpdContinuityOfCare.Count();
            int TotalOons = _context.McpdOutOfNetwork.Count();
            result.Add(TotalGrievances.ToString() + "~" + TotalAppeals.ToString() + "~" + TotalCocs.ToString() + "~" + TotalOons.ToString());
            result.Add(DestinationFolder);
            return result;
        }
        //GenerateMCPDJson/1
        [HttpGet("{id}")]
        public List<string> GenerateMCPDJsonFile(long id)
        {
            List<string> result = new List<string>();
            _logger.Log(LogLevel.Information, "Generate MCPD Json File");
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string DestinationFolder = configuration["Output_MCPDIP"];
            if (id == 1)
            {
                GenerateMCPDJsonFileProd(DestinationFolder);
            }
            else
            {
                GenerateMCPDJsonFileTest(DestinationFolder);
            }
            result.Add(DestinationFolder);
            return result;
        }
        private void GenerateMCPDJsonFileProd(string Output_MCPDIP)
        {
            McpdHeader mcpdHeader = _context.McpdHeaders.FirstOrDefault();
            McpdHeader headerHistory = _contextHistory.McpdHeaders.FirstOrDefault(x => x.ReportingPeriod.Substring(0, 6) == mcpdHeader.ReportingPeriod.Substring(0, 6));
            if (headerHistory != null)
            {
                _contextHistory.Grievances.RemoveRange(_contextHistory.Grievances.Where(x => x.McpdHeaderId == headerHistory.McpdHeaderId));
                _contextHistory.Appeals.RemoveRange(_contextHistory.Appeals.Where(x => x.McpdHeaderId == headerHistory.McpdHeaderId));
                _contextHistory.McpdContinuityOfCare.RemoveRange(_contextHistory.McpdContinuityOfCare.Where(x => x.McpdHeaderId == headerHistory.McpdHeaderId));
                _contextHistory.McpdOutOfNetwork.RemoveRange(_contextHistory.McpdOutOfNetwork.Where(x => x.McpdHeaderId == headerHistory.McpdHeaderId));
                _contextHistory.SaveChanges();
                _contextHistory.McpdHeaders.Remove(headerHistory);
                _contextHistory.SaveChanges();
            }
            //check schema for grievances, add error to errorcontext, add valid to historycontext
            List<McpdGrievance> allGrievances = _context.Grievances.ToList();
            List<Tuple<string, bool, string>> grievanceSchemas = JsonOperationsNew.ValidateGrievance(allGrievances);
            List<McpdGrievance> validGrievances = new List<McpdGrievance>();
            List<McpdGrievance> errorGrievances = new List<McpdGrievance>();
            List<string> dupGrievanceIds = _context.Grievances.GroupBy(x => x.GrievanceId).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
            string maxReceiveDate = DateTime.Today.ToString("yyyyMM") + "01";
            foreach (McpdGrievance grievance in allGrievances)
            {
                bool hasError = false;
                grievance.ErrorMessage = "";
                if (!grievanceSchemas.First(x => x.Item1 == grievance.GrievanceId).Item2)
                {
                    grievance.ErrorMessage = "Schema Error:" + grievanceSchemas.First(x => x.Item1 == grievance.GrievanceId).Item3;
                    hasError = true;
                }
                else
                {
                    //BL_Grievance001
                    if (dupGrievanceIds.Contains(grievance.GrievanceId))
                    {
                        grievance.ErrorMessage += "Business Error: Duplicated Grievance Id~";
                        hasError = true;
                    }
                    //BL_Grievance002
                    if (grievance.GrievanceId.Substring(0, 3) != grievance.PlanCode)
                    {
                        grievance.ErrorMessage += "Business Error: grievance id should start with plan code~";
                        hasError = true;
                    }
                    //BL_Grievance003
                    if (string.Compare(grievance.GrievanceReceivedDate, maxReceiveDate) >= 0)
                    {
                        grievance.ErrorMessage += "Business Error: Receive date should be prior to current month~";
                        hasError = true;
                    }
                    //BL_Grievance004
                    if (grievance.RecordType == "Original" && !string.IsNullOrEmpty(grievance.ParentGrievanceId))
                    {
                        grievance.ErrorMessage += "Business Error: Parent grievance id not allowed for Original~";
                        hasError = true;
                    }
                    if (grievance.RecordType != "Original")
                    {
                        if (string.IsNullOrEmpty(grievance.ParentGrievanceId))
                        {
                            grievance.ErrorMessage += "Business Error: Parent grievance id is missing for non Original~";
                            hasError = true;
                        }
                        else
                        {
                            var parentGrievance = _contextHistory.Grievances.FirstOrDefault(x => x.GrievanceId == grievance.ParentGrievanceId);
                            if (parentGrievance == null)
                            {
                                grievance.ErrorMessage += "Business Error: Parent grievance id couldnot be found~";
                                hasError = true;
                            }
                            else
                            {
                                var processedGrievance = _contextHistory.Grievances.FirstOrDefault(x => x.ParentGrievanceId == grievance.ParentGrievanceId);
                                if (processedGrievance != null)
                                {
                                    grievance.ErrorMessage += "Business Error: Already processed before, no more actions~";
                                    hasError = true;
                                }
                            }
                        }
                    }
                }
                if (hasError)
                {
                    if (grievance.ErrorMessage.Length > 2000) grievance.ErrorMessage = grievance.ErrorMessage.Substring(0, 2000);
                    errorGrievances.Add(grievance);
                }
                else
                {
                    validGrievances.Add(grievance);
                }
            }
            List<McpdAppeal> allAppeals = _context.Appeals.ToList();
            List<Tuple<string, bool, string>> appealSchemas = JsonOperationsNew.ValidateAppeal(allAppeals);
            List<McpdAppeal> validAppeals = new List<McpdAppeal>();
            List<McpdAppeal> errorAppeals = new List<McpdAppeal>();
            List<string> dupAppealIds = _context.Appeals.GroupBy(x => x.AppealId).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
            foreach (McpdAppeal appeal in allAppeals)
            {
                bool hasError = false;
                appeal.ErrorMessage = "";
                if (!appealSchemas.First(x => x.Item1 == appeal.AppealId).Item2)
                {
                    appeal.ErrorMessage = "Schema Error: " + appealSchemas.First(x => x.Item1 == appeal.AppealId).Item3;
                    hasError = true;
                }
                else
                {
                    //BL_Appeal001
                    if (dupAppealIds.Contains(appeal.AppealId))
                    {
                        appeal.ErrorMessage += "Business Error: Duplicated appeal id~";
                        hasError = true;
                    }
                    //BL_Appeal002
                    if (appeal.AppealId.Substring(0, 3) != appeal.PlanCode)
                    {
                        appeal.ErrorMessage += "Business Error: Appeal id should start with plan code~";
                        hasError = true;
                    }
                    //BL_Appeal003
                    if (string.Compare(appeal.AppealReceivedDate, maxReceiveDate) >= 0)
                    {
                        appeal.ErrorMessage += "Business Error: Receive date should be prior to current month~";
                        hasError = true;
                    }
                    //BL_Appeal004
                    if (string.Compare(appeal.NoticeOfActionDate, maxReceiveDate) >= 0)
                    {
                        appeal.ErrorMessage += "Business Error: Action date should be prior to current month~";
                        hasError = true;
                    }
                    //BL_Appeal005
                    if (appeal.RecordType == "Original" && !string.IsNullOrEmpty(appeal.ParentAppealId))
                    {
                        appeal.ErrorMessage += "Business Error: Parent appeal id not allowed for Original~";
                        hasError = true;
                    }
                    if (appeal.RecordType != "Original")
                    {
                        if (string.IsNullOrEmpty(appeal.ParentAppealId))
                        {
                            appeal.ErrorMessage += "Business Error: Parent appeal id is missing for non Original~";
                            hasError = true;
                        }
                        else
                        {
                            var parentAppeal = _contextHistory.Appeals.FirstOrDefault(x => x.AppealId == appeal.ParentAppealId);
                            if (parentAppeal == null)
                            {
                                appeal.ErrorMessage += "Business Error: Parent appeal id couldnot be found~";
                                hasError = true;
                            }
                            else
                            {
                                var processedAppeal = _contextHistory.Appeals.FirstOrDefault(x => x.ParentAppealId == appeal.ParentAppealId);
                                if (processedAppeal != null)
                                {
                                    appeal.ErrorMessage += "Business Error: Already processed before, no more actions~";
                                    hasError = true;
                                }
                            }
                        }
                    }
                    //BL_Appeal006
                    if (appeal.AppealResolutionStatusIndicator == "Unresolved")
                    {
                        if (!string.IsNullOrEmpty(appeal.AppealResolutionDate))
                        {
                            appeal.ErrorMessage += "Business Error: Resolution date not allowed for unresolved appeal~";
                            hasError = true;
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(appeal.AppealResolutionDate))
                        {
                            appeal.ErrorMessage += "Business Error: Resolution date must be populated for resolved appeal~";
                            hasError = true;
                        }
                        else
                        {
                            if (string.Compare(appeal.AppealResolutionDate, maxReceiveDate) >= 0)
                            {
                                appeal.ErrorMessage += "Business Error: Resolution date should be prior to current month~";
                                hasError = true;
                            }
                            if (string.Compare(appeal.AppealResolutionDate, appeal.AppealReceivedDate) < 0)
                            {
                                appeal.ErrorMessage += "Business Error: Resolution date cannot be prior to receive date~";
                                hasError = true;
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(appeal.ParentGrievanceId))
                    {
                        var appealParentGrievance = _contextHistory.Grievances.FirstOrDefault(x => x.GrievanceId == appeal.ParentGrievanceId);
                        if (appealParentGrievance == null)
                        {
                            appeal.ParentGrievanceId = null;
                        }
                    }
                }
                if (hasError)
                {
                    if (appeal.ErrorMessage.Length > 2000) appeal.ErrorMessage = appeal.ErrorMessage.Substring(0, 2000);
                    errorAppeals.Add(appeal);
                }
                else
                {
                    validAppeals.Add(appeal);
                }
            }

            List<McpdContinuityOfCare> allCocs = _context.McpdContinuityOfCare.ToList();
            List<Tuple<string, bool, string>> cocSchemas = JsonOperationsNew.ValidateCOC(allCocs);
            List<McpdContinuityOfCare> validCocs = new List<McpdContinuityOfCare>();
            List<McpdContinuityOfCare> errorCocs = new List<McpdContinuityOfCare>();
            List<string> dupCocIds = _context.McpdContinuityOfCare.GroupBy(x => x.CocId).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
            List<string> validTaxonomyCodes = _contextLog.TaxonomyCodes.Select(x => x.TAXONOMY_CODE).ToList();
            foreach (McpdContinuityOfCare coc in allCocs)
            {
                bool hasError = false;
                coc.ErrorMessage = "";
                if (!cocSchemas.First(x => x.Item1 == coc.CocId).Item2)
                {
                    coc.ErrorMessage = "Schem Error: " + cocSchemas.First(x => x.Item1 == coc.CocId).Item3;
                    hasError = true;
                }
                else
                {
                    //BL_Coc001
                    if (dupCocIds.Contains(coc.CocId))
                    {
                        coc.ErrorMessage += "Business Error: Duplicated COC id~";
                        hasError = true;
                    }
                    //BL_Coc002
                    if (coc.CocId.Substring(0, 3) != coc.PlanCode)
                    {
                        coc.ErrorMessage += "Business Error: COC id should start with plan code~";
                        hasError = true;
                    }
                    //BL_Coc003
                    if (string.Compare(coc.CocReceivedDate, maxReceiveDate) >= 0)
                    {
                        coc.ErrorMessage += "Business Error: Receive date should be prior to current month~";
                        hasError = true;
                    }
                    //BL_Coc004
                    if (coc.RecordType == "Original" && !string.IsNullOrEmpty(coc.ParentCocId))
                    {
                        coc.ErrorMessage += "Business Error: Parent COC id not allowed for Original~";
                        hasError = true;
                    }
                    if (coc.RecordType != "Original")
                    {
                        if (string.IsNullOrEmpty(coc.ParentCocId))
                        {
                            coc.ErrorMessage += "Business Error: Parent COC id is missing for non Original~";
                            hasError = true;
                        }
                        else
                        {
                            var parentCoc = _contextHistory.McpdContinuityOfCare.FirstOrDefault(x => x.CocId == coc.ParentCocId);
                            if (parentCoc == null)
                            {
                                coc.ErrorMessage += "Business Error: Parent COC id couldnot be found~";
                                hasError = true;
                            }
                            else
                            {
                                var processedCoc = _contextHistory.McpdContinuityOfCare.FirstOrDefault(x => x.ParentCocId == coc.ParentCocId);
                                if (processedCoc != null)
                                {
                                    coc.ErrorMessage += "Business Error: Already processed before, no more actions~";
                                    hasError = true;
                                }
                            }
                        }
                    }
                    //BL_Coc005
                    if (coc.CocType != "MER Denial" && coc.CocDispositionIndicator == "Provider is in MCP Network")
                    {
                        coc.ErrorMessage += "Business Error: COC disposition indicator must <> Provider is in MCP Network, if COC type <> MER Denial~";
                        hasError = true;
                    }
                    //BL_Coc006
                    if (coc.CocDispositionIndicator == "Denied" && !string.IsNullOrEmpty(coc.CocExpirationDate))
                    {
                        coc.ErrorMessage += "Business Error:  Expiration date must be blank if COC disposition indicator = Denied~";
                        hasError = true;
                    }
                    //BL_Coc007
                    if (coc.CocDispositionIndicator == "Approved" && string.IsNullOrEmpty(coc.CocExpirationDate))
                    {
                        coc.ErrorMessage += "Business Error: Expiration date must be populated if COC disposition indicator = Denied~";
                        hasError = true;
                    }
                    //BL_Coc008
                    if (coc.CocDispositionIndicator == "Denied" && string.IsNullOrEmpty(coc.CocDenialReasonIndicator))
                    {
                        coc.ErrorMessage += "Business Error: COC denial reason indicator must be populated if COC disposition indicator = Denied~";
                        hasError = true;
                    }
                    //BL_Coc009
                    if (coc.CocDispositionIndicator != "Denied" && !string.IsNullOrEmpty(coc.CocDenialReasonIndicator))
                    {
                        coc.ErrorMessage += "Business Error: COC denial reason indicator must be blank if COC type <> Denied~";
                        hasError = true;
                    }
                    //BL_Coc010
                    if (coc.CocType == "MER Denial" && string.IsNullOrEmpty(coc.MerExemptionId))
                    {
                        coc.ErrorMessage += "Business Error: MER exemption id must be populated if COC type = MER Denial~";
                        hasError = true;
                    }
                    //BL_Coc011
                    if (coc.CocType != "MER Denial" && !string.IsNullOrEmpty(coc.MerExemptionId))
                    {
                        coc.ErrorMessage += "Business Error: MER excemption id must be blank if COC type <> MER Denial~";
                        hasError = true;
                    }
                    //BL_Coc012
                    if (coc.CocType == "MER Denial" && string.IsNullOrEmpty(coc.ExemptionToEnrollmentDenialCode))
                    {
                        coc.ErrorMessage += "Business Error: Exemption to enrollment denial code must be populated if COC type = MER Denial~";
                        hasError = true;
                    }
                    //BL_Coc013
                    if (coc.CocType != "MER Denial" && !string.IsNullOrEmpty(coc.ExemptionToEnrollmentDenialCode))
                    {
                        coc.ErrorMessage += "Business Error: Exemption to enrollment denial code must be blank if COC type <> MER Denial~";
                        hasError = true;
                    }
                    //BL_Coc014
                    if (coc.CocType == "MER Denial" && string.IsNullOrEmpty(coc.ExemptionToEnrollmentDenialDate))
                    {
                        coc.ErrorMessage += "Business Error: Excemption to enrollment denial date must be populated if COC type = MER Denial~";
                        hasError = true;
                    }
                    //BL_Coc015
                    if (coc.CocType != "MER Denial" && !string.IsNullOrEmpty(coc.ExemptionToEnrollmentDenialDate))
                    {
                        coc.ErrorMessage += "Business Error: Exemption to enrollment denial date must be blank if COC type <> MER Denial~";
                        hasError = true;
                    }
                    //BL_Coc016
                    if (coc.CocType == "MER Denial" && string.Compare(coc.ExemptionToEnrollmentDenialDate, maxReceiveDate) >= 0)
                    {
                        coc.ErrorMessage += "Business Error: Exemption to enrollment denial date must be prior to current month~";
                        hasError = true;
                    }
                    //BL_Coc017
                    if (coc.MerCocDispositionIndicator != "MER COC Not Met" && coc.CocProviderNpi != coc.SubmittingProviderNpi)
                    {
                        coc.ErrorMessage += "Business Error: COC provider NPI must = submitting provider NPI, if MER COC disposition indicator <> MER COC Not Met~";
                        hasError = true;
                    }
                    //BL_Coc018
                    if (coc.CocType == "MER Denial" && string.IsNullOrEmpty(coc.MerCocDispositionIndicator))
                    {
                        coc.ErrorMessage += "Business Error: MER COC disposition indicator must be populated, if COC type = MER Denial~";
                        hasError = true;
                    }
                    //BL_Coc019
                    if (coc.CocType != "MER Denial" && !string.IsNullOrEmpty(coc.MerCocDispositionIndicator))
                    {
                        coc.ErrorMessage += "Business Error: MER COC disposition indicator must be blank if COC type <> MER Denial~";
                        hasError = true;
                    }
                    //BL_Coc020
                    if (coc.CocType == "MER Denial" && string.IsNullOrEmpty(coc.MerCocDispositionDate))
                    {
                        coc.ErrorMessage += "Business Error: MER COC disposition date must be populated if COC type = MER Denial~";
                        hasError = true;
                    }
                    //BL_Coc021
                    if (coc.CocType != "MER Denial" && !string.IsNullOrEmpty(coc.MerCocDispositionDate))
                    {
                        coc.ErrorMessage += "Business Error: MER COC disposition date must be blank if COC type <> MER Denial~";
                        hasError = true;
                    }
                    //BL_Coc022
                    if (coc.CocType == "MER Denial" && string.Compare(coc.MerCocDispositionDate, maxReceiveDate) >= 0)
                    {
                        coc.ErrorMessage += "Business Error: MER COC disposition date must be prior to current month~";
                        hasError = true;
                    }
                    //BL_Coc023
                    if (coc.CocType == "MER Denial" && coc.MerCocDispositionIndicator == "MER COC Not Met" && string.IsNullOrEmpty(coc.ReasonMerCocNotMetIndicator))
                    {
                        coc.ErrorMessage += "Business Error: Reason MER COC not met must be populated if COC type = MER Denial and MER COC disposition indicator = MER COC Not Met~";
                        hasError = true;
                    }
                    //BL_Coc024
                    if (coc.CocType != "MER Denial" && !string.IsNullOrEmpty(coc.ReasonMerCocNotMetIndicator))
                    {
                        coc.ErrorMessage += "Business Error: Reason MER COC not met must be blank if COC type <> MER Denial~";
                        hasError = true;
                    }
                    //BL_Coc025
                    if (coc.MerCocDispositionIndicator != "MER COC Not Met" && !string.IsNullOrEmpty(coc.ReasonMerCocNotMetIndicator))
                    {
                        coc.ErrorMessage += "Business Error: Reason MER COC not met must be blank if MER COC disposition indicator <> MER COC Not Met~";
                        hasError = true;
                    }
                    //BL_Coc026
                    if (!string.IsNullOrEmpty(coc.ProviderTaxonomy) && !validTaxonomyCodes.Contains(coc.ProviderTaxonomy))
                    {
                        coc.ErrorMessage += "Invalid Taxonomy Code~";
                        hasError = true;
                    }
                }
                if (hasError)
                {
                    if (coc.ErrorMessage.Length > 2000) coc.ErrorMessage = coc.ErrorMessage.Substring(0, 2000);
                    errorCocs.Add(coc);
                }
                else
                {
                    validCocs.Add(coc);
                }
            }

            List<McpdOutOfNetwork> allOons = _context.McpdOutOfNetwork.ToList();
            List<Tuple<string, bool, string>> oonSchemas = JsonOperationsNew.ValidateOON(allOons);
            List<McpdOutOfNetwork> validOons = new List<McpdOutOfNetwork>();
            List<McpdOutOfNetwork> errorOons = new List<McpdOutOfNetwork>();
            List<string> dupOonIds = _context.McpdOutOfNetwork.GroupBy(x => x.OonId).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
            foreach (McpdOutOfNetwork oon in allOons)
            {
                bool hasError = false;
                oon.ErrorMessage = "";
                if (!oonSchemas.First(x => x.Item1 == oon.OonId).Item2)
                {
                    oon.ErrorMessage = "Schem Error: " + oonSchemas.First(x => x.Item1 == oon.OonId).Item3;
                    hasError = true;
                }
                else
                {
                    //BL_Oon001
                    if (dupOonIds.Contains(oon.OonId))
                    {
                        oon.ErrorMessage += "Business Error: Duplicated Oon id~";
                        hasError = true;
                    }
                    //BL_Oon002
                    if (oon.OonId.Substring(0, 3) != oon.PlanCode)
                    {
                        oon.ErrorMessage += "Business Error: Oon id should start with plan code~";
                        hasError = true;
                    }
                    //BL_Oon003
                    if (string.Compare(oon.OonRequestReceivedDate, maxReceiveDate) >= 0)
                    {
                        oon.ErrorMessage += "Business Error: Receive date should be prior to current month~";
                        hasError = true;
                    }
                    //BL_Oon004
                    if (oon.RecordType == "Original" && !string.IsNullOrEmpty(oon.ParentOonId))
                    {
                        oon.ErrorMessage += "Business Error: Parent Oon id not allowed for Original~";
                        hasError = true;
                    }
                    if (oon.RecordType != "Original")
                    {
                        if (string.IsNullOrEmpty(oon.ParentOonId))
                        {
                            oon.ErrorMessage += "Business Error: Parent Oon id is missing for non Original~";
                            hasError = true;
                        }
                        else
                        {
                            var parentOon = _contextHistory.McpdOutOfNetwork.FirstOrDefault(x => x.OonId == oon.ParentOonId);
                            if (parentOon == null)
                            {
                                oon.ErrorMessage += "Business Error: Parent Oon id couldnot be found~";
                                hasError = true;
                            }
                            else
                            {
                                var processedOon = _contextHistory.McpdOutOfNetwork.FirstOrDefault(x => x.ParentOonId == oon.ParentOonId);
                                if (processedOon != null)
                                {
                                    oon.ErrorMessage += "Business Error: Already processed before, no more actions~";
                                    hasError = true;
                                }
                            }
                        }
                    }
                    //BL_Oon005
                    if (oon.OonResolutionStatusIndicator == "Partial Approval" && string.IsNullOrEmpty(oon.PartialApprovalExplanation))
                    {
                        oon.ErrorMessage += "Business Error: Partial Approval Explanation must be populated when OON Resolution Status Indicator = Partial Approval~";
                        hasError = true;
                    }
                    //BL_Oon006
                    if (oon.OonResolutionStatusIndicator == "Pending" && !string.IsNullOrEmpty(oon.OonRequestResolvedDate))
                    {
                        oon.ErrorMessage += "Business Error: OON Request Resolved Date must be blank if OON Resolution Status Indicator = Pending~";
                        hasError = true;
                    }
                    //BL_Oon007
                    if (oon.OonResolutionStatusIndicator != "Pending" && string.IsNullOrEmpty(oon.OonRequestResolvedDate))
                    {
                        oon.ErrorMessage += "Business Error: OON Request Resolved Date must be populated if OON Resolution Status Indicator <> Pending~";
                        hasError = true;
                    }
                    //BL_Oon008
                    if (oon.OonResolutionStatusIndicator != "Pending" && string.Compare(oon.OonRequestResolvedDate, maxReceiveDate) >= 0)
                    {
                        oon.ErrorMessage += "Business Error: OON Request Resolved Date is not a past date~";
                        hasError = true;
                    }
                    //BL_Oon009
                    if (oon.OonResolutionStatusIndicator != "Pending" && string.Compare(oon.OonRequestResolvedDate, oon.OonRequestReceivedDate) < 0)
                    {
                        oon.ErrorMessage += "Business Error: OON Request Resolved Date must be >= OON Request Received Date~";
                        hasError = true;
                    }
                    //BL_OON010
                    if (oon.ServiceLocationCountry != "US" && !string.IsNullOrEmpty(oon.ServiceLocationState))
                    {
                        oon.ErrorMessage += "Business ERror: Foreign country, state should be blank~";
                        hasError = true;
                    }
                    //BL_OON011
                    if (oon.ServiceLocationCountry != "US" && !string.IsNullOrEmpty(oon.ServiceLocationZip))
                    {
                        oon.ErrorMessage += "Business Error: Foreigh country, zip should be blank~";
                        hasError = true;
                    }
                    //BL_OON012
                    if (!string.IsNullOrEmpty(oon.ProviderTaxonomy) && !validTaxonomyCodes.Contains(oon.ProviderTaxonomy))
                    {
                        oon.ErrorMessage += "Invalid Taxonomy Code~";
                        hasError = true;
                    }
                }
                if (hasError)
                {
                    if (oon.ErrorMessage.Length > 2000) oon.ErrorMessage = oon.ErrorMessage.Substring(0, 2000);
                    errorOons.Add(oon);
                }
                else
                {
                    validOons.Add(oon);
                }
            }

            McpdHeader mcpdHeaderHistory = new McpdHeader
            {
                PlanParent = mcpdHeader.PlanParent,
                SubmissionDate = mcpdHeader.SubmissionDate,
                SchemaVersion = mcpdHeader.SchemaVersion,
                ReportingPeriod = mcpdHeader.ReportingPeriod
            };
            _contextHistory.McpdHeaders.Add(mcpdHeaderHistory);
            _contextHistory.SaveChanges();
            _contextHistory.Grievances.AddRange(validGrievances.Select(x => new McpdGrievance
            {
                McpdHeaderId = mcpdHeaderHistory.McpdHeaderId,
                PlanCode = x.PlanCode,
                Cin = x.Cin,
                GrievanceId = x.GrievanceId,
                RecordType = x.RecordType,
                ParentGrievanceId = x.ParentGrievanceId,
                GrievanceReceivedDate = x.GrievanceReceivedDate,
                GrievanceType = x.GrievanceType,
                BenefitType = x.BenefitType,
                ExemptIndicator = x.ExemptIndicator,
                TradingPartnerCode = x.TradingPartnerCode,
                DataSource = x.DataSource,
                CaseNumber = x.CaseNumber,
                CaseStatus = x.CaseStatus
            }));
            _contextHistory.Appeals.AddRange(validAppeals.Select(x => new McpdAppeal
            {
                McpdHeaderId = mcpdHeaderHistory.McpdHeaderId,
                PlanCode = x.PlanCode,
                Cin = x.Cin,
                AppealId = x.AppealId,
                RecordType = x.RecordType,
                ParentGrievanceId = x.ParentGrievanceId,
                ParentAppealId = x.ParentAppealId,
                AppealReceivedDate = x.AppealReceivedDate,
                NoticeOfActionDate = x.NoticeOfActionDate,
                AppealType = x.AppealType,
                BenefitType = x.BenefitType,
                AppealResolutionStatusIndicator = x.AppealResolutionStatusIndicator,
                AppealResolutionDate = x.AppealResolutionDate,
                PartiallyOverturnIndicator = x.PartiallyOverturnIndicator,
                ExpeditedIndicator = x.ExpeditedIndicator,
                TradingPartnerCode = x.TradingPartnerCode,
                DataSource = x.DataSource,
                CaseNumber = x.CaseNumber,
                CaseStatus = x.CaseStatus
            }));
            _contextHistory.McpdContinuityOfCare.AddRange(validCocs.Select(x => new McpdContinuityOfCare
            {
                McpdHeaderId = mcpdHeaderHistory.McpdHeaderId,
                PlanCode = x.PlanCode,
                Cin = x.Cin,
                CocId = x.CocId,
                RecordType = x.RecordType,
                ParentCocId = x.ParentCocId,
                CocReceivedDate = x.CocReceivedDate,
                CocType = x.CocType,
                BenefitType = x.BenefitType,
                CocDispositionIndicator = x.CocDispositionIndicator,
                CocExpirationDate = x.CocExpirationDate,
                CocDenialReasonIndicator = x.CocDenialReasonIndicator,
                SubmittingProviderNpi = x.SubmittingProviderNpi,
                CocProviderNpi = x.CocProviderNpi,
                ProviderTaxonomy = x.ProviderTaxonomy,
                MerExemptionId = x.MerExemptionId,
                ExemptionToEnrollmentDenialCode = x.ExemptionToEnrollmentDenialCode,
                ExemptionToEnrollmentDenialDate = x.ExemptionToEnrollmentDenialDate,
                MerCocDispositionIndicator = x.MerCocDispositionIndicator,
                MerCocDispositionDate = x.MerCocDispositionDate,
                ReasonMerCocNotMetIndicator = x.ReasonMerCocNotMetIndicator,
                TradingPartnerCode = x.TradingPartnerCode,
                DataSource = x.DataSource,
                CaseNumber = x.CaseNumber,
                CaseStatus = x.CaseStatus
            }));
            _contextHistory.McpdOutOfNetwork.AddRange(validOons.Select(x => new McpdOutOfNetwork
            {
                McpdHeaderId = mcpdHeaderHistory.McpdHeaderId,
                PlanCode = x.PlanCode,
                Cin = x.Cin,
                OonId = x.OonId,
                RecordType = x.RecordType,
                ParentOonId = x.ParentOonId,
                OonRequestReceivedDate = x.OonRequestReceivedDate,
                ReferralRequestReasonIndicator = x.ReferralRequestReasonIndicator,
                OonResolutionStatusIndicator = x.OonResolutionStatusIndicator,
                OonRequestResolvedDate = x.OonRequestResolvedDate,
                PartialApprovalExplanation = x.PartialApprovalExplanation,
                SpecialistProviderNpi = x.SpecialistProviderNpi,
                ProviderTaxonomy = x.ProviderTaxonomy,
                ServiceLocationAddressLine1 = x.ServiceLocationAddressLine1,
                ServiceLocationAddressLine2 = x.ServiceLocationAddressLine2,
                ServiceLocationCity = x.ServiceLocationCity,
                ServiceLocationState = x.ServiceLocationState,
                ServiceLocationZip = x.ServiceLocationZip,
                ServiceLocationCountry = x.ServiceLocationCountry,
                TradingPartnerCode = x.TradingPartnerCode,
                DataSource = x.DataSource,
                CaseNumber = x.CaseNumber,
                CaseStatus = x.CaseStatus
            }));
            _contextHistory.SaveChanges();
            if (errorGrievances.Count() > 0 || errorAppeals.Count() > 0 || errorCocs.Count() > 0 || errorOons.Count() > 0)
            {
                McpdHeader mcpdHeaderError = _contextError.McpdHeaders.FirstOrDefault(x => x.ReportingPeriod == mcpdHeader.ReportingPeriod);
                if (mcpdHeaderError == null)
                {
                    mcpdHeaderError = new McpdHeader
                    {
                        PlanParent = mcpdHeader.PlanParent,
                        SubmissionDate = mcpdHeader.SubmissionDate,
                        SchemaVersion = mcpdHeader.SchemaVersion,
                        ReportingPeriod = mcpdHeader.ReportingPeriod
                    };
                    _contextError.McpdHeaders.Add(mcpdHeaderError);
                    _contextError.SaveChanges();
                }
                if (errorGrievances.Count() > 0)
                {
                    _contextError.Grievances.AddRange(errorGrievances.Select(x => new McpdGrievance
                    {
                        McpdHeaderId = mcpdHeaderError.McpdHeaderId,
                        PlanCode = x.PlanCode,
                        Cin = x.Cin,
                        GrievanceId = x.GrievanceId,
                        RecordType = x.RecordType,
                        ParentGrievanceId = x.ParentGrievanceId,
                        GrievanceReceivedDate = x.GrievanceReceivedDate,
                        GrievanceType = x.GrievanceType,
                        BenefitType = x.BenefitType,
                        ExemptIndicator = x.ExemptIndicator,
                        TradingPartnerCode = x.TradingPartnerCode,
                        ErrorMessage = x.ErrorMessage,
                        DataSource = x.DataSource,
                        CaseNumber = x.CaseNumber,
                        CaseStatus = x.CaseStatus
                    }));
                }
                if (errorAppeals.Count() > 0)
                {
                    _contextError.Appeals.AddRange(errorAppeals.Select(x => new McpdAppeal
                    {
                        McpdHeaderId = mcpdHeaderError.McpdHeaderId,
                        PlanCode = x.PlanCode,
                        Cin = x.Cin,
                        AppealId = x.AppealId,
                        RecordType = x.RecordType,
                        ParentGrievanceId = x.ParentGrievanceId,
                        ParentAppealId = x.ParentAppealId,
                        AppealReceivedDate = x.AppealReceivedDate,
                        NoticeOfActionDate = x.NoticeOfActionDate,
                        AppealType = x.AppealType,
                        BenefitType = x.BenefitType,
                        AppealResolutionStatusIndicator = x.AppealResolutionStatusIndicator,
                        AppealResolutionDate = x.AppealResolutionDate,
                        PartiallyOverturnIndicator = x.PartiallyOverturnIndicator,
                        ExpeditedIndicator = x.ExpeditedIndicator,
                        TradingPartnerCode = x.TradingPartnerCode,
                        ErrorMessage = x.ErrorMessage,
                        DataSource = x.DataSource,
                        CaseNumber = x.CaseNumber,
                        CaseStatus = x.CaseStatus
                    }));
                }
                if (errorCocs.Count() > 0)
                {
                    _contextError.McpdContinuityOfCare.AddRange(errorCocs.Select(x => new McpdContinuityOfCare
                    {
                        McpdHeaderId = mcpdHeaderError.McpdHeaderId,
                        PlanCode = x.PlanCode,
                        Cin = x.Cin,
                        CocId = x.CocId,
                        RecordType = x.RecordType,
                        ParentCocId = x.ParentCocId,
                        CocReceivedDate = x.CocReceivedDate,
                        CocType = x.CocType,
                        BenefitType = x.BenefitType,
                        CocDispositionIndicator = x.CocDispositionIndicator,
                        CocExpirationDate = x.CocExpirationDate,
                        CocDenialReasonIndicator = x.CocDenialReasonIndicator,
                        SubmittingProviderNpi = x.SubmittingProviderNpi,
                        CocProviderNpi = x.CocProviderNpi,
                        ProviderTaxonomy = x.ProviderTaxonomy,
                        MerExemptionId = x.MerExemptionId,
                        ExemptionToEnrollmentDenialCode = x.ExemptionToEnrollmentDenialCode,
                        ExemptionToEnrollmentDenialDate = x.ExemptionToEnrollmentDenialDate,
                        MerCocDispositionIndicator = x.MerCocDispositionIndicator,
                        MerCocDispositionDate = x.MerCocDispositionDate,
                        ReasonMerCocNotMetIndicator = x.ReasonMerCocNotMetIndicator,
                        TradingPartnerCode = x.TradingPartnerCode,
                        ErrorMessage = x.ErrorMessage,
                        DataSource = x.DataSource,
                        CaseNumber = x.CaseNumber,
                        CaseStatus = x.CaseStatus
                    }));
                }
                if (errorOons.Count() > 0)
                {
                    _contextError.McpdOutOfNetwork.AddRange(errorOons.Select(x => new McpdOutOfNetwork
                    {
                        McpdHeaderId = mcpdHeaderError.McpdHeaderId,
                        PlanCode = x.PlanCode,
                        Cin = x.Cin,
                        OonId = x.OonId,
                        RecordType = x.RecordType,
                        ParentOonId = x.ParentOonId,
                        OonRequestReceivedDate = x.OonRequestReceivedDate,
                        ReferralRequestReasonIndicator = x.ReferralRequestReasonIndicator,
                        OonResolutionStatusIndicator = x.OonResolutionStatusIndicator,
                        OonRequestResolvedDate = x.OonRequestResolvedDate,
                        PartialApprovalExplanation = x.PartialApprovalExplanation,
                        SpecialistProviderNpi = x.SpecialistProviderNpi,
                        ProviderTaxonomy = x.ProviderTaxonomy,
                        ServiceLocationAddressLine1 = x.ServiceLocationAddressLine1,
                        ServiceLocationAddressLine2 = x.ServiceLocationAddressLine2,
                        ServiceLocationCity = x.ServiceLocationCity,
                        ServiceLocationState = x.ServiceLocationState,
                        ServiceLocationZip = x.ServiceLocationZip,
                        ServiceLocationCountry = x.ServiceLocationCountry,
                        TradingPartnerCode = x.TradingPartnerCode,
                        ErrorMessage = x.ErrorMessage,
                        DataSource = x.DataSource,
                        CaseNumber = x.CaseNumber,
                        CaseStatus = x.CaseStatus
                    }));
                }
                _contextError.SaveChanges();
            }
            JsonMcpd jsonMcpd = new JsonMcpd();
            jsonMcpd.header = new JsonMcpdHeader
            {
                planParent = mcpdHeader.PlanParent,
                submissionDate = mcpdHeader.SubmissionDate.ToString("yyyyMMdd"),
                schemaVersion = mcpdHeader.SchemaVersion
            };
            char[] GrievanceTypeSpillters = new char[] { '|', ';' };
            jsonMcpd.grievances = validGrievances.Select(x => new JsonGrievance
            {
                planCode = x.PlanCode,
                cin = x.Cin,
                grievanceId = x.GrievanceId,
                recordType = x.RecordType,
                parentGrievanceId = string.IsNullOrEmpty(x.ParentGrievanceId) ? null : x.ParentGrievanceId,
                grievanceReceivedDate = x.GrievanceReceivedDate,
                grievanceType = x.GrievanceType.Split(GrievanceTypeSpillters).ToList(),
                benefitType = x.BenefitType,
                exemptIndicator = x.ExemptIndicator
            }).ToList();
            jsonMcpd.appeals = validAppeals.Select(x => new JsonAppeal
            {
                planCode = x.PlanCode,
                cin = x.Cin,
                appealId = x.AppealId,
                recordType = x.RecordType,
                parentGrievanceId = string.IsNullOrEmpty(x.ParentGrievanceId) ? null : x.ParentGrievanceId,
                parentAppealId = string.IsNullOrEmpty(x.ParentAppealId) ? null : x.ParentAppealId,
                appealReceivedDate = x.AppealReceivedDate,
                noticeOfActionDate = x.NoticeOfActionDate,
                appealType = x.AppealType,
                benefitType = x.BenefitType,
                appealResolutionStatusIndicator = x.AppealResolutionStatusIndicator,
                appealResolutionDate = x.AppealResolutionDate,
                partiallyOverturnIndicator = x.PartiallyOverturnIndicator,
                expeditedIndicator = x.ExpeditedIndicator
            }).ToList();
            jsonMcpd.continuityOfCare = validCocs.Select(x => new JsonCOC
            {
                planCode = x.PlanCode,
                cin = x.Cin,
                cocId = x.CocId,
                recordType = x.RecordType,
                parentCocId = x.ParentCocId,
                cocReceivedDate = x.CocReceivedDate,
                cocType = x.CocType,
                benefitType = x.BenefitType,
                cocDispositionIndicator = x.CocDispositionIndicator,
                cocExpirationDate = x.CocExpirationDate,
                cocDenialReasonIndicator = x.CocDenialReasonIndicator,
                submittingProviderNpi = x.SubmittingProviderNpi,
                cocProviderNpi = x.CocProviderNpi,
                providerTaxonomy = x.ProviderTaxonomy,
                merExemptionId = x.MerExemptionId,
                exemptionToEnrollmentDenialCode = x.ExemptionToEnrollmentDenialCode,
                exemptionToEnrollmentDenialDate = x.ExemptionToEnrollmentDenialDate,
                merCocDispositionIndicator = x.MerCocDispositionIndicator,
                merCocDispositionDate = x.MerCocDispositionDate,
                reasonMerCocNotMetIndicator = x.ReasonMerCocNotMetIndicator
            }).ToList();
            jsonMcpd.outOfNetwork = validOons.Select(x => new JsonOON
            {
                planCode = x.PlanCode,
                cin = x.Cin,
                oonId = x.OonId,
                recordType = x.RecordType,
                parentOonId = x.ParentOonId,
                oonRequestReceivedDate = x.OonRequestReceivedDate,
                referralRequestReasonIndicator = x.ReferralRequestReasonIndicator,
                oonResolutionStatusIndicator = x.OonResolutionStatusIndicator,
                oonRequestResolvedDate = x.OonRequestResolvedDate,
                partialApprovalExplanation = x.PartialApprovalExplanation,
                specialistProviderNpi = x.SpecialistProviderNpi,
                providerTaxonomy = x.ProviderTaxonomy,
                serviceLocationAddressLine1 = x.ServiceLocationAddressLine1,
                serviceLocationAddressLine2 = x.ServiceLocationAddressLine2,
                serviceLocationCity = x.ServiceLocationCity,
                serviceLocationState = x.ServiceLocationState,
                serviceLocationZip = x.ServiceLocationZip,
                serviceLocationCountry = x.ServiceLocationCountry
            }).ToList();
            string fileName = "IEHP_MCPD_" + mcpdHeader.SubmissionDate.ToString("yyyyMMdd") + "_00001" + ".json";
            System.IO.File.WriteAllText(Path.Combine(Output_MCPDIP, fileName), JsonOperations.GetMcpdJson(jsonMcpd));
            SubmissionLog log2 = _contextLog.SubmissionLogs.FirstOrDefault(x => x.RecordYear == mcpdHeader.ReportingPeriod.Substring(0, 4) && x.RecordMonth == mcpdHeader.ReportingPeriod.Substring(4, 2) && x.FileType == "MCPD");
            if (log2 == null)
            {
                log2 = new SubmissionLog
                {
                    RecordYear = mcpdHeader.ReportingPeriod.Substring(0, 4),
                    RecordMonth = mcpdHeader.ReportingPeriod.Substring(4, 2),
                    FileName = fileName,
                    FileType = "MCPD",
                    SubmitterName = "IEHP",
                    SubmissionDate = mcpdHeader.SubmissionDate.ToString("yyyyMMdd"),
                    CreateDate = DateTime.Now,
                    CreateBy = User.Identity.Name
                };
                _contextLog.Add(log2);
                _contextLog.SaveChanges();
            }
            log2.FileName = fileName;
            log2.SubmissionDate = mcpdHeader.SubmissionDate.ToString("yyyyMMdd");
            log2.TotalGrievanceSubmitted = validGrievances.Count();
            log2.TotalAppealSubmitted = validAppeals.Count();
            log2.TotalCOCSubmitted = validCocs.Count();
            log2.TotalOONSubmitted = validOons.Count();
            log2.UpdateDate = DateTime.Now;
            log2.UpdateBy = User.Identity.Name;
            _contextLog.SaveChanges();
            foreach (string IPA in GlobalVariables.TradingPartners)
            {
                ProcessLog log = _contextLog.ProcessLogs.FirstOrDefault(x => x.TradingPartnerCode == IPA && x.RecordYear == mcpdHeader.ReportingPeriod.Substring(0, 4) && x.RecordMonth == mcpdHeader.ReportingPeriod.Substring(4, 2));
                if (log == null)
                {
                    log = new ProcessLog
                    {
                        TradingPartnerCode = IPA,
                        RecordYear = mcpdHeader.ReportingPeriod.Substring(0, 4),
                        RecordMonth = mcpdHeader.ReportingPeriod.Substring(4, 2),
                        RunTime = DateTime.Now,
                        RunBy = User.Identity.Name
                    };
                    _contextLog.Add(log);
                    _context.SaveChanges();
                }
                int countValidGrievance = validGrievances.Count(x => x.TradingPartnerCode == IPA);
                int countErrorGrievance = errorGrievances.Count(x => x.TradingPartnerCode == IPA);
                int countValidAppeal = validAppeals.Count(x => x.TradingPartnerCode == IPA);
                int countErrorAppeal = errorAppeals.Count(x => x.TradingPartnerCode == IPA);
                int countValidCOC = validCocs.Count(x => x.TradingPartnerCode == IPA);
                int countErrorCOC = errorCocs.Count(x => x.TradingPartnerCode == IPA);
                int countValidOON = validOons.Count(x => x.TradingPartnerCode == IPA);
                int countErrorOON = errorOons.Count(x => x.TradingPartnerCode == IPA);
                log.GrievanceErrors = countErrorGrievance;
                log.GrievanceSubmits = countValidGrievance;
                log.GrievanceTotal = countValidGrievance + countErrorGrievance;
                log.AppealErrors = countErrorAppeal;
                log.AppealSubmits = countValidAppeal;
                log.AppealTotal = countValidAppeal + countErrorAppeal;
                log.COCSubmits = countValidCOC;
                log.COCErrors = countErrorCOC;
                log.COCTotal = countValidCOC + countErrorCOC;
                log.OONSubmits = countValidOON;
                log.OONErrors = countErrorOON;
                log.OONTotal = countValidOON + countErrorOON;
                log.RunStatus = "0";
                _contextLog.SaveChanges();
            }
        }
        private void GenerateMCPDJsonFileTest(string Output_MCPDIP)
        {
            McpdHeader mcpdHeader = _context.McpdHeaders.FirstOrDefault();
            JsonMcpd jsonMcpd = new JsonMcpd();
            jsonMcpd.header = new JsonMcpdHeader
            {
                planParent = mcpdHeader.PlanParent,
                submissionDate = mcpdHeader.SubmissionDate.ToString("yyyyMMdd"),
                schemaVersion = mcpdHeader.SchemaVersion
            };
            char[] GrievanceTypeSpillters = new char[] { '|', ';' };
            jsonMcpd.grievances = _context.Grievances.Select(x => new JsonGrievance
            {
                planCode = x.PlanCode,
                cin = x.Cin,
                grievanceId = x.GrievanceId,
                recordType = x.RecordType,
                parentGrievanceId = string.IsNullOrEmpty(x.ParentGrievanceId) ? null : x.ParentGrievanceId,
                grievanceReceivedDate = x.GrievanceReceivedDate,
                grievanceType = x.GrievanceType.Split(GrievanceTypeSpillters).ToList(),
                benefitType = x.BenefitType,
                exemptIndicator = x.ExemptIndicator
            }).ToList();
            jsonMcpd.appeals = _context.Appeals.Select(x => new JsonAppeal
            {
                planCode = x.PlanCode,
                cin = x.Cin,
                appealId = x.AppealId,
                recordType = x.RecordType,
                parentGrievanceId = string.IsNullOrEmpty(x.ParentGrievanceId) ? null : x.ParentGrievanceId,
                parentAppealId = string.IsNullOrEmpty(x.ParentAppealId) ? null : x.ParentAppealId,
                appealReceivedDate = x.AppealReceivedDate,
                noticeOfActionDate = x.NoticeOfActionDate,
                appealType = x.AppealType,
                benefitType = x.BenefitType,
                appealResolutionStatusIndicator = x.AppealResolutionStatusIndicator,
                appealResolutionDate = x.AppealResolutionDate,
                partiallyOverturnIndicator = x.PartiallyOverturnIndicator,
                expeditedIndicator = x.ExpeditedIndicator
            }).ToList();
            jsonMcpd.continuityOfCare = _context.McpdContinuityOfCare.Select(x => new JsonCOC
            {
                planCode = x.PlanCode,
                cin = x.Cin,
                cocId = x.CocId,
                recordType = x.RecordType,
                parentCocId = x.ParentCocId,
                cocReceivedDate = x.CocReceivedDate,
                cocType = x.CocType,
                benefitType = x.BenefitType,
                cocDispositionIndicator = x.CocDispositionIndicator,
                cocExpirationDate = x.CocExpirationDate,
                cocDenialReasonIndicator = x.CocDenialReasonIndicator,
                submittingProviderNpi = x.SubmittingProviderNpi,
                cocProviderNpi = x.CocProviderNpi,
                providerTaxonomy = x.ProviderTaxonomy,
                merExemptionId = x.MerExemptionId,
                exemptionToEnrollmentDenialCode = x.ExemptionToEnrollmentDenialCode,
                exemptionToEnrollmentDenialDate = x.ExemptionToEnrollmentDenialDate,
                merCocDispositionIndicator = x.MerCocDispositionIndicator,
                merCocDispositionDate = x.MerCocDispositionDate,
                reasonMerCocNotMetIndicator = x.ReasonMerCocNotMetIndicator
            }).ToList();
            jsonMcpd.outOfNetwork = _context.McpdOutOfNetwork.Select(x => new JsonOON
            {
                planCode = x.PlanCode,
                cin = x.Cin,
                oonId = x.OonId,
                recordType = x.RecordType,
                parentOonId = x.ParentOonId,
                oonRequestReceivedDate = x.OonRequestReceivedDate,
                referralRequestReasonIndicator = x.ReferralRequestReasonIndicator,
                oonResolutionStatusIndicator = x.OonResolutionStatusIndicator,
                oonRequestResolvedDate = x.OonRequestResolvedDate,
                partialApprovalExplanation = x.PartialApprovalExplanation,
                specialistProviderNpi = x.SpecialistProviderNpi,
                providerTaxonomy = x.ProviderTaxonomy,
                serviceLocationAddressLine1 = x.ServiceLocationAddressLine1,
                serviceLocationAddressLine2 = x.ServiceLocationAddressLine2,
                serviceLocationCity = x.ServiceLocationCity,
                serviceLocationState = x.ServiceLocationState,
                serviceLocationZip = x.ServiceLocationZip,
                serviceLocationCountry = x.ServiceLocationCountry
            }).ToList();
            int i305 = 0, i306 = 0;
            foreach (var item in jsonMcpd.grievances)
            {
                if (item.planCode == "305")
                {
                    item.cin = GlobalVariables.TestCin305[i305];
                    i305++;
                    if (i305 >= 10) i305 = 0;
                }
                else if (item.planCode == "306")
                {
                    item.cin = GlobalVariables.TestCin306[i306];
                    i306++;
                    if (i306 >= 10) i306 = 0;
                }
            }
            i305 = 0;
            i306 = 0;
            foreach (var item in jsonMcpd.appeals)
            {
                if (item.planCode == "305")
                {
                    item.cin = GlobalVariables.TestCin305[i305];
                    i305++;
                    if (i305 >= 10) i305 = 0;
                }
                else if (item.planCode == "306")
                {
                    item.cin = GlobalVariables.TestCin306[i306];
                    i306++;
                    if (i306 >= 10) i306 = 0;
                }
            }
            i305 = 0;
            i306 = 0;
            foreach (var item in jsonMcpd.continuityOfCare)
            {
                if (item.planCode == "305")
                {
                    item.cin = GlobalVariables.TestCin305[i305];
                    if (item.cocType == "MER Denial") item.merExemptionId = GlobalVariables.TestCocMer305[i305];
                    i305++;
                    if (i305 >= 10) i305 = 0;
                }
                else if (item.planCode == "306")
                {
                    item.cin = GlobalVariables.TestCin306[i306];
                    if (item.cocType == "MER Denial") item.merExemptionId = GlobalVariables.TestCocMer306[i306];
                    i306++;
                    if (i306 >= 10) i306 = 0;
                }
            }
            i305 = 0;
            i306 = 0;
            foreach (var item in jsonMcpd.outOfNetwork)
            {
                if (item.planCode == "305")
                {
                    item.cin = GlobalVariables.TestCin305[i305];
                    i305++;
                    if (i305 >= 10) i305 = 0;
                }
                else if (item.planCode == "306")
                {
                    item.cin = GlobalVariables.TestCin306[i306];
                    i306++;
                    if (i306 >= 10) i306 = 0;
                }
            }
            string fileName = "IEHP_MCPD_" + mcpdHeader.SubmissionDate.ToString("yyyyMMdd") + "_00001" + ".json";
            System.IO.File.WriteAllText(Path.Combine(Output_MCPDIP, fileName), JsonOperations.GetMcpdJson(jsonMcpd));
        }
    }
}
