using Eligibility834.Data;
using EncDataModel.M834;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Eligibility834.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Eligibility834Controller : ControllerBase
    {
        private readonly ILogger<Eligibility834Controller> _logger;
        private readonly Eligibility834Context _context;
        public Eligibility834Controller(ILogger<Eligibility834Controller> logger, Eligibility834Context context)
        {
            _logger = logger;
            _context = context;
        }
        //Eligibility834
        [HttpGet]
        public List<string> GetNewEligibility834Files()
        {
            _logger.Log(LogLevel.Information, "inquiry unprocessed files");
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string sourcePath = configuration["Source_Eligibility834"];
            string archivePath = configuration["Archive_Eligibility834"];
            List<string> result = Directory.EnumerateFiles(sourcePath).Select(x => Path.GetFileName(x)).ToList();
            result.Insert(0, archivePath);
            result.Insert(0, sourcePath);
            return result;
        }
        //Eligibility834/1
        [HttpGet("{id}")]
        public Tuple<int, int> ProcessEligibility834Files(long id)
        {
            _logger.Log(LogLevel.Information, "process new eligibility 834 files");
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string sourcePath = configuration["Source_Eligibility834"];
            string archivePath = configuration["Archive_Eligibility834"];
            DirectoryInfo di = new DirectoryInfo(sourcePath);
            FileInfo[] fis = di.GetFiles();
            int totalFiles = fis.Length;
            int goodFiles = 0;
            foreach (FileInfo fi in fis)
            {
                _logger.Log(LogLevel.Information, "Processing file " + fi.Name + " now...");
                M834File processingFile = _context.M834Files.FirstOrDefault(x => x.FileName == fi.Name);
                if (processingFile != null)
                {
                    _logger.Log(LogLevel.Information, fi.Name + " has been already processed");
                    MoveFile(archivePath, fi);
                    continue;
                }
                string s834 = System.IO.File.ReadAllText(fi.FullName).Replace("\n", "").Replace("\r", "");
                string[] s834Lines = s834.Split('~');
                s834 = null;
                int memberCount = s834Lines.Count(x => x.StartsWith("INS*"));
                if (memberCount < 1)
                {
                    _logger.Log(LogLevel.Information, "File " + fi.Name + " not valid");
                    MoveFile(archivePath, fi);
                    continue;
                }
                _logger.Log(LogLevel.Information, "Processing file " + fi.Name + " total records: " + memberCount.ToString());

                processingFile = new M834File();
                processingFile.FileName = fi.Name;

                string tempSeg = s834Lines[0];
                string[] tempArray = tempSeg.Split('*');
                processingFile.SenderId = tempArray[6].Trim();
                processingFile.ReceiverId = tempArray[8].Trim();
                processingFile.InterchangeControlNumber = tempArray[13];
                char elementDelimiter = (char)tempArray[16].ToCharArray()[0];
                processingFile.CreateUser = Environment.UserName;
                processingFile.CreateDate = DateTime.Now;
                tempSeg = s834Lines[1];
                tempArray = tempSeg.Split('*');
                processingFile.TransactionDate = tempArray[4];
                processingFile.TransactionTime = tempArray[5];
                tempSeg = s834Lines[3];
                tempArray = tempSeg.Split('*');
                processingFile.TransactionPurposeCode = tempArray[1];
                processingFile.TransactionReferenceNumber = tempArray[2];
                processingFile.TransactionTimeCode = tempArray[5];
                processingFile.TransactionActionCode = tempArray[8];
                tempSeg = s834Lines[4];
                tempArray = tempSeg.Split('*');

                _context.M834Files.Add(processingFile);
                _context.SaveChanges();
                string LoopNumber = "Header";
                Elig834 elig834 = new Elig834();
                elig834.m834file = processingFile;
                int lineNumber = 0;
                foreach (string s834Line in s834Lines)
                {
                    Process834Line(s834Line, elementDelimiter, ref elig834, ref LoopNumber, ref lineNumber);
                    lineNumber++;
                }
                Save834Batch(ref elig834);
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
        private void Save834Batch(ref Elig834 elig834)
        {
           
            _context.M834AdditionalNames.AddRange(elig834.m834additionalnames);
            _context.M834Details.AddRange(elig834.m834details);
            _context.M834DisabilityInfos.AddRange(elig834.m834disabilityinfos);
            _context.M834EmploymentClasses.AddRange(elig834.m834employmentclasses);
            _context.M834HCCOBInfos.AddRange(elig834.m834hccobinfos);
            _context.M834HCProviderInfos.AddRange(elig834.m834hcproviderinfos);
            _context.M834HealthCoverages.AddRange(elig834.m834healthcoverages);
            _context.M834Languages.AddRange(elig834.m834languages);
            _context.M834MemberLevelDates.AddRange(elig834.m834memberleveldates);
            _context.M834PolicyAnounts.AddRange(elig834.m834policyamounts);
            _context.M834ReportingCategories.AddRange(elig834.m834reportingcategories);
            _context.M834SubIds.AddRange(elig834.m834subids);
            _context.SaveChanges();
            elig834.m834additionalnames.Clear();
            elig834.m834details.Clear();
            elig834.m834disabilityinfos.Clear();
            elig834.m834employmentclasses.Clear();
            elig834.m834hccobinfos.Clear();
            elig834.m834hcproviderinfos.Clear();
            elig834.m834healthcoverages.Clear();
            elig834.m834languages.Clear();
            elig834.m834memberleveldates.Clear();
            elig834.m834policyamounts.Clear();
            elig834.m834reportingcategories.Clear();
            elig834.m834subids.Clear();
        }
        private void Process834Line(string s834Line, char elementDelimiter, ref Elig834 elig834, ref string LoopNumber, ref int lineNumber)
        {
            string[] segments = s834Line.Split('*');
            switch (segments[0])
            {
                case "REF":
                    if (LoopNumber == "Header")
                    {
                        elig834.m834file.TransactionPolicyNumber = segments[2];
                    }
                    else if (LoopNumber == "2000")
                    {
                        if (segments[1] == "0F") elig834.m834details.Last().SubscriberId = segments[2];
                        else if (segments[1] == "1L") elig834.m834details.Last().PolicyNumber = segments[2];
                        else
                        {
                            M834SubId sid = new M834SubId
                            {
                                FileId = elig834.m834file.FileId,
                                DetailId = elig834.m834details.Last().DetailId,
                                SubIdQualifier = segments[1],
                                SubId = segments[2]
                            };
                            elig834.m834subids.Add(sid);
                        }
                    }
                    else if (LoopNumber == "2300")
                    {
                        if (segments[1] == "QQ")
                        {
                            elig834.m834healthcoverages.Last().PriorCoverageMonthCount = segments[2];
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(elig834.m834healthcoverages.Last().PolicyNumber01))
                            {
                                elig834.m834healthcoverages.Last().PolicyQualifier01 = segments[1];
                                elig834.m834healthcoverages.Last().PolicyNumber01 = segments[2];
                            }
                            else if (string.IsNullOrEmpty(elig834.m834healthcoverages.Last().PolicyNumber02))
                            {
                                elig834.m834healthcoverages.Last().PolicyQualifier02 = segments[1];
                                elig834.m834healthcoverages.Last().PolicyNumber02 = segments[2];
                            }
                            else if (string.IsNullOrEmpty(elig834.m834healthcoverages.Last().PolicyNumber03))
                            {
                                elig834.m834healthcoverages.Last().PolicyQualifier03 = segments[1];
                                elig834.m834healthcoverages.Last().PolicyNumber03 = segments[2];
                            }
                            else if (string.IsNullOrEmpty(elig834.m834healthcoverages.Last().PolicyNumber04))
                            {
                                elig834.m834healthcoverages.Last().PolicyQualifier04 = segments[1];
                                elig834.m834healthcoverages.Last().PolicyNumber04 = segments[2];
                            }
                            else if (string.IsNullOrEmpty(elig834.m834healthcoverages.Last().PolicyNumber05))
                            {
                                elig834.m834healthcoverages.Last().PolicyQualifier05 = segments[1];
                                elig834.m834healthcoverages.Last().PolicyNumber05 = segments[2];
                            }
                            else if (string.IsNullOrEmpty(elig834.m834healthcoverages.Last().PolicyNumber06))
                            {
                                elig834.m834healthcoverages.Last().PolicyQualifier06 = segments[1];
                                elig834.m834healthcoverages.Last().PolicyNumber06 = segments[2];
                            }
                            else if (string.IsNullOrEmpty(elig834.m834healthcoverages.Last().PolicyNumber07))
                            {
                                elig834.m834healthcoverages.Last().PolicyQualifier07 = segments[1];
                                elig834.m834healthcoverages.Last().PolicyNumber07 = segments[2];
                            }
                            else if (string.IsNullOrEmpty(elig834.m834healthcoverages.Last().PolicyNumber08))
                            {
                                elig834.m834healthcoverages.Last().PolicyQualifier08 = segments[1];
                                elig834.m834healthcoverages.Last().PolicyNumber08 = segments[2];
                            }
                            else if (string.IsNullOrEmpty(elig834.m834healthcoverages.Last().PolicyNumber09))
                            {
                                elig834.m834healthcoverages.Last().PolicyQualifier09 = segments[1];
                                elig834.m834healthcoverages.Last().PolicyNumber09 = segments[2];
                            }
                            else if (string.IsNullOrEmpty(elig834.m834healthcoverages.Last().PolicyNumber10))
                            {
                                elig834.m834healthcoverages.Last().PolicyQualifier10 = segments[1];
                                elig834.m834healthcoverages.Last().PolicyNumber10 = segments[2];
                            }
                            else if (string.IsNullOrEmpty(elig834.m834healthcoverages.Last().PolicyNumber11))
                            {
                                elig834.m834healthcoverages.Last().PolicyQualifier11 = segments[1];
                                elig834.m834healthcoverages.Last().PolicyNumber11 = segments[2];
                            }
                            else if (string.IsNullOrEmpty(elig834.m834healthcoverages.Last().PolicyNumber12))
                            {
                                elig834.m834healthcoverages.Last().PolicyQualifier12 = segments[1];
                                elig834.m834healthcoverages.Last().PolicyNumber12 = segments[2];
                            }
                            else if (string.IsNullOrEmpty(elig834.m834healthcoverages.Last().PolicyNumber13))
                            {
                                elig834.m834healthcoverages.Last().PolicyQualifier13 = segments[1];
                                elig834.m834healthcoverages.Last().PolicyNumber13 = segments[2];
                            }
                            else if (string.IsNullOrEmpty(elig834.m834healthcoverages.Last().PolicyNumber14))
                            {
                                elig834.m834healthcoverages.Last().PolicyQualifier14 = segments[1];
                                elig834.m834healthcoverages.Last().PolicyNumber14 = segments[2];
                            }
                        }
                    }
                    else if (LoopNumber == "2320")
                    {
                        if (string.IsNullOrEmpty(elig834.m834hccobinfos.Last().CobQualifier1))
                        {
                            elig834.m834hccobinfos.Last().CobQualifier1 = segments[1];
                            elig834.m834hccobinfos.Last().CobPolicyNumber1 = segments[2];
                        }
                        else if (string.IsNullOrEmpty(elig834.m834hccobinfos.Last().CobQualifier2))
                        {
                            elig834.m834hccobinfos.Last().CobQualifier2 = segments[1];
                            elig834.m834hccobinfos.Last().CobPolicyNumber2 = segments[2];
                        }
                        else if (string.IsNullOrEmpty(elig834.m834hccobinfos.Last().CobQualifier3))
                        {
                            elig834.m834hccobinfos.Last().CobQualifier3 = segments[1];
                            elig834.m834hccobinfos.Last().CobPolicyNumber3 = segments[2];
                        }
                        else if (string.IsNullOrEmpty(elig834.m834hccobinfos.Last().CobQualifier4))
                        {
                            elig834.m834hccobinfos.Last().CobQualifier4 = segments[1];
                            elig834.m834hccobinfos.Last().CobPolicyNumber4 = segments[2];
                        }
                    }
                    else if (LoopNumber == "2700")
                    {
                        elig834.m834reportingcategories.Last().ReferenceIdQualifier = segments[1];
                        elig834.m834reportingcategories.Last().ReferenceId = segments[2];
                    }
                    break;
                case "DTP":
                    if (LoopNumber == "Header")
                    {
                        switch (segments[1])
                        {
                            case "007":
                                elig834.m834file.EffectiveDate = segments[3];
                                break;
                            case "090":
                                elig834.m834file.ReportStartDate = segments[3];
                                break;
                            case "091":
                                elig834.m834file.ReportEndDate = segments[3];
                                break;
                            case "303":
                                elig834.m834file.MaintenanceEffectiveDate = segments[3];
                                break;
                            case "382":
                                elig834.m834file.EnrollmentDate = segments[3];
                                break;
                            case "388":
                                elig834.m834file.PaymentDate = segments[3];
                                break;
                        }
                    }
                    else if (LoopNumber == "2000")
                    {
                        M834MemberLevelDate mld = new M834MemberLevelDate
                        {
                            FileId = elig834.m834file.FileId,
                            DetailId = elig834.m834details.Last().DetailId,
                            DateQualifier = segments[1],
                            MemberLevelDate = segments[2]
                        };
                        elig834.m834memberleveldates.Add(mld);
                    }
                    else if (LoopNumber == "2200")
                    {
                        if (segments[1] == "360") elig834.m834disabilityinfos.Last().DisabilityStartDate = segments[3];
                        else if (segments[1] == "361") elig834.m834disabilityinfos.Last().DisabilityEndDate = segments[3];
                    }
                    else if (LoopNumber == "2300")
                    {
                        if (string.IsNullOrEmpty(elig834.m834healthcoverages.Last().DateQualifier1))
                        {
                            elig834.m834healthcoverages.Last().DateQualifier1 = segments[1];
                            if (segments[2] == "D8") elig834.m834healthcoverages.Last().DateBegin1 = segments[3];
                            else
                            {
                                elig834.m834healthcoverages.Last().DateBegin1 = segments[3].Split(elementDelimiter)[0];
                                elig834.m834healthcoverages.Last().DateEnd1 = segments[3].Split(elementDelimiter)[1];
                            }
                        }
                        else if (string.IsNullOrEmpty(elig834.m834healthcoverages.Last().DateQualifier2))
                        {
                            elig834.m834healthcoverages.Last().DateQualifier2 = segments[1];
                            if (segments[2] == "D8") elig834.m834healthcoverages.Last().DateBegin2 = segments[3];
                            else
                            {
                                elig834.m834healthcoverages.Last().DateBegin2 = segments[3].Split(elementDelimiter)[0];
                                elig834.m834healthcoverages.Last().DateEnd2 = segments[3].Split(elementDelimiter)[1];
                            }
                        }
                        else if (string.IsNullOrEmpty(elig834.m834healthcoverages.Last().DateQualifier3))
                        {
                            elig834.m834healthcoverages.Last().DateQualifier3 = segments[1];
                            if (segments[2] == "D8") elig834.m834healthcoverages.Last().DateBegin3 = segments[3];
                            else
                            {
                                elig834.m834healthcoverages.Last().DateBegin3 = segments[3].Split(elementDelimiter)[0];
                                elig834.m834healthcoverages.Last().DateEnd3 = segments[3].Split(elementDelimiter)[1];
                            }
                        }
                        else if (string.IsNullOrEmpty(elig834.m834healthcoverages.Last().DateQualifier4))
                        {
                            elig834.m834healthcoverages.Last().DateQualifier4 = segments[1];
                            if (segments[2] == "D8") elig834.m834healthcoverages.Last().DateBegin4 = segments[3];
                            else
                            {
                                elig834.m834healthcoverages.Last().DateBegin4 = segments[3].Split(elementDelimiter)[0];
                                elig834.m834healthcoverages.Last().DateEnd4 = segments[3].Split(elementDelimiter)[1];
                            }
                        }
                        else if (string.IsNullOrEmpty(elig834.m834healthcoverages.Last().DateQualifier5))
                        {
                            elig834.m834healthcoverages.Last().DateQualifier5 = segments[1];
                            if (segments[2] == "D8") elig834.m834healthcoverages.Last().DateBegin5 = segments[3];
                            else
                            {
                                elig834.m834healthcoverages.Last().DateBegin5 = segments[3].Split(elementDelimiter)[0];
                                elig834.m834healthcoverages.Last().DateEnd5 = segments[3].Split(elementDelimiter)[1];
                            }
                        }
                        else if (string.IsNullOrEmpty(elig834.m834healthcoverages.Last().DateQualifier6))
                        {
                            elig834.m834healthcoverages.Last().DateQualifier6 = segments[1];
                            if (segments[2] == "D8") elig834.m834healthcoverages.Last().DateBegin6 = segments[3];
                            else
                            {
                                elig834.m834healthcoverages.Last().DateBegin6 = segments[3].Split(elementDelimiter)[0];
                                elig834.m834healthcoverages.Last().DateEnd6 = segments[3].Split(elementDelimiter)[1];
                            }
                        }
                    }
                    else if (LoopNumber == "2320")
                    {
                        if (segments[1] == "344") elig834.m834hccobinfos.Last().CobBeginDate = segments[3];
                        else if (segments[1] == "345") elig834.m834hccobinfos.Last().CobEndDate = segments[3];
                    }
                    else if (LoopNumber == "2700")
                    {
                        string[] temparray = segments[3].Split('-');
                        elig834.m834reportingcategories.Last().ReportBeginDate = temparray[0];
                        if (temparray.Length > 1) elig834.m834reportingcategories.Last().ReportEndDate = temparray[1];
                    }
                    break;
                case "QTY":
                    if (LoopNumber == "Header")
                    {
                        switch (segments[1])
                        {
                            case "DT":
                                elig834.m834file.DependentQuantity = segments[2];
                                break;
                            case "ET":
                                elig834.m834file.EmployeeQuantity = segments[2];
                                break;
                            case "TO":
                                elig834.m834file.TotalQuantity = segments[2];
                                break;
                        }
                    }
                    break;
                case "N1":
                    if (LoopNumber == "Header")
                    {
                        switch (segments[1])
                        {
                            case "P5":
                                elig834.m834file.SponsorName = segments[2];
                                elig834.m834file.SponsorIdQualifier = segments[3];
                                elig834.m834file.SponsorId = segments[4];
                                break;
                            case "IN":
                                elig834.m834file.PayerName = segments[2];
                                elig834.m834file.PayerIdQualifier = segments[3];
                                elig834.m834file.PayerId = segments[4];
                                break;
                            case "BO":
                            case "TV":
                                LoopNumber = "1000C";
                                if (string.IsNullOrEmpty(elig834.m834file.Broker1Id))
                                {
                                    elig834.m834file.Broker1Name = segments[2];
                                    elig834.m834file.Broker1IdQualifier = segments[3];
                                    elig834.m834file.Broker1Id = segments[4];
                                }
                                else
                                {
                                    elig834.m834file.Broker2Name = segments[2];
                                    elig834.m834file.Broker2IdQualifier = segments[3];
                                    elig834.m834file.Broker2Id = segments[4];
                                }
                                break;
                        }
                    }
                    else if (LoopNumber == "2700")
                    {
                        elig834.m834reportingcategories.Last().ParticipantName = segments[2];
                    }
                    break;
                case "ACT":
                    if (LoopNumber == "1000C")
                    {
                        if (string.IsNullOrEmpty(elig834.m834file.Broker1AccountNumber))
                        {
                            elig834.m834file.Broker1AccountNumber = segments[1];
                            if (segments.Length > 6) elig834.m834file.Broker1AccountNumber2 = segments[6];
                        }
                        else
                        {
                            elig834.m834file.Broker2AccountNumber = segments[1];
                            if (segments.Length > 6) elig834.m834file.Broker2AccountNumber2 = segments[6];
                        }
                    }
                    break;
                case "INS":
                    if (elig834.m834details.Count >= 1000)
                    {
                        Save834Batch(ref elig834);
                    }
                    LoopNumber = "2000";
                    M834Detail detail = new M834Detail
                    {
                        FileId = elig834.m834file.FileId,
                        ConditionCode = segments[1],
                        RelationshipCode = segments[2],
                        MaintenanceTypeCode = segments[3],
                        MaintenanceReasonCode = segments[4],
                        BenefitStatusCode = segments[5],
                    };
                    if (segments.Length > 6)
                    {
                        string[] temparray = segments[6].Split(elementDelimiter);
                        detail.MedicarePlanCode = temparray[0];
                        if (temparray.Length > 1) detail.MedicareEligibilityReasonCode = temparray[1];
                    }
                    if (segments.Length > 7) detail.COBRACode = segments[7];
                    if (segments.Length > 8) detail.EmploymentStatusCode = segments[8];
                    if (segments.Length > 9) detail.StudentStatusCode = segments[9];
                    if (segments.Length > 10) detail.HandicapInd = segments[10];
                    if (segments.Length > 12) detail.MemberDeathDate = segments[12];
                    if (segments.Length > 13) detail.ConfidentialityCode = segments[13];
                    if (segments.Length > 17) detail.BirthSequenceNumber = segments[17];
                    detail.DetailId = "INS" + DateTime.Now.ToString("yyMMddHHmmss") + lineNumber.ToString().PadLeft(7, '0');
                    elig834.m834details.Add(detail);
                    break;
                case "NM1":
                    if (LoopNumber == "2000" && segments[1] == "IL")
                    {
                        LoopNumber = "2100A";
                        elig834.m834details.Last().MemberLastName = segments[3];
                        if (segments.Length > 4) elig834.m834details.Last().MemberFirstName = segments[4];
                        if (segments.Length > 5) elig834.m834details.Last().MemberMiddleName = segments[5];
                        if (segments.Length > 6) elig834.m834details.Last().MemberPrefix = segments[6];
                        if (segments.Length > 7) elig834.m834details.Last().MemberSuffix = segments[7];
                        if (segments.Length > 8) elig834.m834details.Last().MemberIdQualifier = segments[8];
                        if (segments.Length > 9) elig834.m834details.Last().MemberId = segments[9];
                    }
                    else if (segments[1] == "70")
                    {
                        LoopNumber = "2100B";
                        M834AdditionalName an = new M834AdditionalName
                        {
                            FileId = elig834.m834file.FileId,
                            DetailId = elig834.m834details.Last().DetailId,
                            NameQualifier = segments[1],
                            LastName = segments[3],
                        };
                        if (segments.Length > 4) an.FirstName = segments[4];
                        if (segments.Length > 5) an.MiddleName = segments[5];
                        if (segments.Length > 6) an.Prefix = segments[6];
                        if (segments.Length > 7) an.Suffix = segments[7];
                        if (segments.Length > 8) an.IdQualifier = segments[8];
                        if (segments.Length > 9) an.NameId = segments[9];
                        elig834.m834additionalnames.Add(an);
                    }
                    else if (segments[1] == "31")
                    {
                        LoopNumber = "2100C";
                        M834AdditionalName an = new M834AdditionalName
                        {
                            FileId = elig834.m834file.FileId,
                            DetailId = elig834.m834details.Last().DetailId,
                            NameQualifier = segments[1]
                        };
                        elig834.m834additionalnames.Add(an);
                    }
                    else if (segments[1] == "36")
                    {
                        LoopNumber = "2100D";
                        M834AdditionalName an = new M834AdditionalName
                        {
                            FileId = elig834.m834file.FileId,
                            DetailId = elig834.m834details.Last().DetailId,
                            NameQualifier = segments[1]
                        };
                        if (segments.Length > 3) an.LastName = segments[3];
                        if (segments.Length > 4) an.FirstName = segments[4];
                        if (segments.Length > 5) an.MiddleName = segments[5];
                        if (segments.Length > 6) an.Prefix = segments[6];
                        if (segments.Length > 7) an.Suffix = segments[7];
                        if (segments.Length > 8) an.IdQualifier = segments[8];
                        if (segments.Length > 9) an.NameId = segments[9];
                        elig834.m834additionalnames.Add(an);
                    }
                    else if (segments[1] == "M8")
                    {
                        LoopNumber = "2100E";
                        M834AdditionalName an = new M834AdditionalName
                        {
                            FileId = elig834.m834file.FileId,
                            DetailId = elig834.m834details.Last().DetailId,
                            NameQualifier = segments[1],
                            LastName = segments[3]
                        };
                        elig834.m834additionalnames.Add(an);
                    }
                    else if (segments[1] == "S3")
                    {
                        LoopNumber = "2100F";
                        M834AdditionalName an = new M834AdditionalName
                        {
                            FileId = elig834.m834file.FileId,
                            DetailId = elig834.m834details.Last().DetailId,
                            NameQualifier = segments[1],
                            LastName = segments[3],
                            FirstName = segments[4]
                        };
                        if (segments.Length > 5) an.MiddleName = segments[5];
                        if (segments.Length > 6) an.Prefix = segments[6];
                        if (segments.Length > 7) an.Suffix = segments[7];
                        if (segments.Length > 8) an.IdQualifier = segments[8];
                        if (segments.Length > 9) an.NameId = segments[9];
                        elig834.m834additionalnames.Add(an);
                    }
                    else if ("6Y,9K,E1,EI,EXS,GB,GD,J6,LR,QD,S1,TZ,X4".Contains(segments[1]))
                    {
                        LoopNumber = "2100G";
                        M834AdditionalName an = new M834AdditionalName
                        {
                            FileId = elig834.m834file.FileId,
                            DetailId = elig834.m834details.Last().DetailId,
                            NameQualifier = segments[1],
                            LastName = segments[3]
                        };
                        if (segments.Length > 4) an.FirstName = segments[4];
                        if (segments.Length > 5) an.MiddleName = segments[5];
                        if (segments.Length > 6) an.Prefix = segments[6];
                        if (segments.Length > 7) an.Suffix = segments[7];
                        if (segments.Length > 8) an.IdQualifier = segments[8];
                        if (segments.Length > 9) an.NameId = segments[9];
                        elig834.m834additionalnames.Add(an);
                    }
                    else if (segments[1] == "45")
                    {
                        LoopNumber = "2100H";
                        M834AdditionalName an = new M834AdditionalName
                        {
                            FileId = elig834.m834file.FileId,
                            DetailId = elig834.m834details.Last().DetailId,
                            NameQualifier = segments[1]
                        };
                        if (segments.Length > 3) an.LastName = segments[3];
                        if (segments.Length > 4) an.FirstName = segments[4];
                        if (segments.Length > 5) an.MiddleName = segments[5];
                        if (segments.Length > 6) an.Prefix = segments[6];
                        if (segments.Length > 7) an.Suffix = segments[7];
                        if (segments.Length > 8) an.IdQualifier = segments[8];
                        if (segments.Length > 9) an.NameId = segments[9];
                        elig834.m834additionalnames.Add(an);
                    }
                    else if (LoopNumber == "2310")
                    {
                        elig834.m834hcproviderinfos.Last().ProviderQualifier = segments[1];
                        elig834.m834hcproviderinfos.Last().ProviderLastName = segments[3];
                        elig834.m834hcproviderinfos.Last().ProviderFirstName = segments[4];
                        elig834.m834hcproviderinfos.Last().ProviderMiddleName = segments[5];
                        elig834.m834hcproviderinfos.Last().ProviderPrefix = segments[6];
                        elig834.m834hcproviderinfos.Last().ProviderSuffix = segments[7];
                        elig834.m834hcproviderinfos.Last().ProviderIdQualifier = segments[8];
                        elig834.m834hcproviderinfos.Last().ProviderId = segments[9];
                        elig834.m834hcproviderinfos.Last().EntityRelationshipCode = segments[10];
                    }
                    else if (LoopNumber == "2320")
                    {
                        LoopNumber = "2330";
                        if (string.IsNullOrEmpty(elig834.m834hccobinfos.Last().Entity1Qualifier))
                        {
                            elig834.m834hccobinfos.Last().Entity1Qualifier = segments[1];
                            if (segments.Length > 3) elig834.m834hccobinfos.Last().Entity1LastName = segments[3];
                            if (segments.Length > 8) elig834.m834hccobinfos.Last().Entity1IdQualifier = segments[8];
                            if (segments.Length > 9) elig834.m834hccobinfos.Last().Entity1Id = segments[9];
                        }
                        else if (string.IsNullOrEmpty(elig834.m834hccobinfos.Last().Entity2Qualifier))
                        {
                            elig834.m834hccobinfos.Last().Entity2Qualifier = segments[1];
                            if (segments.Length > 3) elig834.m834hccobinfos.Last().Entity2LastName = segments[3];
                            if (segments.Length > 8) elig834.m834hccobinfos.Last().Entity2IdQualifier = segments[8];
                            if (segments.Length > 9) elig834.m834hccobinfos.Last().Entity2Id = segments[9];
                        }
                        else if (string.IsNullOrEmpty(elig834.m834hccobinfos.Last().Entity3Qualifier))
                        {
                            elig834.m834hccobinfos.Last().Entity3Qualifier = segments[1];
                            if (segments.Length > 3) elig834.m834hccobinfos.Last().Entity3LastName = segments[3];
                            if (segments.Length > 8) elig834.m834hccobinfos.Last().Entity3IdQualifier = segments[8];
                            if (segments.Length > 9) elig834.m834hccobinfos.Last().Entity3Id = segments[9];
                        }
                    }
                    break;
                case "PER":
                    switch (LoopNumber)
                    {
                        case "2100A":
                            elig834.m834details.Last().CommunicationQualifier1 = segments[3];
                            elig834.m834details.Last().CommunicationNumber1 = segments[4];
                            if (segments.Length > 6)
                            {
                                elig834.m834details.Last().CommunicationQualifier2 = segments[5];
                                elig834.m834details.Last().CommunicationNumber2 = segments[6];
                            }
                            if (segments.Length > 8)
                            {
                                elig834.m834details.Last().CommunicationQualifier3 = segments[7];
                                elig834.m834details.Last().CommunicationNumber3 = segments[8];
                            }
                            break;
                        case "2100D":
                        case "2100E":
                        case "2100F":
                        case "2100G":
                            elig834.m834additionalnames.Last().ContactQualifier1 = segments[3];
                            elig834.m834additionalnames.Last().ContactNumber1 = segments[4];
                            if (segments.Length > 6) { elig834.m834additionalnames.Last().ContactQualifier2 = segments[5]; elig834.m834additionalnames.Last().ContactNumber2 = segments[6]; }
                            if (segments.Length > 8) { elig834.m834additionalnames.Last().ContactQualifier3 = segments[7]; elig834.m834additionalnames.Last().ContactNumber3 = segments[8]; }
                            break;
                        case "2310":
                            if (string.IsNullOrEmpty(elig834.m834hcproviderinfos.Last().ProviderContact1Number1))
                            {
                                elig834.m834hcproviderinfos.Last().ProviderContact1Qualifier1 = segments[3];
                                elig834.m834hcproviderinfos.Last().ProviderContact1Number1 = segments[4];
                                if (segments.Length > 6) { elig834.m834hcproviderinfos.Last().ProviderContact1Qualifier2 = segments[5]; elig834.m834hcproviderinfos.Last().ProviderContact1Number2 = segments[6]; }
                                if (segments.Length > 8) { elig834.m834hcproviderinfos.Last().ProviderContact1Qualifier3 = segments[7]; elig834.m834hcproviderinfos.Last().ProviderContact1Number3 = segments[8]; }
                            }
                            else if (string.IsNullOrEmpty(elig834.m834hcproviderinfos.Last().ProviderContact2Number1))
                            {
                                elig834.m834hcproviderinfos.Last().ProviderContact2Qualifier1 = segments[3];
                                elig834.m834hcproviderinfos.Last().ProviderContact2Number1 = segments[4];
                                if (segments.Length > 6) { elig834.m834hcproviderinfos.Last().ProviderContact2Qualifier2 = segments[5]; elig834.m834hcproviderinfos.Last().ProviderContact2Number2 = segments[6]; }
                                if (segments.Length > 8) { elig834.m834hcproviderinfos.Last().ProviderContact2Qualifier3 = segments[7]; elig834.m834hcproviderinfos.Last().ProviderContact2Number3 = segments[8]; }
                            }
                            break;
                        case "2330":
                            if (string.IsNullOrEmpty(elig834.m834hccobinfos.Last().Entity1ContactQualifier))
                            {
                                elig834.m834hccobinfos.Last().Entity1ContactQualifier = segments[3];
                                elig834.m834hccobinfos.Last().Entity1ContactNumber = segments[4];
                            }
                            else if (string.IsNullOrEmpty(elig834.m834hccobinfos.Last().Entity2ContactQualifier))
                            {
                                elig834.m834hccobinfos.Last().Entity2ContactQualifier = segments[3];
                                elig834.m834hccobinfos.Last().Entity2ContactNumber = segments[4];
                            }
                            else if (string.IsNullOrEmpty(elig834.m834hccobinfos.Last().Entity3ContactQualifier))
                            {
                                elig834.m834hccobinfos.Last().Entity3ContactQualifier = segments[3];
                                elig834.m834hccobinfos.Last().Entity3ContactNumber = segments[4];
                            }
                            break;
                    }
                    break;
                case "N3":
                    switch (LoopNumber)
                    {
                        case "2100A":
                            elig834.m834details.Last().MemberAddress = segments[1];
                            if (segments.Length > 2) elig834.m834details.Last().MemberAddress2 = segments[2];
                            break;
                        case "2100C":
                        case "2100D":
                        case "2100E":
                        case "2100F":
                        case "2100G":
                        case "2100H":
                            elig834.m834additionalnames.Last().Address = segments[1];
                            if (segments.Length > 2) elig834.m834additionalnames.Last().Address2 = segments[2];
                            break;
                        case "2310":
                            if (string.IsNullOrEmpty(elig834.m834hcproviderinfos.Last().ProviderAddress1))
                            {
                                elig834.m834hcproviderinfos.Last().ProviderAddress1 = segments[1];
                                if (segments.Length > 2) elig834.m834hcproviderinfos.Last().ProviderAddress12 = segments[2];
                            }
                            else if (string.IsNullOrEmpty(elig834.m834hcproviderinfos.Last().ProviderAddress2))
                            {
                                elig834.m834hcproviderinfos.Last().ProviderAddress2 = segments[1];
                                if (segments.Length > 2) elig834.m834hcproviderinfos.Last().ProviderAddress22 = segments[2];
                            }
                            break;
                        case "2330":
                            if (string.IsNullOrEmpty(elig834.m834hccobinfos.Last().Entity1Address))
                            {
                                elig834.m834hccobinfos.Last().Entity1Address = segments[1];
                                if (segments.Length > 2) elig834.m834hccobinfos.Last().Entity1Address2 = segments[2];
                            }
                            else if (string.IsNullOrEmpty(elig834.m834hccobinfos.Last().Entity2Address))
                            {
                                elig834.m834hccobinfos.Last().Entity2Address = segments[1];
                                if (segments.Length > 2) elig834.m834hccobinfos.Last().Entity2Address2 = segments[2];
                            }
                            else if (string.IsNullOrEmpty(elig834.m834hccobinfos.Last().Entity3Address))
                            {
                                elig834.m834hccobinfos.Last().Entity3Address = segments[1];
                                if (segments.Length > 2) elig834.m834hccobinfos.Last().Entity3Address2 = segments[2];
                            }
                            break;
                    }
                    break;
                case "N4":
                    switch (LoopNumber)
                    {
                        case "2100A":
                            elig834.m834details.Last().MemberCity = segments[1];
                            if (segments.Length > 2) elig834.m834details.Last().MemberState = segments[2];
                            if (segments.Length > 3) elig834.m834details.Last().MemberZip = segments[3];
                            if (segments.Length > 4) elig834.m834details.Last().MemberCountry = segments[4];
                            if (segments.Length > 5) elig834.m834details.Last().MemberLocationQualifier = segments[5];
                            if (segments.Length > 6) elig834.m834details.Last().MemberLocationId = segments[6];
                            if (segments.Length > 7) elig834.m834details.Last().MemberCountrySubCode = segments[7];
                            break;
                        case "2100C":
                        case "2100D":
                        case "2100E":
                        case "2100F":
                        case "2100G":
                        case "2100H":
                            elig834.m834additionalnames.Last().AddressCity = segments[1];
                            if (segments.Length > 2) elig834.m834additionalnames.Last().AddressSTate = segments[2];
                            if (segments.Length > 3) elig834.m834additionalnames.Last().AddressZip = segments[3];
                            if (segments.Length > 4) elig834.m834additionalnames.Last().AddressCountry = segments[4];
                            if (segments.Length > 7) elig834.m834additionalnames.Last().AddressCountrySubCode = segments[7];
                            break;
                        case "2310":
                            elig834.m834hcproviderinfos.Last().ProviderAddressCity = segments[1];
                            if (segments.Length > 2) elig834.m834hcproviderinfos.Last().ProviderAddressState = segments[2];
                            if (segments.Length > 3) elig834.m834hcproviderinfos.Last().ProviderAddressZip = segments[3];
                            if (segments.Length > 4) elig834.m834hcproviderinfos.Last().ProviderAddressCountry = segments[4];
                            if (segments.Length > 7) elig834.m834hcproviderinfos.Last().ProviderAddressCountrySubCode = segments[7];
                            break;
                        case "2330":
                            if (string.IsNullOrEmpty(elig834.m834hccobinfos.Last().Entity1AddressCity))
                            {
                                elig834.m834hccobinfos.Last().Entity1AddressCity = segments[1];
                                if (segments.Length > 2) elig834.m834hccobinfos.Last().Entity1AddressState = segments[2];
                                if (segments.Length > 3) elig834.m834hccobinfos.Last().Entity1AddressZip = segments[3];
                                if (segments.Length > 4) elig834.m834hccobinfos.Last().Entity1AddressCountry = segments[4];
                                if (segments.Length > 7) elig834.m834hccobinfos.Last().Entity1AddressCountrySubCode = segments[7];
                            }
                            else if (string.IsNullOrEmpty(elig834.m834hccobinfos.Last().Entity2AddressCity))
                            {
                                elig834.m834hccobinfos.Last().Entity2AddressCity = segments[1];
                                if (segments.Length > 2) elig834.m834hccobinfos.Last().Entity2AddressState = segments[2];
                                if (segments.Length > 3) elig834.m834hccobinfos.Last().Entity2AddressZip = segments[3];
                                if (segments.Length > 4) elig834.m834hccobinfos.Last().Entity2AddressCountry = segments[4];
                                if (segments.Length > 7) elig834.m834hccobinfos.Last().Entity2AddressCountrySubCode = segments[7];
                            }
                            else if (string.IsNullOrEmpty(elig834.m834hccobinfos.Last().Entity3AddressCity))
                            {
                                elig834.m834hccobinfos.Last().Entity3AddressCity = segments[1];
                                if (segments.Length > 2) elig834.m834hccobinfos.Last().Entity3AddressState = segments[2];
                                if (segments.Length > 3) elig834.m834hccobinfos.Last().Entity3AddressZip = segments[3];
                                if (segments.Length > 4) elig834.m834hccobinfos.Last().Entity3AddressCountry = segments[4];
                                if (segments.Length > 7) elig834.m834hccobinfos.Last().Entity3AddressCountrySubCode = segments[7];
                            }
                            break;
                    }
                    break;
                case "DMG":
                    if (LoopNumber == "2100A")
                    {
                        elig834.m834details.Last().MemberBirthDate = segments[2];
                        elig834.m834details.Last().MemberGender = segments[3];
                        if (segments.Length > 4) elig834.m834details.Last().MemberMaritalStatus = segments[4];
                        if (segments.Length > 5)
                        {
                            string[] temparray = segments[5].Split(elementDelimiter);
                            elig834.m834details.Last().MemberEthnicity = temparray[0];
                            if (temparray.Length > 2) elig834.m834details.Last().MemberEthnicityClassificationCode = temparray[2];
                        }
                        if (segments.Length > 6) elig834.m834details.Last().MemberCitizenship = segments[6];
                        if (segments.Length > 11) elig834.m834details.Last().MemberEthnicityCollectionCode = segments[11];
                    }
                    else if (LoopNumber == "2100B")
                    {
                        if (segments.Length > 2) elig834.m834additionalnames.Last().BirthDate = segments[2];
                        if (segments.Length > 3) elig834.m834additionalnames.Last().Gender = segments[3];
                        if (segments.Length > 4) elig834.m834additionalnames.Last().MaritalStatus = segments[4];
                        if (segments.Length > 5)
                        {
                            string[] temparray = segments[5].Split(elementDelimiter);
                            elig834.m834additionalnames.Last().Ethnicity = temparray[0];
                            if (temparray.Length > 2) elig834.m834additionalnames.Last().EthnicityClassificationCode = temparray[2];
                        }
                        if (segments.Length > 6) elig834.m834additionalnames.Last().Citizenship = segments[6];
                        if (segments.Length > 11) elig834.m834additionalnames.Last().EthnicityCollectionCode = segments[11];
                    }
                    break;
                case "EC":
                    if (LoopNumber == "2100A")
                    {
                        M834EmploymentClass ec = new M834EmploymentClass
                        {
                            FileId = elig834.m834file.FileId,
                            DetailId = elig834.m834details.Last().DetailId,
                            EmploymentClassCode1 = segments[1]
                        };
                        if (segments.Length > 2) ec.EmploymentClassCode2 = segments[2];
                        if (segments.Length > 3) ec.EmploymentClassCode3 = segments[3];
                        elig834.m834employmentclasses.Add(ec);
                    }
                    break;
                case "ICM":
                    if (LoopNumber == "2100A")
                    {
                        elig834.m834details.Last().MemberIncomeFrequencyCode = segments[1];
                        elig834.m834details.Last().MemberIncome = segments[2];
                        if (segments.Length > 3) elig834.m834details.Last().MemberWorkHours = segments[3];
                        if (segments.Length > 4) elig834.m834details.Last().MemberWorkDepartment = segments[4];
                        if (segments.Length > 5) elig834.m834details.Last().MemberSalaryGrade = segments[5];
                    }
                    break;
                case "AMT":
                    if (LoopNumber == "2100A")
                    {
                        M834PolicyAmount pa = new M834PolicyAmount
                        {
                            FileId = elig834.m834file.FileId,
                            DetailId = elig834.m834details.Last().DetailId,
                            AmountQualifier = segments[1],
                            ContractAmount = segments[2]
                        };
                        elig834.m834policyamounts.Add(pa);
                    }
                    else if (LoopNumber == "2300")
                    {
                        if (string.IsNullOrEmpty(elig834.m834healthcoverages.Last().AmountQualifier1))
                        {
                            elig834.m834healthcoverages.Last().AmountQualifier1 = segments[1];
                            elig834.m834healthcoverages.Last().Amount1 = segments[2];
                        }
                        else if (string.IsNullOrEmpty(elig834.m834healthcoverages.Last().AmountQualifier2))
                        {
                            elig834.m834healthcoverages.Last().AmountQualifier2 = segments[1];
                            elig834.m834healthcoverages.Last().Amount2 = segments[2];
                        }
                        else if (string.IsNullOrEmpty(elig834.m834healthcoverages.Last().AmountQualifier3))
                        {
                            elig834.m834healthcoverages.Last().AmountQualifier3 = segments[1];
                            elig834.m834healthcoverages.Last().Amount3 = segments[2];
                        }
                        else if (string.IsNullOrEmpty(elig834.m834healthcoverages.Last().AmountQualifier4))
                        {
                            elig834.m834healthcoverages.Last().AmountQualifier4 = segments[1];
                            elig834.m834healthcoverages.Last().Amount4 = segments[2];
                        }
                        else if (string.IsNullOrEmpty(elig834.m834healthcoverages.Last().AmountQualifier5))
                        {
                            elig834.m834healthcoverages.Last().AmountQualifier5 = segments[1];
                            elig834.m834healthcoverages.Last().Amount5 = segments[2];
                        }
                        else if (string.IsNullOrEmpty(elig834.m834healthcoverages.Last().AmountQualifier6))
                        {
                            elig834.m834healthcoverages.Last().AmountQualifier6 = segments[1];
                            elig834.m834healthcoverages.Last().Amount6 = segments[2];
                        }
                        else if (string.IsNullOrEmpty(elig834.m834healthcoverages.Last().AmountQualifier7))
                        {
                            elig834.m834healthcoverages.Last().AmountQualifier7 = segments[1];
                            elig834.m834healthcoverages.Last().Amount7 = segments[2];
                        }
                        else if (string.IsNullOrEmpty(elig834.m834healthcoverages.Last().AmountQualifier8))
                        {
                            elig834.m834healthcoverages.Last().AmountQualifier8 = segments[1];
                            elig834.m834healthcoverages.Last().Amount8 = segments[2];
                        }
                        else if (string.IsNullOrEmpty(elig834.m834healthcoverages.Last().AmountQualifier9))
                        {
                            elig834.m834healthcoverages.Last().AmountQualifier9 = segments[1];
                            elig834.m834healthcoverages.Last().Amount9 = segments[2];
                        }
                    }
                    break;
                case "HLH":
                    if (LoopNumber == "2100A")
                    {
                        elig834.m834details.Last().MemberHealthCode = segments[1];
                        if (segments.Length > 2) elig834.m834details.Last().MemberHeight = segments[2];
                        if (segments.Length > 3) elig834.m834details.Last().MemberWeight = segments[3];
                    }
                    break;
                case "LUI":
                    if (LoopNumber == "2100A")
                    {
                        M834Language language = new M834Language
                        {
                            FileId = elig834.m834file.FileId,
                            DetailId = elig834.m834details.Last().DetailId,
                        };
                        if (segments.Length > 1) language.IdQualifier = segments[1];
                        if (segments.Length > 2) language.LanguageId = segments[2];
                        if (segments.Length > 3) language.LanguageDescription = segments[4];
                        if (segments.Length > 5) language.LanguageUsageInd = segments[5];
                        elig834.m834languages.Add(language);
                    }
                    break;
                case "DSB":
                    LoopNumber = "2200";
                    M834DisabilityInfo di = new M834DisabilityInfo
                    {
                        FileId = elig834.m834file.FileId,
                        DetailId = elig834.m834details.Last().DetailId,
                        DisabilityTypeCode = segments[1]
                    };
                    if (segments.Length > 7) di.ProductIdQualifier = segments[7];
                    if (segments.Length > 8) di.DiagnosisCode = segments[8];
                    elig834.m834disabilityinfos.Add(di);
                    break;
                case "HD":
                    LoopNumber = "2300";
                    M834HealthCoverage hc = new M834HealthCoverage
                    {
                        FileId = elig834.m834file.FileId,
                        DetailId = elig834.m834details.Last().DetailId,
                        MaintenanceTypeCode = segments[1],
                        InsuranceLineCode = segments[3]
                    };
                    if (segments.Length > 4) hc.PlanCoverageDescription = segments[4];
                    if (segments.Length > 5) hc.CoverageLevelCode = segments[5];
                    if (segments.Length > 9) hc.LateEnrollmentInd = segments[9];
                    hc.HCId = "HD" + DateTime.Now.ToString("yyMMddHHmmss") + lineNumber.ToString().PadLeft(7, '0');
                    elig834.m834healthcoverages.Add(hc);
                    break;
                case "IDC":
                    if (string.IsNullOrEmpty(elig834.m834healthcoverages.Last().IdCardTypeCode1))
                    {
                        elig834.m834healthcoverages.Last().PlanCoverageDescription1 = segments[1];
                        elig834.m834healthcoverages.Last().IdCardTypeCode1 = segments[2];
                        if (segments.Length > 3) elig834.m834healthcoverages.Last().IdCardCount1 = segments[3];
                        if (segments.Length > 4) elig834.m834healthcoverages.Last().ActionCode1 = segments[4];
                    }
                    else if (string.IsNullOrEmpty(elig834.m834healthcoverages.Last().IdCardTypeCode2))
                    {
                        elig834.m834healthcoverages.Last().PlanCoverageDescription2 = segments[1];
                        elig834.m834healthcoverages.Last().IdCardTypeCode2 = segments[2];
                        if (segments.Length > 3) elig834.m834healthcoverages.Last().IdCardCount2 = segments[3];
                        if (segments.Length > 4) elig834.m834healthcoverages.Last().ActionCode2 = segments[4];
                    }
                    else if (string.IsNullOrEmpty(elig834.m834healthcoverages.Last().IdCardTypeCode3))
                    {
                        elig834.m834healthcoverages.Last().PlanCoverageDescription3 = segments[1];
                        elig834.m834healthcoverages.Last().IdCardTypeCode3 = segments[2];
                        if (segments.Length > 3) elig834.m834healthcoverages.Last().IdCardCount3 = segments[3];
                        if (segments.Length > 4) elig834.m834healthcoverages.Last().ActionCode3 = segments[4];
                    }
                    break;
                case "LX":
                    if (LoopNumber == "2300" || LoopNumber == "2310")
                    {
                        LoopNumber = "2310";
                        M834HCProviderInfo pi = new M834HCProviderInfo
                        {
                            FileId = elig834.m834file.FileId,
                            DetailId = elig834.m834details.Last().DetailId,
                            HCId = elig834.m834healthcoverages.Last().HCId
                        };
                        elig834.m834hcproviderinfos.Add(pi);
                    }
                    else if (LoopNumber == "2700")
                    {
                        LoopNumber = "2700";
                        M834ReportingCategory rc = new M834ReportingCategory
                        {
                            FileId = elig834.m834file.FileId,
                            DetailId = elig834.m834details.Last().DetailId,
                            SequenceNumber = segments[1]
                        };
                        elig834.m834reportingcategories.Add(rc);
                    }
                    break;
                case "PLA":
                    if (LoopNumber == "2310")
                    {
                        elig834.m834hcproviderinfos.Last().ProviderChangeCode = segments[1];
                        elig834.m834hcproviderinfos.Last().ProviderChangeQualifier = segments[2];
                        elig834.m834hcproviderinfos.Last().ProviderChangeDate = segments[3];
                        elig834.m834hcproviderinfos.Last().ProviderChangeReasonCode = segments[5];
                    }
                    break;
                case "COB":
                    LoopNumber = "2320";
                    M834HCCOBInfo cob = new M834HCCOBInfo
                    {
                        FileId = elig834.m834file.FileId,
                        DetailId = elig834.m834details.Last().DetailId,
                        HCId = elig834.m834healthcoverages.Last().HCId,
                        SequenceCode = segments[1],
                        MemberGroupNumber = segments[2],
                        CobCode = segments[3],
                    };
                    if (segments.Length > 4) cob.ServiceTypeCode = segments[4];
                    elig834.m834hccobinfos.Add(cob);
                    break;
                case "LS":
                    LoopNumber = "2700";
                    break;
            }
        }
    }
}
