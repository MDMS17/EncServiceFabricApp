using EncDataModel.CMSMAO002;
using EncLoadCMSMAO002.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EncLoadCMSMAO002.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CMSMAO002Controller : ControllerBase
    {
        private readonly ILogger<CMSMAO002Controller> _logger;
        private readonly CMSMAO002Context _context;
        public CMSMAO002Controller(ILogger<CMSMAO002Controller> logger, CMSMAO002Context context)
        {
            _logger = logger;
            _context = context;
        }
        //CMSMAO002
        [HttpGet]
        public List<string> GetNewCMSMAO002Files()
        {
            _logger.Log(LogLevel.Information, "inquiry unprocessed files");
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string sourcePath = configuration["Source_CMSMAO002"];
            string archivePath = configuration["Archive_CMSMAO002"];
            List<string> result = Directory.EnumerateFiles(sourcePath).Select(x => Path.GetFileName(x)).ToList();
            result.Insert(0, archivePath);
            result.Insert(0, sourcePath);
            return result;
        }
        //CMSMAO002/1
        [HttpGet("{id}")]
        public Tuple<int, int> ProcessCMSMAO002Files(long id)
        {
            _logger.Log(LogLevel.Information, "process new files");
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string sourcePath = configuration["Source_CMSMAO002"];
            string archivePath = configuration["Archive_CMSMAO002"];
            DirectoryInfo di = new DirectoryInfo(sourcePath);
            FileInfo[] fis = di.GetFiles();
            int totalFiles = fis.Length;
            int goodFiles = 0;
            foreach (FileInfo fi in fis)
            {
                _logger.Log(LogLevel.Information, "Processing file " + fi.Name + " now...");
                MAO2File processingFile = _context.Files.FirstOrDefault(x => x.FileName == fi.Name);
                if (processingFile != null)
                {
                    _logger.Log(LogLevel.Information, fi.Name + " has been already processed");
                    MoveFile(archivePath, fi);
                    continue;
                }
                processingFile = new MAO2File
                {
                    FileName = fi.Name,
                    CreateUser = Environment.UserName,
                    CreateDate = DateTime.Today
                };
                string sMao002 = System.IO.File.ReadAllText(fi.FullName).Replace("\r", "");
                string[] sMao002Lines = sMao002.Split('\n');
                sMao002 = null;
                processingFile.FileName = fi.Name;
                _context.Files.Add(processingFile);
                _context.SaveChanges();
                List<MAO2Detail> details = new List<MAO2Detail>();
                foreach (string line in sMao002Lines)
                {
                    ParserMao2Line(line, ref processingFile, ref details);
                }
                _context.Details.AddRange(details);
                _context.SaveChanges();
                goodFiles++;
            }
            return Tuple.Create(totalFiles, goodFiles);
        }


        private void MoveFile(string archivePath, FileInfo fi)
        {
            if (System.IO.File.Exists(Path.Combine(archivePath, fi.Name))) System.IO.File.Delete(Path.Combine(archivePath, fi.Name));
            fi.MoveTo(Path.Combine(archivePath, fi.Name));
        }
        private void ParserMao2Line(string line, ref MAO2File processingFile, ref List<MAO2Detail> details)
        {
            string[] segments = line.Split('*');
            if (segments[0] == "0") //header
            {
                processingFile.TransactionDate = segments[3];
                processingFile.SenderId = segments[6].Substring(0, 6);
                processingFile.ICN = segments[6].Substring(6, 9);
                processingFile.RecordType = segments[7];
                processingFile.ProductionFlag = segments[8];
            }
            else if (segments[0] == "1") //detail
            {
                MAO2Detail detail = new MAO2Detail
                {
                    FileId = processingFile.FileId,
                    ClaimId = segments[3].Trim(),
                    InternalControlNumber = segments[4],
                    LineNumber = segments[5],
                    EncounterStatus = segments[6],
                    ErrorCode = segments[7],
                    ErrorDescription = segments[8]
                };
                details.Add(detail);
            }
            else if (segments[0] == "9") //trailer
            {
                processingFile.TotalErrors = segments[2];
                processingFile.TotalLinesAccepted = segments[3];
                processingFile.TotalLinesRejected = segments[4];
                processingFile.TotalLinesSubmitted = segments[5];
                processingFile.TotalEncountersAccepted = segments[6];
                processingFile.TotalEncountersRejected = segments[7];
                processingFile.TotalEncountersSubmitted = segments[8];
            }
        }
    }
}
