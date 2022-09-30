using DHCSEVR.Data;
using EncDataModel.DHCS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DHCSEVR.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DHCSEVRController : ControllerBase
    {
        private readonly ILogger<DHCSEVRController> _logger;
        private readonly DHCSEVRContext _context;
        public DHCSEVRController(ILogger<DHCSEVRController> logger, DHCSEVRContext context)
        {
            _logger = logger;
            _context = context;
        }
        //DHCSEVR
        [HttpGet]
        public List<string> GetNewDHCSEVRFiles()
        {
            _logger.Log(LogLevel.Information, "inquiry unprocessed files");
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string sourcePath = configuration["Source_DHCS"];
            string archivePath = configuration["Archive_DHCS"];
            List<string> result = Directory.EnumerateFiles(sourcePath).Select(x => Path.GetFileName(x)).ToList();
            result.Insert(0, archivePath);
            result.Insert(0, sourcePath);
            return result;
        }
        //DHCSEVR/1
        [HttpGet("{id}")]
        public Tuple<int, int> ProcessDHCSEVRFiles(long id)
        {
            _logger.Log(LogLevel.Information, "process new DHCS EVR files");
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string sourcePath = configuration["Source_DHCS"];
            string archivePath = configuration["Archive_DHCS"];
            DirectoryInfo di = new DirectoryInfo(sourcePath);
            FileInfo[] fis = di.GetFiles();
            int totalFiles = fis.Length;
            int goodFiles = 0;
            foreach (FileInfo fi in fis)
            {
                _logger.Log(LogLevel.Information, "Processing file " + fi.Name + " now...");
                DHCSFile processingFile = _context.Files.FirstOrDefault(x => x.FileName == fi.Name);
                if (processingFile != null)
                {
                    _logger.Log(LogLevel.Information, fi.Name + " has been already processed");
                    MoveFile(archivePath, fi);
                    continue;
                }
                List<DHCSTransaction> transactions = new List<DHCSTransaction>();
                List<DHCSEncounter> encounters = new List<DHCSEncounter>();
                List<DHCSEncounterResponse> responses = new List<DHCSEncounterResponse>();
                List<DHCSServiceLine> lines = new List<DHCSServiceLine>();
                XDocument xDoc = XDocument.Load(fi.FullName);
                XNamespace ns = "http://www.dhcs.ca.gov/EDS/DHCSResponse";
                processingFile = new DHCSFile
                {
                    FileName = fi.Name,
                    EncounterFileName = xDoc.Descendants(ns + "EncounterFileName").FirstOrDefault()?.Value,
                    SubmitterName =
                    xDoc.Descendants(ns + "EncounterSubmitterName").FirstOrDefault()?.Value,
                    SubmissionDate =
                    xDoc.Descendants(ns + "EncounterSubmissionDate").FirstOrDefault()?.Value,
                    ValidationStatus = xDoc.Descendants(ns + "ValidationStatus").FirstOrDefault()?.Value,
                    CreateUser = Environment.UserName,
                    CreateDate = DateTime.Today
                };
                _context.Files.Add(processingFile);
                _context.SaveChanges();
                foreach (XElement eleTransaction in xDoc.Descendants(ns + "Transaction"))
                {
                    DHCSTransaction transaction = new DHCSTransaction();
                    transaction.FileId = processingFile.FileId;
                    transaction.TransactionStatus = eleTransaction.Attributes("Status").FirstOrDefault()?.Value;
                    transaction.TransactionNumber =
                        eleTransaction.Descendants(ns + "TransactionNumber").FirstOrDefault()?.Value;
                    foreach (XElement ele in eleTransaction.Descendants(ns + "Identifiers").Descendants(ns + "Envelope"))
                    {
                        switch (ele.Attributes("IdentifierName").FirstOrDefault()?.Value)
                        {
                            case "ISAControlNumber":
                                transaction.ISAControlNumber =
                                    ele.Attributes("IdentifierValue").FirstOrDefault()?.Value;
                                break;
                            case "GroupControlNumber":
                                transaction.GroupControlNumber =
                                    ele.Attributes("IdentifierValue").FirstOrDefault()?.Value;
                                break;
                            case "OriginatorTransactionId":
                                transaction.OriginatorTransactionId =
                                    ele.Attributes("IdentifierValue").FirstOrDefault()?.Value;
                                break;
                        }
                    }
                    transactions.Add(transaction);
                    foreach (XElement eleEncounter in eleTransaction.Descendants(ns + "Encounter"))
                    {
                        DHCSEncounter encounter = new DHCSEncounter
                        {
                            FileId = processingFile.FileId,
                            TransactionNumber = transactions.Last().TransactionNumber,
                            EncounterStatus = eleEncounter.Attributes("Status").FirstOrDefault()?.Value,
                            EncounterReferenceNumber =
                            eleEncounter.Descendants(ns + "EncounterReferenceNumber").FirstOrDefault()?.Value,
                            DHCSEncounterId = eleEncounter.Descendants(ns + "EncounterId").FirstOrDefault()?.Value
                        };
                        encounters.Add(encounter);
                        foreach (XElement eleResponse in eleEncounter.Descendants(ns + "Response"))
                        {
                            DHCSEncounterResponse response = new DHCSEncounterResponse
                            {
                                FileId = processingFile.FileId,
                                TransactionNumber = transactions.Last().TransactionNumber,
                                EncounterReferenceNumber = encounters.Last().EncounterReferenceNumber,
                                Severity = eleResponse.Attributes("Severity").FirstOrDefault()?.Value,
                                IssueId = eleResponse.Descendants(ns + "Id").FirstOrDefault()?.Value,
                                IsSNIP = eleResponse.Descendants(ns + "IsSNIP").FirstOrDefault()?.Value,
                                IssueDescription =
                                eleResponse.Descendants(ns + "Description").FirstOrDefault()?.Value
                            };
                            responses.Add(response);
                        }

                        foreach (XElement eleService in eleEncounter.Descendants(ns + "Service"))
                        {
                            DHCSServiceLine line = new DHCSServiceLine
                            {
                                FileId = processingFile.FileId,
                                TransactionNumber = transactions.Last().TransactionNumber,
                                EncounterReferenceNumber = encounters.Last().EncounterReferenceNumber,
                                Line = eleService.Attributes().FirstOrDefault()?.Value,
                                Severity = eleService.Descendants(ns + "Response").FirstOrDefault()?.Attributes().FirstOrDefault()?.Value,
                                Id = eleService.Descendants(ns + "Id").FirstOrDefault()?.Value,
                                Description = eleService.Descendants(ns + "Description").FirstOrDefault()?.Value
                            };
                            lines.Add(line);
                        }
                    }
                }

                _context.Transactions.AddRange(transactions);
                _context.Encounters.AddRange(encounters);
                _context.Responses.AddRange(responses);
                _context.ServiceLines.AddRange(lines);
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
    }
}
