using EncDataModel.CMSMAO004;
using EncLoadCMSMAO004.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EncLoadCMSMAO004.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CMSMAO004Controller : ControllerBase
    {
        private readonly ILogger<CMSMAO004Controller> _logger;
        private readonly CMSMAO004Context _context;
        public CMSMAO004Controller(ILogger<CMSMAO004Controller> logger, CMSMAO004Context context)
        {
            _logger = logger;
            _context = context;
        }
        //CMSMAO004
        [HttpGet]
        public List<string> GetNewCMSMAO004Files()
        {
            _logger.Log(LogLevel.Information, "inquiry unprocessed files");
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string sourcePath = configuration["Source_CMSMAO004"];
            string archivePath = configuration["Archive_CMSMAO004"];
            List<string> result = Directory.EnumerateFiles(sourcePath).Select(x => Path.GetFileName(x)).ToList();
            result.Insert(0, archivePath);
            result.Insert(0, sourcePath);
            return result;
        }
        //CMSMAO004/1
        [HttpGet("{id}")]
        public Tuple<int, int> ProcessCMSMAO004Files(long id)
        {
            _logger.Log(LogLevel.Information, "process new MAO004 files");
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string sourcePath = configuration["Source_CMSMAO004"];
            string archivePath = configuration["Archive_CMSMAO004"];
            DirectoryInfo di = new DirectoryInfo(sourcePath);
            FileInfo[] fis = di.GetFiles();
            int totalFiles = fis.Length;
            int goodFiles = 0;
            foreach (FileInfo fi in fis)
            {
                _logger.Log(LogLevel.Information, "Processing file " + fi.Name + " now...");
                MAO_004_Header processingFile = _context.Headers.FirstOrDefault(x => x.FileName == fi.Name);
                if (processingFile != null)
                {
                    _logger.Log(LogLevel.Information, fi.Name + " has been already processed");
                    MoveFile(archivePath, fi);
                    continue;
                }
                string sMao004 = System.IO.File.ReadAllText(fi.FullName).Replace("\r", "");
                string[] sMao004Lines = sMao004.Split('\n');
                sMao004 = null;
                string[] temp = sMao004Lines[0].Split('*');
                processingFile = new MAO_004_Header
                {
                    TransmissionId=0,
                    ReportId=temp[1],
                    ContractId=temp[2],
                    ReportDate=DateTime.Parse(temp[3].Substring(4,2)+"/"+temp[3].Substring(6,2)+"/"+temp[3].Substring(0,4)),
                    ReportDescription=temp[4],
                    SubmissionFileType=temp[6],
                    RecordCount=0,
                    Phase = "3",
                    Version = "1",
                    CreateUser = Environment.UserName,
                    CreateDate = DateTime.Now,
                    FileName = fi.Name
                };
                _context.Headers.Add(processingFile);
                _context.SaveChanges();
                List<MAO_004_Detail> details = new List<MAO_004_Detail>();
                List<MAO_004_DiagnosisCode> diagCodes = new List<MAO_004_DiagnosisCode>();
                int maxId = 0;
                if(_context.Details.Count() > 0) maxId = _context.Details.Max(x => x.Id);
                string EncounterIcn = "";
                string ICD = null;
                foreach (string line in sMao004Lines)
                {
                    string[] segments = line.Split('*');
                    if (segments[0] == "1") //detail
                    {
                        if (EncounterIcn == segments[4])
                        {
                            for (int j = 0; j < 38; j++)
                            {
                                if (!string.IsNullOrEmpty(segments[j * 2 + 15].Trim()))
                                {
                                    MAO_004_DiagnosisCode diagCode = new MAO_004_DiagnosisCode
                                    {
                                        DetailId = maxId,
                                        Code = segments[j * 2 + 15].Trim(),
                                        ICD = ICD,
                                        Flag = segments[j * 2 + 16],
                                        CreateUser = Environment.UserName,
                                        CreateDate = DateTime.Now
                                    };
                                    diagCodes.Add(diagCode);
                                }
                                else 
                                {
                                    break;
                                }
                            }
                        }
                        else 
                        {
                            maxId++;
                            ICD = segments[14];
                            EncounterIcn = segments[4];
                            MAO_004_Detail detail = new MAO_004_Detail
                            {
                                Id = maxId,
                                HeaderId = processingFile.Id,
                                ReportId = segments[1],
                                MedicareContractId = segments[2],
                                BeneficiaryHICN = segments[3],
                                EncounterICN = segments[4],
                                EncounterTypeSwitch = segments[5],
                                OriginalEncounterICN = segments[6],
                                OriginalEncounterStatus = segments[7],
                                AllowDisallowFlag = segments[12],
                                AllowDisallowReasonCode = segments[13],
                                SubmissionDate=DateTime.Parse(segments[8].Substring(4, 2) + "/" + segments[8].Substring(6, 2) + "/" + segments[8].Substring(0, 4)),
                                ServiceStartDate=DateTime.Parse(segments[9].Substring(4, 2) + "/" + segments[9].Substring(6, 2) + "/" + segments[9].Substring(0, 4)),
                                ServiceEndDate=DateTime.Parse(segments[10].Substring(4, 2) + "/" + segments[10].Substring(6, 2) + "/" + segments[10].Substring(0, 4)),
                                ClaimType=segments[11],
                                CreateUser=Environment.UserName,
                                CreateDate=DateTime.Now
                            };
                            details.Add(detail);
                            for (int j = 0; j < 38; j++)
                            {
                                if (!string.IsNullOrEmpty(segments[j * 2 + 15].Trim()))
                                {
                                    MAO_004_DiagnosisCode diagCode = new MAO_004_DiagnosisCode
                                    {
                                        DetailId = maxId,
                                        Code = segments[j * 2 + 15].Trim(),
                                        ICD = ICD,
                                        Flag = segments[j * 2 + 16],
                                        CreateUser = Environment.UserName,
                                        CreateDate = DateTime.Now
                                    };
                                    diagCodes.Add(diagCode);
                                }
                                else 
                                {
                                    break;
                                }
                            }
                        }
                    }
                    else if (segments[0] == "9") //trailer
                    {
                        processingFile.RecordCount = int.Parse(segments[3]);
                    }
                    if (details.Count() >= 1000) 
                    {
                        SaveBatch(ref details, ref diagCodes);
                    }
                }
                SaveBatch(ref details, ref diagCodes);
                goodFiles++;
            }
            return Tuple.Create(totalFiles, goodFiles);
        }


        private void MoveFile(string archivePath, FileInfo fi)
        {
            if (System.IO.File.Exists(Path.Combine(archivePath, fi.Name))) System.IO.File.Delete(Path.Combine(archivePath, fi.Name));
            fi.MoveTo(Path.Combine(archivePath, fi.Name));
        }
        private void SaveBatch(ref List<MAO_004_Detail> details, ref List<MAO_004_DiagnosisCode> diagCodes) 
        {
            _context.Details.AddRange(details);
            _context.DiagCodes.AddRange(diagCodes);
            _context.SaveChanges();
            details.Clear();
            diagCodes.Clear();
        }
    }
}
