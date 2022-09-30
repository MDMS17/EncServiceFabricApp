using System;
using EncDataModel.CMS999;
using EncLoadCMS999.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace EncLoadCMS999.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CMS999Controller : ControllerBase
    {
        private readonly ILogger<CMS999Controller> _logger;
        private readonly CMS999Context _context;
        public CMS999Controller(ILogger<CMS999Controller> logger, CMS999Context context)
        {
            _logger = logger;
            _context = context;
        }
        //CMS999
        [HttpGet]
        public List<string> GetNewCMS999Files()
        {
            _logger.Log(LogLevel.Information, "inquiry unprocessed files");
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string sourcePath = configuration["Source_CMS999"];
            string archivePath = configuration["Archive_CMS999"];
            List<string> result = Directory.EnumerateFiles(sourcePath).Select(x => Path.GetFileName(x)).ToList();
            result.Insert(0, archivePath);
            result.Insert(0, sourcePath);
            return result;
        }
        //CMS999/1
        [HttpGet("{id}")]
        public Tuple<int, int> ProcessCMS999Files(long id)
        {
            _logger.Log(LogLevel.Information, "process new files");
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string sourcePath = configuration["Source_CMS999"];
            string archivePath = configuration["Archive_CMS999"];
            DirectoryInfo di = new DirectoryInfo(sourcePath);
            FileInfo[] fis = di.GetFiles();
            int totalFiles = fis.Length;
            int goodFiles = 0;
            foreach (FileInfo fi in fis)
            {
                List<_999Transaction> transactions = new List<_999Transaction>();
                List<_999Error> errors = new List<_999Error>();
                List<_999Element> elements = new List<_999Element>();
                _999File processingFile = _context.Files.FirstOrDefault(x => x.FileName == fi.Name);
                if (processingFile != null)
                {
                    _logger.Log(LogLevel.Information, "File " + fi.Name + " already processed before");
                    MoveFile(archivePath, fi);
                    continue;
                }
                string s999 = System.IO.File.ReadAllText(fi.FullName);
                s999 = s999.Replace("\r", "").Replace("\n", "");
                string[] s999Lines = s999.Split('~');
                s999 = null;

                processingFile = new _999File();
                processingFile.FileName = fi.Name;

                string tempSeg = s999Lines[1];
                string[] tempArray = tempSeg.Split('*');
                processingFile.ReceiverId = tempArray[2];
                processingFile.SenderId = tempArray[3];
                processingFile.TransactionDate = tempArray[4];
                processingFile.TransactionTime = tempArray[5];
                processingFile.GroupControlNumber = tempArray[6];
                tempSeg = s999Lines[0];
                tempArray = tempSeg.Split('*');
                processingFile.ICN = tempArray[13];
                processingFile.ProductionFlag = tempArray[15];
                processingFile.CreateUser = Environment.UserName;
                processingFile.CreateDate = DateTime.Today;
                _context.Files.Add(processingFile);
                _context.SaveChanges();
                string loopName = "";
                foreach (string s999Line in s999Lines)
                {
                    Parser999Line(s999Line, ref processingFile, ref transactions, ref errors, ref elements, ref loopName);
                }

                _context.Transactions.AddRange(transactions);
                _context.Errors.AddRange(errors);
                _context.Elements.AddRange(elements);
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
        private void Parser999Line(string line, ref _999File processingFile, ref List<_999Transaction> transactions,
            ref List<_999Error> errors, ref List<_999Element> elements, ref string loopName)
        {
            string[] segments = line.Split('*');
            switch (segments[0])
            {
                case "AK2":
                    loopName = "2000";
                    _999Transaction transaction = new _999Transaction
                    {
                        FileId = processingFile.FileId,
                        TransactionControlNumber = segments[2]
                    };
                    transactions.Add(transaction);
                    break;
                case "IK3":
                    loopName = "2100";
                    _999Error error = new _999Error
                    {
                        FileId = processingFile.FileId,
                        TransactionControlNumber = transactions.Last().TransactionControlNumber,
                        SegmentCode = segments[1],
                        PositionInTransaction = segments[2],
                        LoopCode = segments[3],
                        ErrorCode = segments[4]
                    };
                    errors.Add(error);
                    break;
                case "IK4":
                    loopName = "2110";
                    _999Element element = new _999Element
                    {
                        FileId = processingFile.FileId,
                        TransactionControlNumber = transactions.Last().TransactionControlNumber,
                        PositionInTransaction = errors.Last().PositionInTransaction,
                        PositionInSegment = segments[1],
                        ElementReferenceInSegment = segments[2],
                        ElementErrorCode = segments[3],
                        ElementBadDataCopy = segments.Length > 4 ? segments[4] : null
                    };
                    elements.Add(element);
                    break;
                case "CTX":
                    if (loopName == "2100")
                    {
                        if (segments[1] != "SITUATIONAL TRIGGER")
                        {
                            errors.Last().BusinessUnitName = segments[1].Split(':')[0];
                            errors.Last().BusinessUnitCode = segments[1].Split(':')[1];
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(errors.Last().CtxSegmentCode))
                            {
                                _999Error error2 = new _999Error
                                {
                                    FileId = processingFile.FileId,
                                    TransactionControlNumber = transactions.Last().TransactionControlNumber,
                                    SegmentCode = errors[errors.Count - 2].SegmentCode,
                                    PositionInTransaction = errors[errors.Count - 2].PositionInTransaction,
                                    LoopCode = errors[errors.Count - 2].LoopCode,
                                    ErrorCode = errors[errors.Count - 2].ErrorCode,
                                    CtxSegmentCode = segments[2],
                                    CtxPositionInTransaction = segments[3],
                                    CtxLoopCode = segments.Length > 4 ? segments[4] : null,
                                    CtxPositionInSegment = segments.Length > 5 ? segments[5] : null,
                                    CtxReferenceInSegment = segments.Length > 6 ? segments[6] : null
                                };
                                errors.Add(error2);
                            }
                            else
                            {
                                errors.Last().CtxSegmentCode = segments[2];
                                errors.Last().CtxPositionInTransaction = segments[3];
                                if (segments.Length > 4) errors.Last().CtxLoopCode = segments[4];
                                if (segments.Length > 5) errors.Last().CtxPositionInSegment = segments[5];
                                if (segments.Length > 6) errors.Last().CtxReferenceInSegment = segments[6];
                            }
                        }
                    }
                    else if (loopName == "2110")
                    {
                        if (!string.IsNullOrEmpty(elements.Last().ElementSegmentCode))
                        {
                            _999Element element2 = new _999Element
                            {
                                FileId = processingFile.FileId,
                                TransactionControlNumber = transactions.Last().TransactionControlNumber,
                                PositionInTransaction = errors.Last().PositionInTransaction,
                                PositionInSegment = elements.Last().PositionInSegment,
                                ElementReferenceInSegment = elements.Last().ElementReferenceInSegment,
                                ElementErrorCode = elements.Last().ElementErrorCode,
                                ElementBadDataCopy = elements.Last().ElementBadDataCopy,
                                ElementSegmentCode = segments[2],
                                ElementSegmentPositionInTransaction = segments[3],
                                ElementLoopCode = segments.Length > 4 ? segments[4] : null,
                                ElementPositionInSegment = segments.Length > 5 ? segments[5] : null,
                                ElementReferenceNumber = segments.Length > 6 ? segments[6] : null
                            };
                            elements.Add(element2);
                        }
                        else
                        {
                            elements.Last().ElementSegmentCode = segments[2];
                            elements.Last().ElementSegmentPositionInTransaction = segments[3];
                            if (segments.Length > 4) elements.Last().ElementLoopCode = segments[4];
                            if (segments.Length > 5) elements.Last().ElementPositionInSegment = segments[5];
                            if (segments.Length > 6) elements.Last().ElementReferenceInSegment = segments[6];
                        }
                    }

                    break;
                case "IK5":
                    transactions.Last().TransactionAckCode = segments[1];
                    if (segments.Length > 2) transactions.Last().TransactionError1 = segments[2];
                    if (segments.Length > 3) transactions.Last().TransactionError2 = segments[3];
                    if (segments.Length > 4) transactions.Last().TransactionError3 = segments[4];
                    if (segments.Length > 5) transactions.Last().TransactionError4 = segments[5];
                    if (segments.Length > 6) transactions.Last().TransactionError5 = segments[6];
                    break;
                case "AK9":
                    processingFile.FileAckCode = segments[1];
                    processingFile.TransactionsIncluded = segments[2];
                    processingFile.TransactionsReceived = segments[3];
                    processingFile.TransactionsAccepted = segments[4];
                    if (segments.Length > 5) processingFile.FileError1 = segments[5];
                    if (segments.Length > 6) processingFile.FileError2 = segments[6];
                    if (segments.Length > 7) processingFile.FileError3 = segments[7];
                    if (segments.Length > 8) processingFile.FileError4 = segments[8];
                    if (segments.Length > 9) processingFile.FileError5 = segments[9];
                    break;
            }
        }
    }
}
