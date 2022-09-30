using EncDataModel.CMS277CA;
using EncLoadCMS277CA.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace EncLoadCMS277CA.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CMS277CAController : ControllerBase
    {
        private readonly ILogger<CMS277CAController> _logger;
        private readonly CMS277CAContext _context;
        public CMS277CAController(ILogger<CMS277CAController> logger, CMS277CAContext context)
        {
            _logger = logger;
            _context = context;
        }
        //CMS277CA
        [HttpGet]
        public List<string> GetNewCMS277CAFiles()
        {
            _logger.Log(LogLevel.Information, "inquiry unprocessed files");
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string sourcePath = configuration["Source_CMS277CA"];
            string archivePath = configuration["Archive_CMS277CA"];
            List<string> result = Directory.EnumerateFiles(sourcePath).Select(x => Path.GetFileName(x)).ToList();
            result.Insert(0, archivePath);
            result.Insert(0, sourcePath);
            return result;
        }
        //CMS277CA/1
        [HttpGet("{id}")]
        public Tuple<int, int> ProcessCMS277CAFiles(long id)
        {
            _logger.Log(LogLevel.Information, "process new files");
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string sourcePath = configuration["Source_CMS277CA"];
            string archivePath = configuration["Archive_CMS277CA"];
            DirectoryInfo di = new DirectoryInfo(sourcePath);
            FileInfo[] fis = di.GetFiles();
            int totalFiles = fis.Length;
            int goodFiles = 0;
            foreach (FileInfo fi in fis)
            {
                List<_277CABillProv> billProvs = new List<_277CABillProv>();
                List<_277CAPatient> patients = new List<_277CAPatient>();
                List<_277CALine> lines = new List<_277CALine>();
                List<_277CAStc> stcs = new List<_277CAStc>();
                _277CAFile processingFile = _context.Files.FirstOrDefault(x => x.FileName == fi.Name);
                if (processingFile != null)
                {
                    _logger.Log(LogLevel.Information, "File " + fi.Name + " already processed before");
                    MoveFile(archivePath, fi);
                    continue;
                }
                string s277CA = System.IO.File.ReadAllText(fi.FullName).Replace("\r", "").Replace("\n", "");
                string[] s277CALines = s277CA.Split('~');
                s277CA = null;
                int encounterCount = s277CALines.Count(x => x.StartsWith("TRN*2*")) - 1;
                if (encounterCount <= 0)
                {
                    _logger.Log(LogLevel.Information, "File " + fi.Name + " not valid");
                    MoveFile(archivePath, fi);
                    continue;
                }
                _logger.Log(LogLevel.Information, "Processing file " + fi.Name + " total records: " + encounterCount.ToString());

                processingFile = new _277CAFile();
                processingFile.FileName = fi.Name;

                string tempSeg = s277CALines[1];
                string[] tempArray = tempSeg.Split('*');
                processingFile.ReceiverId = tempArray[2];
                processingFile.SenderId = tempArray[3];
                processingFile.TransactionDate = tempArray[4];
                processingFile.TransactionTime = tempArray[5];
                processingFile.GroupControlNumber = tempArray[6];
                tempSeg = s277CALines[5];
                tempArray = tempSeg.Split('*');
                processingFile.ReceiverName = tempArray[3];
                tempSeg = s277CALines[10];
                tempArray = tempSeg.Split('*');
                processingFile.SenderName = tempArray[3];
                tempSeg = s277CALines[11];
                tempArray = tempSeg.Split('*');
                processingFile.BatchId = tempArray[2];
                tempSeg = s277CALines[0];
                tempArray = tempSeg.Split('*');
                processingFile.ICN = tempArray[13];
                char elementDelimiter = (char)tempArray[16].ToCharArray()[0];
                processingFile.CreateUser = Environment.UserName;
                processingFile.CreateDate = DateTime.Today;
                _context.Files.Add(processingFile);
                _context.SaveChanges();

                string LoopName = "";
                foreach (string s277CALine in s277CALines)
                {
                    Parse277CALine(s277CALine, ref processingFile, ref billProvs, ref patients,
                        ref lines, ref stcs, ref LoopName, ref elementDelimiter);
                }

                _context.BillProvs.AddRange(billProvs);
                _context.Patients.AddRange(patients);
                _context.Lines.AddRange(lines);
                _context.Stcs.AddRange(stcs);
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
        private void Parse277CALine(string line, ref _277CAFile processingFile, ref List<_277CABillProv> billProvs,
            ref List<_277CAPatient> patients, ref List<_277CALine> lines, ref List<_277CAStc> stcs, ref string loopName, ref char elementDelimiter)
        {
            string[] segments = line.Split('*');
            switch (segments[0])
            {
                case "HL":
                    switch (segments[3])
                    {
                        case "20":
                            loopName = "2000A"; //receiver
                            break;
                        case "21":
                            loopName = "2000B"; //sender
                            break;
                        case "19":
                            loopName = "2000C"; //billing provider
                            break;
                        case "PT":
                            loopName = "2000D"; //patient
                            break;
                    }

                    break;
                case "STC":
                    _277CAStc stc = new _277CAStc();
                    switch (loopName)
                    {
                        case "2000B":
                            stc.StcType = "File";
                            stc.FileId = processingFile.FileId;
                            break;
                        case "2000C":
                            stc.StcType = "BillProv";
                            stc.FileId = processingFile.FileId;
                            stc.BillProvId = billProvs.Last().BillProvId;
                            stc.ClaimId = billProvs.Last().ClaimId;
                            break;
                        case "2000D":
                            stc.StcType = "Patient";
                            stc.FileId = processingFile.FileId;
                            stc.BillProvId = billProvs.Last().BillProvId;
                            stc.ClaimId = patients.Last().ClaimId;
                            stc.PatientId = patients.Last().PatientId;
                            break;
                        case "2220D":
                            stc.StcType = "Line";
                            stc.FileId = processingFile.FileId;
                            stc.BillProvId = billProvs.Last().BillProvId;
                            stc.ClaimId = patients.Last().ClaimId;
                            stc.PatientId = patients.Last().PatientId;
                            break;
                    }

                    string[] elements = segments[1].Split(elementDelimiter);
                    stc.ClaimStatusCategory1 = elements[0];
                    stc.ClaimStatusCode1 = elements[1];
                    if (elements.Length > 2) stc.EntityIdentifier1 = elements[2];
                    stc.StatusInfoEffDate = segments[2];
                    stc.ActionCode = segments[3];
                    if (segments.Length > 4) stc.ChargeAmount = segments[4];
                    if (segments.Length > 10)
                    {
                        elements = segments[10].Split(elementDelimiter);
                        stc.ClaimStatusCategory2 = elements[0];
                        stc.ClaimStatusCode2 = elements[1];
                        if (elements.Length > 2) stc.EntityIdentifier2 = elements[2];
                    }

                    if (segments.Length > 11)
                    {
                        elements = segments[11].Split(elementDelimiter);
                        stc.ClaimStatusCategory3 = elements[0];
                        stc.ClaimStatusCode3 = elements[1];
                        if (elements.Length > 2) stc.EntityIdentifier3 = elements[2];
                    }
                    stcs.Add(stc);
                    break;
                case "QTY":
                    if (loopName == "2000B")
                    {
                        if (segments[1] == "90") processingFile.TotalAcceptedQuantity = segments[2];
                        if (segments[1] == "AA") processingFile.TotalRejectedQuantity = segments[2];
                    }
                    else if (loopName == "2000C")
                    {
                        if (segments[1] == "QA") billProvs.Last().BillProvAcceptedQuantity = segments[2];
                        if (segments[1] == "QC") billProvs.Last().BillProvRejectedQuantity = segments[2];
                    }

                    break;
                case "AMT":
                    if (loopName == "2000B")
                    {
                        if (segments[1] == "YU") processingFile.TotalAcceptedAmount = segments[2];
                        if (segments[1] == "YY") processingFile.TotalRejectedAmount = segments[2];
                    }
                    else if (loopName == "2000C")
                    {
                        if (segments[1] == "YU") billProvs.Last().BillProvAcceptedAmount = segments[2];
                        if (segments[1] == "YY") billProvs.Last().BillProvRejectedAmount = segments[2];
                    }

                    break;
                case "NM1":
                    if (loopName == "2000C")
                    {
                        _277CABillProv billProv = new _277CABillProv()
                        {
                            FileId = processingFile.FileId,
                            BillProvName = segments[3],
                            BillProvIdQual = segments[8],
                            BillProvId = segments[9]
                        };
                        billProvs.Add(billProv);
                    }
                    else if (loopName == "2000D")
                    {
                        _277CAPatient patient = new _277CAPatient
                        {
                            FileId = processingFile.FileId,
                            BillProvId = billProvs.Last().BillProvId,
                            PatientLastName = segments[3],
                            PatientFirstName = segments[4],
                            PatientMI = segments[5],
                            PatientIdQual = segments[8],
                            PatientId = segments[9]
                        };
                        patients.Add(patient);
                    }

                    break;
                case "TRN":
                    if (loopName == "2000C" && segments[1] == "1")
                    {
                        billProvs.Last().ClaimId = segments[2];
                    }
                    else if (loopName == "2000D" && segments[1] == "2")
                    {
                        patients.Last().ClaimId = segments[2];
                    }

                    break;
                case "REF":
                    if (loopName == "2000C")
                    {
                        if (string.IsNullOrEmpty(billProvs.Last().BillProvSecondIdQual1))
                        {
                            billProvs.Last().BillProvSecondIdQual1 = segments[1];
                            billProvs.Last().BillProvSecondId1 = segments[2];
                        }
                        else if (string.IsNullOrEmpty(billProvs.Last().BillProvSecondIdQual2))
                        {
                            billProvs.Last().BillProvSecondIdQual2 = segments[1];
                            billProvs.Last().BillProvSecondId2 = segments[2];
                        }
                        else if (string.IsNullOrEmpty(billProvs.Last().BillProvSecondIdQual3))
                        {
                            billProvs.Last().BillProvSecondIdQual3 = segments[1];
                            billProvs.Last().BillProvSecondId3 = segments[2];
                        }
                    }
                    else if (loopName == "2000D")
                    {
                        switch (segments[1])
                        {
                            case "1K":
                                patients.Last().PayerClaimControlNumber = segments[2];
                                break;
                            case "D9":
                                patients.Last().ClearingHouseTraceNumber = segments[2];
                                break;
                            case "BLT":
                                patients.Last().BillType = segments[2];
                                break;
                        }
                    }
                    else if (loopName == "2220D")
                    {
                        if (segments[1] == "FJ")
                        {
                            lines.Last().LineItemControlNumber = segments[2];
                        }
                    }

                    break;
                case "DTP":
                    if (loopName == "2000D")
                    {
                        if (segments[1] == "472")
                        {
                            if (segments[2] == "RD8")
                            {
                                patients.Last().ServiceDateFrom = segments[3].Split('-')[0];
                                patients.Last().ServiceDateTo = segments[3].Split('-')[1];
                            }
                            else if (segments[2] == "D8")
                            {
                                patients.Last().ServiceDateFrom = segments[3];
                            }
                        }
                    }
                    else if (loopName == "2220D")
                    {
                        if (segments[1] == "472")
                        {
                            if (segments[2] == "RD8")
                            {
                                lines.Last().ServiceDateFrom = segments[3].Split('-')[0];
                                lines.Last().ServiceDateTo = segments[3].Split('-')[1];
                            }
                            else if (segments[2] == "D8")
                            {
                                lines.Last().ServiceDateFrom = segments[3];
                            }
                        }
                    }

                    break;
                case "SVC":
                    loopName = "2220D";
                    string[] tempArray = segments[1].Split(elementDelimiter);
                    _277CALine line2 = new _277CALine
                    {
                        FileId = processingFile.FileId,
                        ClaimId = patients.Last().ClaimId,
                        ProcedureQual = tempArray[0],
                        ProcedureCode = tempArray[1],
                        Modifier1 = tempArray.Length > 2 ? tempArray[2] : null,
                        Modifier2 = tempArray.Length > 3 ? tempArray[3] : null,
                        Modifier3 = tempArray.Length > 4 ? tempArray[4] : null,
                        Modifier4 = tempArray.Length > 5 ? tempArray[5] : null,
                        LineChargeAmount = segments[2],
                        RevenueCode = segments.Length > 4 ? segments[4] : null,
                        UnitCount = segments.Length > 7 ? segments[7] : null
                    };
                    lines.Add(line2);
                    break;
            }
        }
    }
}
