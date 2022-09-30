using EncDataModel.RunStatus;
using EncDataModel.Submission837;
using Load837SubHistory.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using X12Lib;
using Newtonsoft.Json;

namespace Load837SubHistory.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Load837SubHistoryController : ControllerBase
    {
        private readonly ILogger<Load837SubHistoryController> _logger;
        private readonly Sub837HistoryContext _context;
        public Load837SubHistoryController(ILogger<Load837SubHistoryController> logger, Sub837HistoryContext context)
        {
            _logger = logger;
            _context = context;
        }
        //Load837SubHistory
        [HttpGet]
        public List<string> GetSource837SubHistoryFiles()
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
        public List<string> Load837SubHistoryFiles(long id)
        {
            List<string> result = new List<string>();
            _logger.Log(LogLevel.Information, "Load 837 files to database");
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string sourcePath = configuration["Source_837File"];
            string archivePath = configuration["Archive_837File"];
            string operationPath = configuration["OperationFolder"];
            string OperationFile = Path.Combine(operationPath, "Load837SubHistory", "Op" + DateTime.Today.ToString("yyyyMMdd") + ".json");
            RunStatus_LoadFileModel runStatus = JsonConvert.DeserializeObject<RunStatus_LoadFileModel>(System.IO.File.ReadAllText(OperationFile));
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
                    SaveValidClaims(ref claims);
                    goodFiles++;
                }
                MoveFile(archivePath, fi);
                runStatus.FileNames.Add(fi.Name);
            }
            result.Add(totalFiles.ToString());
            result.Add(goodFiles.ToString());
            runStatus.CurrentRunStatus = "0";
            System.IO.File.WriteAllText(OperationFile, JsonConvert.SerializeObject(runStatus, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            return result;
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
        private void MoveFile(string archivePath, FileInfo fi)
        {
            if (System.IO.File.Exists(Path.Combine(archivePath, fi.Name))) System.IO.File.Delete(Path.Combine(archivePath, fi.Name));
            fi.MoveTo(Path.Combine(archivePath, fi.Name));
        }
    }
}
