using EncDataModel.CMSMAO001;
using EncLoadCMSMAO001.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EncLoadCMSMAO001.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CMSMAO001Controller : ControllerBase
    {
        private readonly ILogger<CMSMAO001Controller> _logger;
        private readonly CMSMAO001Context _context;
        public CMSMAO001Controller(ILogger<CMSMAO001Controller> logger, CMSMAO001Context context)
        {
            _logger = logger;
            _context = context;
        }
        //CMSMAO001
        [HttpGet]
        public List<string> GetNewCMSMAO001Files()
        {
            _logger.Log(LogLevel.Information, "inquiry unprocessed files");
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string sourcePath = configuration["Source_CMSMAO001"];
            string archivePath = configuration["Archive_CMSMAO001"];
            List<string> result = Directory.EnumerateFiles(sourcePath).Select(x => Path.GetFileName(x)).ToList();
            result.Insert(0, archivePath);
            result.Insert(0, sourcePath);
            return result;
        }
        //CMSMAO001/1
        [HttpGet("{id}")]
        public Tuple<int, int> ProcessCMSMAO001Files(long id)
        {
            _logger.Log(LogLevel.Information, "process new files");
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string sourcePath = configuration["Source_CMSMAO001"];
            string archivePath = configuration["Archive_CMSMAO001"];
            DirectoryInfo di = new DirectoryInfo(sourcePath);
            FileInfo[] fis = di.GetFiles();
            int totalFiles = fis.Length;
            int goodFiles = 0;
            foreach (FileInfo fi in fis)
            {
                _logger.Log(LogLevel.Information, "Processing file " + fi.Name + " now...");
                Mao001Header processingFile = _context.Headers.FirstOrDefault(x => x.FileName == fi.Name);
                if (processingFile != null) 
                {
                    _logger.Log(LogLevel.Information, fi.Name + " has been already processed");
                    MoveFile(archivePath, fi);
                    continue;
                }
                processingFile = new Mao001Header();
                string sMao001 = System.IO.File.ReadAllText(fi.FullName).Replace("\r", "");
                string[] sMao001Lines = sMao001.Split('\n');
                sMao001 = null;
                processingFile.FileName = fi.Name;
                _context.Headers.Add(processingFile);
                _context.SaveChanges();
                string[] segments;
                List<Mao001Detail> details = new List<Mao001Detail>();
                foreach (string line in sMao001Lines)
                {
                    segments = line.Split('*');
                    if (line.StartsWith("0*MAO-001"))
                    {
                        processingFile.SenderId = segments[6].Substring(0, 6);
                        processingFile.InterchangeControlNumber = segments[6].Substring(6, 9);
                        processingFile.InterchangeDate = segments[6].Substring(15, 8);
                        processingFile.RecordType = segments[7];
                        processingFile.ProductionFlag = segments[8];
                    }
                    else if (line.StartsWith("1*MAO-001"))
                    {
                        Mao001Detail detail = new Mao001Detail
                        {
                            HeaderId = processingFile.HeaderId,
                            ContractId = segments[2],
                            EncounterId = segments[3],
                            InternalControlNumber = segments[4].Trim(),
                            LineNumber = segments[5],
                            DupEncounterId = segments[6],
                            DupInternalControlNumber = segments[7].Trim(),
                            DupLineNumber = segments[8],
                            HICN = segments[9].Trim(),
                            DateOfService = segments[10],
                            ErrorCode = segments[11]
                        };
                        details.Add(detail);
                    }
                    else if (line.StartsWith("9*MAO-001"))
                    {
                        processingFile.TotalDupLines = segments[2];
                        processingFile.TotalLines = segments[3];
                        processingFile.TotalEncounters = segments[4];
                    }
                    if (details.Count >= 1000)
                    {
                        SaveBatch(ref details);
                    }
                }
                if (details.Count > 0)
                {
                    SaveBatch(ref details);
                }
                _context.SaveChanges();
                MoveFile(archivePath, fi);
                goodFiles++;
            }
            return Tuple.Create(totalFiles, goodFiles);
        }

        private void MoveFile(string archivePath, FileInfo fi)
        {
            if (System.IO.File.Exists(Path.Combine(archivePath, fi.Name))) System.IO.File.Delete(Path.Combine(archivePath, fi.Name));
            fi.MoveTo(Path.Combine(archivePath, fi.Name));
        }
        private void SaveBatch(ref List<Mao001Detail> details)
        {
            _context.Details.AddRange(details);
            _context.SaveChanges();
            details.Clear();
        } 
    }
}
