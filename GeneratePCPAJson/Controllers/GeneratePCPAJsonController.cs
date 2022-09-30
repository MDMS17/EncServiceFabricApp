using EncDataModel.MCPDIP;
using GeneratePCPAJson.Data;
using JsonLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace GeneratePCPAJson.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GeneratePCPAJsonController : ControllerBase
    {
        private readonly ILogger<GeneratePCPAJsonController> _logger;
        private readonly StagingContext _context;
        private readonly ErrorContext _contextError;
        private readonly HistoryContext _contextHistory;
        private readonly LogContext _contextLog;
        public GeneratePCPAJsonController(ILogger<GeneratePCPAJsonController> logger, StagingContext context, ErrorContext contextError, HistoryContext contextHistory, LogContext contextLog)
        {
            _logger = logger;
            _context = context;
            _contextError = contextError;
            _contextHistory = contextHistory;
            _contextLog = contextLog;
        }
        //GeneratePCPAJson, initial query total records
        [HttpGet]
        public List<string> GetPCPACountsForExport()
        {
            List<string> result = new List<string>();
            _logger.Log(LogLevel.Information, "query total counts for PCPA Json export");
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string DestinationFolder = configuration["Output_MCPDIP"];
            int TotalPcpas = _context.PcpAssignments.Count();
            result.Add(TotalPcpas.ToString());
            result.Add(DestinationFolder);
            return result;
        }
        //GeneratePCPAJson/1
        [HttpGet("{id}")]
        public List<string> GeneratePCPAJsonFile(long id)
        {
            List<string> result = new List<string>();
            _logger.Log(LogLevel.Information, "Generate PCPA Json File");
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string DestinationFolder = configuration["Output_MCPDIP"];
            if (id == 1)
            {
                GeneratePCPAJsonFileProd(DestinationFolder);
            }
            else
            {
                GeneratePCPAJsonFileTest(DestinationFolder);
            }
            result.Add(DestinationFolder);
            return result;
        }
        private void GeneratePCPAJsonFileProd(string Output_MCPDIP) 
        {
            PcpHeader PcpaHeader = _context.PcpHeaders.FirstOrDefault();
            PcpHeader headerHistory = _contextHistory.PcpHeaders.FirstOrDefault(x => x.ReportingPeriod.Substring(0, 6) == PcpaHeader.ReportingPeriod.Substring(0, 6));
            if (headerHistory != null)
            {
                _contextHistory.PcpAssignments.RemoveRange(_contextHistory.PcpAssignments.Where(x => x.PcpHeaderId == headerHistory.PcpHeaderId));
                _contextHistory.PcpHeaders.Remove(headerHistory);
                _contextHistory.SaveChanges();
            }
            var dupCins = _context.PcpAssignments.GroupBy(x => x.Cin).Where(x => x.Count() > 1).Select(x => x.Key).ToList();
            List<PcpAssignment> validPcpas = new List<PcpAssignment>();
            List<PcpAssignment> errorPcpas = new List<PcpAssignment>();
            int totalPages = _context.PcpAssignments.Count() / 100000;
            string cinPattern = "^[0-9]{8}[A-Z]$";
            string npiPattern = "^[0-9]{10}$";
            for (int i = 0; i <= totalPages; i++)
            {
                List<PcpAssignment> currentPcpas = _context.PcpAssignments.Skip(i * 100000).Take(100000).ToList();
                if (currentPcpas.Count() == 0) break;
                bool HasError = false;
                foreach (PcpAssignment pcpa in currentPcpas)
                {
                    pcpa.ErrorMessage = "";
                    if (dupCins.Contains(pcpa.Cin))
                    {
                        pcpa.ErrorMessage += "Business Error: Duplicated Cin~";
                        HasError = true;
                    }
                    if (pcpa.PlanCode != "305" && pcpa.PlanCode != "306")
                    {
                        pcpa.ErrorMessage += "Business Error: Invalid PlanCode~";
                        HasError = true;
                    }
                    if (!System.Text.RegularExpressions.Regex.Match(pcpa.Cin, cinPattern).Success)
                    {
                        pcpa.ErrorMessage += "Schema Error: Invalid CIN~";
                        HasError = true;
                    }
                    if (!System.Text.RegularExpressions.Regex.Match(pcpa.Npi, npiPattern).Success)
                    {
                        pcpa.ErrorMessage += "Schema Error: Invalid NPI~";
                        HasError = true;
                    }
                    if (HasError) 
                    {
                        pcpa.ErrorMessage = pcpa.ErrorMessage.Remove(pcpa.ErrorMessage.Length - 1);
                        errorPcpas.Add(pcpa);
                    }
                    else
                    {
                        validPcpas.Add(pcpa);
                    }
                }
            }
            headerHistory = new PcpHeader
            {
                PlanParent = PcpaHeader.PlanParent,
                ReportingPeriod = PcpaHeader.ReportingPeriod,
                SubmissionDate = PcpaHeader.SubmissionDate,
                SubmissionType = PcpaHeader.SubmissionType,
                SubmissionVersion = PcpaHeader.SubmissionVersion,
                SchemaVersion = PcpaHeader.SchemaVersion
            };
            _contextHistory.PcpHeaders.Add(headerHistory);
            _contextHistory.SaveChanges();
            for (int j = 0; j <= validPcpas.Count() / 100000; j++)
            {
                _contextHistory.PcpAssignments.AddRange(validPcpas.Skip(j * 100000).Take(100000).Select(x => new PcpAssignment
                {
                    PcpHeaderId = headerHistory.PcpHeaderId,
                    PlanCode = x.PlanCode,
                    Cin = x.Cin,
                    Npi = x.Npi,
                    TradingPartnerCode = x.TradingPartnerCode,
                    DataSource = x.DataSource,
                    NpiSourceType = x.NpiSourceType
                }));
                _contextHistory.SaveChanges();
            }
            if (errorPcpas.Count() > 0)
            {
                PcpHeader headerError = _contextError.PcpHeaders.FirstOrDefault(x => x.ReportingPeriod == PcpaHeader.ReportingPeriod);
                if (headerError == null)
                {
                    headerError = new PcpHeader
                    {
                        PlanParent = PcpaHeader.PlanParent,
                        ReportingPeriod = PcpaHeader.ReportingPeriod,
                        SubmissionDate = PcpaHeader.SubmissionDate,
                        SubmissionType = PcpaHeader.SubmissionType,
                        SubmissionVersion = PcpaHeader.SubmissionVersion,
                        SchemaVersion = PcpaHeader.SchemaVersion
                    };
                    _contextError.PcpHeaders.Add(headerError);
                    _contextError.SaveChanges();
                }
                _contextError.PcpAssignments.AddRange(errorPcpas.Select(x => new PcpAssignment
                {
                    PcpHeaderId = headerError.PcpHeaderId,
                    PlanCode = x.PlanCode,
                    Cin = x.Cin,
                    Npi = x.Npi,
                    TradingPartnerCode = x.TradingPartnerCode,
                    ErrorMessage = x.ErrorMessage,
                    DataSource = x.DataSource,
                    NpiSourceType = x.NpiSourceType
                }));
                _contextError.SaveChanges();
            }

            JsonPcpa jsonPcpa = new JsonPcpa();
            jsonPcpa.header = new JsonPcpaHeader
            {
                planParent = PcpaHeader.PlanParent,
                reportingPeriod = PcpaHeader.ReportingPeriod,
                submissionDate = PcpaHeader.SubmissionDate,
                submissionType = PcpaHeader.SubmissionType,
                submissionVersion = PcpaHeader.SubmissionVersion,
                schemaVersion = PcpaHeader.SchemaVersion
            };
            jsonPcpa.pcpa = validPcpas.Select(x => new JsonPcpaDetail
            {
                planCode = x.PlanCode,
                cin = x.Cin,
                npi = x.Npi
            }).ToList();
            string fileName = "IEHP_PCPA_" + PcpaHeader.SubmissionDate + "_00001" + ".json";
            System.IO.File.WriteAllText(Path.Combine(Output_MCPDIP, fileName), JsonOperations.GetPcpaJson(jsonPcpa));
            SubmissionLog log2 = _contextLog.SubmissionLogs.FirstOrDefault(x => x.RecordYear == PcpaHeader.ReportingPeriod.Substring(0, 4) && x.RecordMonth == PcpaHeader.ReportingPeriod.Substring(4, 2) && x.FileType == "PCPA");
            if (log2 == null)
            {
                log2 = new SubmissionLog
                {
                    RecordYear = PcpaHeader.ReportingPeriod.Substring(0, 4),
                    RecordMonth = PcpaHeader.ReportingPeriod.Substring(4, 2),
                    FileName = fileName,
                    FileType = "PCPA",
                    SubmitterName = "IEHP",
                    SubmissionDate = PcpaHeader.SubmissionDate,
                    CreateDate = DateTime.Now,
                    CreateBy = User.Identity.Name
                };
                _contextLog.Add(log2);
                _contextLog.SaveChanges();
            }
            log2.FileName = fileName;
            log2.SubmissionDate = PcpaHeader.SubmissionDate;
            log2.TotalPCPASubmitted = validPcpas.Count();
            log2.UpdateDate = DateTime.Now;
            log2.UpdateBy = User.Identity.Name;
            _contextLog.SaveChanges();
            foreach (string IPA in GlobalVariables.TradingPartners)
            {
                ProcessLog log = _contextLog.ProcessLogs.FirstOrDefault(x => x.TradingPartnerCode == IPA && x.RecordYear == PcpaHeader.ReportingPeriod.Substring(0, 4) && x.RecordMonth == PcpaHeader.ReportingPeriod.Substring(4, 2));
                if (log == null)
                {
                    log = new ProcessLog
                    {
                        TradingPartnerCode = IPA,
                        RecordYear = PcpaHeader.ReportingPeriod.Substring(0, 4),
                        RecordMonth = PcpaHeader.ReportingPeriod.Substring(4, 2),
                        RunTime = DateTime.Now,
                        RunBy = User.Identity.Name
                    };
                    _contextLog.Add(log);
                    _contextLog.SaveChanges();
                }
                int countValid = validPcpas.Count(x => x.TradingPartnerCode == IPA);
                int countError = errorPcpas.Count(x => x.TradingPartnerCode == IPA);
                log.PCPAErrors = countError;
                log.PCPASubmits = countValid;
                log.PCPATotal = countValid + countError;
                log.RunStatus = "0";
                _contextLog.SaveChanges();
            }
        }
        private void GeneratePCPAJsonFileTest(string Output_MCPDIP) 
        {
            PcpHeader PcpaHeader = _context.PcpHeaders.FirstOrDefault();
            JsonPcpa jsonPcpa = new JsonPcpa();
            jsonPcpa.header = new JsonPcpaHeader
            {
                planParent = PcpaHeader.PlanParent,
                reportingPeriod = PcpaHeader.ReportingPeriod,
                submissionDate = PcpaHeader.SubmissionDate,
                submissionType = PcpaHeader.SubmissionType,
                submissionVersion = PcpaHeader.SubmissionVersion,
                schemaVersion = PcpaHeader.SchemaVersion
            };
            jsonPcpa.pcpa = _context.PcpAssignments.Select(x => new JsonPcpaDetail
            {
                planCode = x.PlanCode,
                cin = x.Cin,
                npi = x.Npi
            }).ToList();
            int i305 = 0, i306 = 0;
            foreach (var item in jsonPcpa.pcpa)
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
            string fileName = "IEHP_PCPA_" + PcpaHeader.SubmissionDate + "_00001" + ".json";
            System.IO.File.WriteAllText(Path.Combine(Output_MCPDIP, fileName), JsonOperations.GetPcpaJson(jsonPcpa));
        }
    }
}
