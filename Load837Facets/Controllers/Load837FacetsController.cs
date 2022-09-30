using EncDataModel.RunStatus;
using Load837Facets.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Load837Facets.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Load837FacetsController : ControllerBase
    {
        private readonly ILogger<Load837FacetsController> _logger;
        private readonly Sub837Context _context;
        private readonly ErrorContext _contextError;
        private readonly FacetsContext _contextFacets;
        public Load837FacetsController(ILogger<Load837FacetsController> logger, Sub837Context context, ErrorContext contextError, FacetsContext contextFacets)
        {
            _logger = logger;
            _context = context;
            _contextError = contextError;
            _contextFacets = contextFacets;
        }
        [HttpGet]
        public List<string> Load837Facets()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string sourcePath = configuration["Source_837IncludesExcludes"];
            string archivePath = configuration["Archive_837IncludesExcludes"];
            List<string> result = Directory.EnumerateFiles(sourcePath).Select(x => Path.GetFileName(x)).ToList();
            result.Insert(0, archivePath);
            result.Insert(0, sourcePath);
            return result;
        }
        [HttpPost]
        public List<string> Load837Facets([FromBody] List<string> Items)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string operationPath = configuration["OperationFolder"];
            string sourcePath = configuration["Source_837IncludesExcludes"];
            string archivePath = configuration["Archive_837IncludesExcludes"];
            Common.cn = configuration.GetConnectionString("Sub837Context");
            Common.cn_Facets = configuration.GetConnectionString("FacetsContext");
            string OperationFile = Path.Combine(operationPath, "Load837Facets", "Op" + DateTime.Today.ToString("yyyyMMdd") + ".json");
            RunStatus_Load837Database runStatus = JsonConvert.DeserializeObject<RunStatus_Load837Database>(System.IO.File.ReadAllText(OperationFile));
            List<string> result = new List<string>();
            _logger.Log(LogLevel.Information, "Load Facets records to staging");
            Common.StartDate = Items[0];
            Common.EndDate = Items[1];
            if (Items[9] == "True") 
            {
                //includes
                Common.Includes = new List<string>();
                DirectoryInfo di = new DirectoryInfo(sourcePath);
                FileInfo[] fis = di.GetFiles();
                foreach (FileInfo fi in fis) 
                {
                    if (fi.Name.ToUpper().Contains("INCLUDE")) 
                    {
                        string s = System.IO.File.ReadAllText(fi.FullName);
                        s = s.Replace("\r", "");
                        Common.Includes.AddRange(s.Split('\n'));
                        Common.Includes.RemoveAll(string.IsNullOrWhiteSpace);
                    }
                }
            }
            if (Items[10] == "True")
            {
                //excludes
                Common.Excludes = new List<string>();
                DirectoryInfo di = new DirectoryInfo(sourcePath);
                FileInfo[] fis = di.GetFiles();
                foreach (FileInfo fi in fis)
                {
                    if (fi.Name.ToUpper().Contains("EXCLUDE"))
                    {
                        string s = System.IO.File.ReadAllText(fi.FullName);
                        s = s.Replace("\r", "");
                        Common.Excludes.AddRange(s.Split('\n'));
                        Common.Excludes.RemoveAll(string.IsNullOrWhiteSpace);
                    }
                }
            }
            if (Items[8]=="True" && Items[2] == "True")
            {
                //CMS Professional
                Common.PlanId = "H53";
                Common.ClaimType = "M";
                string ssss = string.Format(Common.FacetsCount, Common.StartDate, Common.EndDate, Common.ClaimType);
                var vvvv = _contextFacets.FacetsCounts.FromSqlRaw(ssss);
                int totalCMCP = _contextFacets.FacetsCounts.FromSqlRaw(string.Format(Common.FacetsCount, Common.StartDate, Common.EndDate, Common.ClaimType)).First().ClaimCount;
                Common.TotalCMSP = totalCMCP;
                Common.PageNumber = 0;
                Processes.Loading.LoadFacetsCMSP(_contextFacets, _context);
                if (totalCMCP > 0)
                {
                    int pagesCMCP = (int)Math.Ceiling((decimal)totalCMCP / 10000);
                    for (int page = 0; page < pagesCMCP; page++)
                    {
                        Common.PageNumber = page;
                        Processes.Loading.LoadFacetsCMSP(_contextFacets, _context);
                    }
                }
            }
            if (Items[8] == "True" && Items[3] == "True")
            {
                //CMS Institutional
                Common.PlanId = "H53";
                Common.ClaimType = "H";
                int totalCMCI = _contextFacets.FacetsCounts.FromSqlRaw(string.Format(Common.FacetsCount, Common.StartDate, Common.EndDate, Common.ClaimType)).First().ClaimCount;
                Common.TotalCMSI = totalCMCI;
                Common.PageNumber = 0;
                if (totalCMCI > 0)
                {
                    int pagesCMCI = (int)Math.Ceiling((decimal)totalCMCI / 10000);
                    for (int page = 0; page < pagesCMCI; page++)
                    {
                        Common.PageNumber = page;
                        Processes.Loading.LoadFacetsCMSI(_contextFacets, _context);
                    }
                }
            }
            if (Items[8] == "True" && Items[4] == "True")
            {
                //Dual Propfessional
                Common.PlanId = "H53";
                Common.ClaimType = "M";
                int totalDualP = _contextFacets.FacetsCounts.FromSqlRaw(string.Format(Common.FacetsCount, Common.StartDate, Common.EndDate, Common.ClaimType)).First().ClaimCount;
                Common.TotalDualP = totalDualP;
                Common.PageNumber = 0;
                if (totalDualP > 0)
                {
                    int pagesDualP = (int)Math.Ceiling((decimal)totalDualP / 10000);
                    for (int page = 0; page < pagesDualP; page++)
                    {
                        Common.PageNumber = page;
                        Processes.Loading.LoadFacetsDualP(_contextFacets, _context);
                    }
                }
            }
            if (Items[8] == "True" && Items[5] == "True")
            {
                //Dual Institutional
                Common.PlanId = "H53";
                Common.ClaimType = "H";
                int totalDualI = _contextFacets.FacetsCounts.FromSqlRaw(string.Format(Common.FacetsCount, Common.StartDate, Common.EndDate, Common.ClaimType)).First().ClaimCount;
                Common.TotalDualI = totalDualI;
                Common.PageNumber = 0;
                if (totalDualI > 0)
                {
                    int pagesDualI = (int)Math.Ceiling((decimal)totalDualI / 10000);
                    for (int page = 0; page < pagesDualI; page++)
                    {
                        Common.PageNumber = page;
                        Processes.Loading.LoadFacetsDualI(_contextFacets, _context);
                    }
                }
            }
            if (Items[8] == "True" && Items[6] == "True")
            {
                //State Professional
                Common.PlanId = "H53";
                Common.ClaimType = "M";
                int totalStateP = _contextFacets.FacetsCounts.FromSqlRaw(string.Format(Common.FacetsCount, Common.StartDate, Common.EndDate, Common.ClaimType)).First().ClaimCount;
                Common.TotalStateP = totalStateP;
                Common.PageNumber = 0;
                if (totalStateP > 0)
                {
                    int pagesStateP = (int)Math.Ceiling((decimal)totalStateP / 10000);
                    for (int page = 0; page < pagesStateP; page++)
                    {
                        Common.PageNumber = page;
                        Processes.Loading.LoadFacetsStateP(_contextFacets, _context);
                    }
                }
            }
            if (Items[8] == "True" && Items[7] == "True")
            {
                //State Institutional
                Common.PlanId = "H53";
                Common.ClaimType = "H";
                int totalStateI = _contextFacets.FacetsCounts.FromSqlRaw(string.Format(Common.FacetsCount, Common.StartDate, Common.EndDate, Common.ClaimType)).First().ClaimCount;
                Common.TotalStateI = totalStateI;
                Common.PageNumber = 0;
                if (totalStateI > 0)
                {
                    int pagesStateI = (int)Math.Ceiling((decimal)totalStateI / 10000);
                    for (int page = 0; page < pagesStateI; page++)
                    {
                        Common.PageNumber = page;
                        Processes.Loading.LoadFacetsStateI(_contextFacets, _context);
                    }
                }
            }
            if (Common.Includes != null && Common.Includes.Count > 0) 
            {

            }
            runStatus.CurrentRunStatus = "0";
            System.IO.File.WriteAllText(OperationFile, JsonConvert.SerializeObject(runStatus, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            return result;
        }
    }
}
