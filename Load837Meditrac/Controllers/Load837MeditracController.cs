using EncDataModel.RunStatus;
using Load837Meditrac.Data;
using Load837Meditrac.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Load837Meditrac.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Load837MeditracController : ControllerBase
    {
        private readonly ILogger<Load837MeditracController> _logger;
        private readonly Sub837Context _context;
        private readonly ErrorContext _contextError;
        private readonly MeditracContext _contextMeditrac;
        public Load837MeditracController(ILogger<Load837MeditracController> logger, Sub837Context context, ErrorContext contextError, MeditracContext contextMeditrac)
        {
            _logger = logger;
            _context = context;
            _contextError = contextError;
            _contextMeditrac = contextMeditrac;
        }
        [HttpGet]
        public List<string> Load837Meditrac() 
        {
            List<string> result = new List<string>();
            _logger.Log(LogLevel.Information, "query total counts for data loading from Meditrac");
            //CMS professional
            result.Add(_context.ClaimHeaders.Count(x => x.ExportType == "CMSP").ToString());
            //CMS Institutional
            result.Add(_context.ClaimHeaders.Count(x => x.ExportType == "CMSI").ToString());
            //Dual Professional
            result.Add(_context.ClaimHeaders.Count(x => x.ExportType == "CCIP").ToString());
            //Dual Institutioal
            result.Add(_context.ClaimHeaders.Count(x => x.ExportType == "CCII").ToString());
            //State Professional
            result.Add(_context.ClaimHeaders.Count(x => x.ExportType == "DHCSP").ToString());
            //State Institutioanl
            result.Add(_context.ClaimHeaders.Count(x => x.ExportType == "DHCSI").ToString());
            return result;
        }
        [HttpPost]
        public List<string> Load837Meditrac([FromBody] List<string> Items)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string operationPath = configuration["OperationFolder"];
            string OperationFile = Path.Combine(operationPath, "Load837Meditrac", "Op" + DateTime.Today.ToString("yyyyMMdd") + ".json");
            RunStatus_Load837Database runStatus = JsonConvert.DeserializeObject<RunStatus_Load837Database>(System.IO.File.ReadAllText(OperationFile));
            List<string> result = new List<string>();
            _logger.Log(LogLevel.Information, "Load Meditrac records to staging");
            Common.StartDate = Items[0];
            Common.EndDate = Items[1];
            if (Items[2] == "True") 
            {
                //CMS Professional
                Common.StatusCodes = "'TBP','ENC','CLD'";
                Common.GroupNumbers = "'H53'";
                Common.ClaimType = "HCF";
                int totalCMCP = _contextMeditrac.IntFromSQL(string.Format(Common.QueryMeditracCounts, Common.StatusCodes, Common.GroupNumbers, Common.ClaimType, Common.StartDate, Common.EndDate));
                Common.TotalCMSP = totalCMCP;
                Common.PageNumber = 0;
                if (totalCMCP > 0) 
                {
                    int pagesCMCP = (int)Math.Ceiling((decimal)totalCMCP / 10000);
                    for (int page = 0; page < pagesCMCP; page++)
                    {
                        Common.PageNumber = page;
                        Processes.Loading.LoadMeditracCMSP(_contextMeditrac, _context);
                    }
                }
            }
            if (Items[3] == "True") 
            {
                //CMS Institutional
                Common.StatusCodes = "'TBP','ENC','CLD'";
                Common.GroupNumbers = "'H53'";
                Common.ClaimType = "UB";
                Common common = new Common();
                int totalCMCI = _contextMeditrac.IntFromSQL(string.Format(Common.QueryMeditracCounts, Common.StatusCodes, Common.GroupNumbers, Common.ClaimType, Common.StartDate, Common.EndDate));
                Common.TotalCMSI = totalCMCI;
                if (totalCMCI > 0) 
                {
                    int pagesCMCI = (int)Math.Ceiling((decimal)totalCMCI / 10000);
                    for (int page = 0; page < pagesCMCI; page++)
                    {
                        Common.PageNumber = page;
                        Processes.Loading.LoadMeditracCMSI(_contextMeditrac, _context);
                    }
                }
            }
            if (Items[4] == "True") 
            {
                //Dual Propfessional
                Common.StatusCodes = "'TBP','ENC','CLD'";
                Common.GroupNumbers = "'810','812'";
                Common.ClaimType = "HCF";
                int totalCCIP = _contextMeditrac.IntFromSQL(string.Format(Common.QueryMeditracCounts, Common.StatusCodes, Common.GroupNumbers, Common.ClaimType, Common.StartDate, Common.EndDate));
                Common.TotalDualP = totalCCIP;
                if (totalCCIP > 0) 
                {
                    int pagesCCIP = (int)Math.Ceiling((decimal)totalCCIP / 10000);
                    for (int page = 0; page < pagesCCIP; page++)
                    {
                        Common.PageNumber = page;
                        Processes.Loading.LoadMeditracDualP(_contextMeditrac, _context);
                    }
                }
            }
            if (Items[5] == "True") 
            {
                //Dual Institutional
                Common.StatusCodes = "'TBP','ENC','CLD'";
                Common.GroupNumbers = "'810','812'";
                Common.ClaimType = "UB";
                int totalCCII = _contextMeditrac.IntFromSQL(string.Format(Common.QueryMeditracCounts, Common.StatusCodes, Common.GroupNumbers, Common.ClaimType, Common.StartDate, Common.EndDate));
                Common.TotalDualI = totalCCII;
                if (totalCCII > 0) 
                {
                    int pagesCCII = (int)Math.Ceiling((decimal)totalCCII / 10000);
                    for (int page = 0; page < pagesCCII; page++)
                    {
                        Common.PageNumber = page;
                        Processes.Loading.LoadMeditracDualI(_contextMeditrac, _context);
                    }
                }
            }
            if (Items[6] == "True") 
            {
                //State Professional
                Common.StatusCodes = "'TBP','ENC'";
                Common.GroupNumbers = "'305','306'";
                Common.ClaimType = "HCF";
                int totalStateP = _contextMeditrac.IntFromSQL(string.Format(Common.QueryMeditracCounts, Common.StatusCodes, Common.GroupNumbers, Common.ClaimType, Common.StartDate, Common.EndDate));
                Common.TotalStateP = totalStateP;
                if (totalStateP > 0) 
                {
                    int pagesStateP = (int)Math.Ceiling((decimal)totalStateP / 10000);
                    for (int page = 0; page < pagesStateP; page++)
                    {
                        Common.PageNumber = page;
                        Processes.Loading.LoadMeditracStateP(_contextMeditrac, _context);
                    }
                }
            }
            if (Items[7] == "True") 
            {
                //State Institutional
                Common.StatusCodes = "'TPB','ENC'";
                Common.GroupNumbers = "'305','306'";
                Common.ClaimType = "UB";
                int totalStateI = _contextMeditrac.IntFromSQL(string.Format(Common.QueryMeditracCounts, Common.StatusCodes, Common.GroupNumbers, Common.ClaimType, Common.StartDate, Common.EndDate));
                Common.TotalStateI = totalStateI;
                if (totalStateI > 0) 
                {
                    int pagesStateI = (int)Math.Ceiling((decimal)totalStateI / 10000);
                    for (int page = 0; page < pagesStateI; page++)
                    {
                        Common.PageNumber = page;
                        Processes.Loading.LoadMeditracStateI(_contextMeditrac, _context);
                    }
                }
            }
            result.Add(Common.TotalCMSI.ToString());
            result.Add(Common.ActualCMSI.ToString());
            result.Add(Common.TotalCMSP.ToString());
            result.Add(Common.ActualCMSP.ToString());
            result.Add(Common.TotalDualI.ToString());
            result.Add(Common.ActualDualI.ToString());
            result.Add(Common.TotalDualP.ToString());
            result.Add(Common.ActualDualP.ToString());
            result.Add(Common.TotalStateI.ToString());
            result.Add(Common.ActualStateI.ToString());
            result.Add(Common.TotalStateP.ToString());
            result.Add(Common.ActualStateP.ToString());
            runStatus.CurrentRunStatus = "0";
            System.IO.File.WriteAllText(OperationFile, JsonConvert.SerializeObject(runStatus, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            return result;
        }
    }
}
