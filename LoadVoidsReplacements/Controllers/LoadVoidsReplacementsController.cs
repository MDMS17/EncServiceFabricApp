using EncDataModel.RunStatus;
using LoadVoidsReplacements.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LoadVoidsReplacements.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoadVoidsReplacementsController : ControllerBase
    {
        private readonly ILogger<LoadVoidsReplacementsController> _logger;
        private readonly Sub837CacheContext _context;
        private readonly Sub837HistoryContext _contextHistory;
        private readonly CMSMAO002Context _contextMAO002;
        public LoadVoidsReplacementsController(ILogger<LoadVoidsReplacementsController> logger, Sub837CacheContext context, Sub837HistoryContext contextHistory, CMSMAO002Context contextMAO002)
        {
            _logger = logger;
            _context = context;
            _contextHistory = contextHistory;
            _contextMAO002 = contextMAO002;
        }
        //LoadVoidsReplacements
        [HttpGet]
        public List<string> GetSourceFiles4VoidsReplacements()
        {
            _logger.Log(LogLevel.Information, "inquiry source files for voids or replacements");
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string sourcePath = configuration["Source_837VoidsReplacements"];
            string archivePath = configuration["Archive_837VoidsReplacements"];
            List<string> result = Directory.EnumerateFiles(sourcePath).Select(x => Path.GetFileName(x)).ToList();
            result.Insert(0, archivePath);
            result.Insert(0, sourcePath);
            return result;
        }
        //LoadVoidsReplacements/1
        [HttpGet("{id}")]
        public List<string> LoadVoidsReplacementsFromFiles(long id)
        {
            List<string> result = new List<string>();
            _logger.Log(LogLevel.Information, "Load 837 voids or replacements from files");
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string sourcePath = configuration["Source_837VoidsReplacements"];
            string archivePath = configuration["Archive_837VoidsReplacements"];
            string operationPath = configuration["OperationFolder"];
            string OperationFile = Path.Combine(operationPath, "LoadVoidsReplacements", "Op" + DateTime.Today.ToString("yyyyMMdd") + ".json");
            RunStatus_LoadFileModel runStatus = JsonConvert.DeserializeObject<RunStatus_LoadFileModel>(System.IO.File.ReadAllText(OperationFile));
            DirectoryInfo di = new DirectoryInfo(sourcePath);
            FileInfo[] fis = di.GetFiles();
            int totalFiles = fis.Length;
            int goodFiles = 0;
            foreach (FileInfo fi in fis)
            {
                _logger.Log(LogLevel.Information, "Processing file " + fi.Name + " now...");
                string s = System.IO.File.ReadAllText(fi.FullName);
                s = s.Replace("\r", "");
                List<string> ClaimIds = s.Split('\n').ToList();
                string FrequencyCode = "7";
                if (fi.Name.ToUpper().Contains("VOID")) FrequencyCode = "8";
                int counter = 0;
                foreach (string ClaimId in ClaimIds) 
                {
                    var responseReocrd = _contextMAO002.Details.OrderByDescending(x => x.DetailId).FirstOrDefault(x => x.ClaimId == ClaimId && x.EncounterStatus == "Accepted");
                    if (Response == null) continue;
                    var claimHeader = _contextHistory.ClaimHeaders.OrderByDescending(x => x.ID).FirstOrDefault(x => x.ClaimID == ClaimId);
                    if (claimHeader == null) continue;
                    claimHeader.ClaimFrequencyCode = FrequencyCode;
                    _context.ClaimHeaders.Add(claimHeader);
                    _context.ClaimCAS.AddRange(_context.ClaimCAS.Where(x => x.FileID == claimHeader.FileID && x.ClaimID == ClaimId));
                    _context.ClaimCRCs.AddRange(_context.ClaimCRCs.Where(x => x.FileID == claimHeader.FileID && x.ClaimID==ClaimId));
                    _context.ClaimHIs.AddRange(_context.ClaimHIs.Where(x => x.FileID == claimHeader.FileID && x.ClaimID==ClaimId));
                    _context.ClaimK3s.AddRange(_context.ClaimK3s.Where(x => x.FileID == claimHeader.FileID && x.ClaimID==ClaimId));
                    _context.ClaimLineFRMs.AddRange(_context.ClaimLineFRMs.Where(x => x.FileID == claimHeader.FileID && x.ClaimID==ClaimId));
                    _context.ClaimLineLQs.AddRange(_context.ClaimLineLQs.Where(x => x.FileID == claimHeader.FileID && x.ClaimID==ClaimId));
                    _context.ClaimLineMEAs.AddRange(_context.ClaimLineMEAs.Where(x => x.FileID == claimHeader.FileID && x.ClaimID==ClaimId));
                    _context.ClaimLineSVDs.AddRange(_context.ClaimLineSVDs.Where(x => x.FileID == claimHeader.FileID && x.ClaimID==ClaimId));
                    _context.ClaimNtes.AddRange(_context.ClaimNtes.Where(x => x.FileID == claimHeader.FileID && x.ClaimID==ClaimId));
                    _context.ClaimPatients.AddRange(_context.ClaimPatients.Where(x => x.FileID == claimHeader.FileID && x.ClaimID==ClaimId));
                    _context.ClaimProviders.AddRange(_context.ClaimProviders.Where(x => x.FileID == claimHeader.FileID && x.ClaimID==ClaimId));
                    _context.ClaimPWKs.AddRange(_context.ClaimPWKs.Where(x => x.FileID == claimHeader.FileID && x.ClaimID==ClaimId));
                    _context.ClaimSBRs.AddRange(_context.ClaimSBRs.Where(x => x.FileID == claimHeader.FileID && x.ClaimID==ClaimId));
                    _context.ClaimSecondaryIdentifications.AddRange(_context.ClaimSecondaryIdentifications.Where(x => x.FileID == claimHeader.FileID && x.ProviderQualifier!="F8" && x.ClaimID==ClaimId));
                    _context.ProviderContacts.AddRange(_context.ProviderContacts.Where(x => x.FileID == claimHeader.FileID && x.ClaimID==ClaimId));
                    _context.ServiceLines.AddRange(_context.ServiceLines.Where(x => x.FileID == claimHeader.FileID && x.ClaimID==ClaimId));
                    _context.ToothStatus.AddRange(_context.ToothStatus.Where(x => x.FileID == claimHeader.FileID && x.ClaimID==ClaimId));
                    _context.ClaimSecondaryIdentifications.Add(new EncDataModel.Submission837.ClaimSecondaryIdentification
                    {
                        FileID=claimHeader.FileID,
                        ClaimID=ClaimId,
                        ServiceLineNumber="0",
                        LoopName="2300",
                        ProviderQualifier="F8",
                        ProviderID=responseReocrd.InternalControlNumber
                    });
                    counter++;
                    if (counter >= 1000) 
                    {
                        _context.SaveChanges();
                        counter = 0;
                    }
                }
                _context.SaveChanges();
                MoveFile(archivePath, fi);
                runStatus.FileNames.Add(fi.Name);
            }
            result.Add(totalFiles.ToString());
            result.Add(goodFiles.ToString());
            runStatus.CurrentRunStatus = "0";
            System.IO.File.WriteAllText(OperationFile, JsonConvert.SerializeObject(runStatus, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            return result;
        }
        private void MoveFile(string archivePath, FileInfo fi)
        {
            if (System.IO.File.Exists(Path.Combine(archivePath, fi.Name))) System.IO.File.Delete(Path.Combine(archivePath, fi.Name));
            fi.MoveTo(Path.Combine(archivePath, fi.Name));
        }
    }
}
