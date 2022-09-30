using EncDataModel.Provider274;
using Load274Data.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Load274Data.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Load274DataController : ControllerBase
    {
        private readonly ILogger<Load274DataController> _logger;
        private readonly P274Context _context;
        public Load274DataController(ILogger<Load274DataController> logger, P274Context context)
        {
            _logger = logger;
            _context = context;
        }
        //Load274Data
        [HttpGet]
        public List<string> GetNew274DataFiles()
        {
            _logger.Log(LogLevel.Information, "inquiry unprocessed files");
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string sourcePath = configuration["Source_Provider274"];
            string archivePath = configuration["Archive_Provider274"];
            List<string> result = Directory.EnumerateFiles(sourcePath).Select(x => Path.GetFileName(x)).ToList();
            result.Insert(0, archivePath);
            result.Insert(0, sourcePath);
            result.Insert(0, "0");
            return result;
        }
        //Load274Data/1
        [HttpGet("{id}")]
        public List<string> Process274DataFiles(long id)
        {
            List<string> result = new List<string>();
            _logger.Log(LogLevel.Information, "process new provider 274 files");
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string sourcePath = configuration["Source_Provider274"];
            string archivePath = configuration["Archive_Provider274"];
            DirectoryInfo di = new DirectoryInfo(sourcePath);
            FileInfo[] fis = di.GetFiles();
            int totalFiles = fis.Length;
            int goodFiles = 0;
            foreach (FileInfo fi in fis)
            {
                _logger.Log(LogLevel.Information, "Processing file " + fi.Name + " now...");
                P274File processingFile = _context.Files.FirstOrDefault(x => x.FileName == fi.Name);
                if (processingFile != null)
                {
                    _logger.Log(LogLevel.Information, fi.Name + " has been already processed");
                    MoveFile(archivePath, fi);
                    continue;
                }
                string s274 = System.IO.File.ReadAllText(fi.FullName).Replace("\r", "").Replace("\n", "");
                string[] s274Lines = s274.Split('~');
                s274 = null;
                processingFile = new P274File();
                processingFile.FileName = fi.Name;
                string[] tempSegs = s274Lines[0].Split('*');
                processingFile.SenderId = tempSegs[6];
                processingFile.ReceiverId = tempSegs[8];
                processingFile.InterchangeControlNumber = tempSegs[13];
                tempSegs = s274Lines[1].Split('*');
                processingFile.TransactionSenderIdentifier = tempSegs[3];
                processingFile.TransactionDate = tempSegs[4];
                processingFile.TransactionTime = tempSegs[5];
                processingFile.AddedDate = DateTime.Now;
                _context.Files.Add(processingFile);
                _context.SaveChanges();
                string loopName = "Header";
                Provider274 p274 = new Provider274();
                p274.p274file = processingFile;
                int lineNumber = 1;
                string groupId = "";
                string siteId = "";
                foreach (string line in s274Lines)
                {
                    Process274Line(line, ref loopName, ref p274, ref lineNumber, ref groupId, ref siteId);
                    lineNumber++;
                }
                _context.Entities.AddRange(p274.p274affiliatedentities);
                _context.Details.AddRange(p274.p274details);
                _context.Groups.AddRange(p274.p274groupidnumbers);
                _context.Infos.AddRange(p274.p274informations);
                _context.Crcs.AddRange(p274.p274sitecrcs);
                _context.Areas.AddRange(p274.p274specializationareas);
                _context.SaveChanges();
                MoveFile(archivePath, fi);
                goodFiles++;
            }
            result.Add(totalFiles.ToString());
            result.Add(goodFiles.ToString());
            return result;
        }


        private void MoveFile(string archivePath, FileInfo fi)
        {
            if (System.IO.File.Exists(Path.Combine(archivePath, fi.Name))) System.IO.File.Delete(Path.Combine(archivePath, fi.Name));
            fi.MoveTo(Path.Combine(archivePath, fi.Name));
        }
        private void Process274Line(string line, ref string loopName, ref Provider274 p274, ref int lineNumber, ref string groupId, ref string siteId)
        {
            string[] segments = line.Split('*');
            switch (segments[0])
            {
                case "DTM":
                    if (loopName == "Header")
                    {
                        p274.p274file.DirectoryExtractDate = segments[2];
                    }
                    break;
                case "PER":
                    if (loopName == "Header")
                    {
                        p274.p274file.ContactFunctionCode = segments[1];
                        p274.p274file.ContactName = segments[2];
                        p274.p274file.ContactQualifier1 = segments[3];
                        p274.p274file.ContactNumber1 = segments[4];
                        if (segments.Length > 6)
                        {
                            p274.p274file.ContactQualifier2 = segments[5];
                            p274.p274file.ContactNumber2 = segments[6];
                        }
                        if (segments.Length > 8)
                        {
                            p274.p274file.ContactQualifier3 = segments[7];
                            p274.p274file.ContactNumber3 = segments[8];
                        }
                    }
                    else if (loopName == "2000D")
                    {
                        if (string.IsNullOrEmpty(p274.p274details.Last().SiteContact1FunctionCode))
                        {
                            p274.p274details.Last().SiteContact1FunctionCode = segments[1];
                            p274.p274details.Last().SiteContact1Qualifier1 = segments[3];
                            p274.p274details.Last().SiteContact1Number1 = segments[4];
                            if (segments.Length > 6)
                            {
                                p274.p274details.Last().SiteContact1Qualifier2 = segments[5];
                                p274.p274details.Last().SiteContact1Number2 = segments[6];
                            }
                            if (segments.Length > 8)
                            {
                                p274.p274details.Last().SiteContact1Qualifier3 = segments[7];
                                p274.p274details.Last().SiteContact1Number3 = segments[8];
                            }
                        }
                        else if (string.IsNullOrEmpty(p274.p274details.Last().SiteContact2FunctionCode))
                        {
                            p274.p274details.Last().SiteContact2FunctionCode = segments[1];
                            p274.p274details.Last().SiteContact2Qualifier1 = segments[3];
                            p274.p274details.Last().SiteContact2Number1 = segments[4];
                            if (segments.Length > 6)
                            {
                                p274.p274details.Last().SiteContact2Qualifier2 = segments[5];
                                p274.p274details.Last().SiteContact2Number2 = segments[6];
                            }
                            if (segments.Length > 8)
                            {
                                p274.p274details.Last().SiteContact2Qualifier3 = segments[7];
                                p274.p274details.Last().SiteContact2Number3 = segments[8];
                            }
                        }
                    }
                    break;
                case "HL":
                    if (segments[3] == "20") loopName = "2000A";
                    else if (segments[3] == "21") loopName = "2000B";
                    else if (segments[3] == "19") loopName = "2000C";
                    else if (segments[3] == "N") loopName = "2000D";
                    else if (segments[3] == "4") loopName = "2000E";
                    break;
                case "NM1":
                    if (loopName == "2000A" || loopName == "2000B")
                    {
                        P274Information info = new P274Information
                        {
                            FileId = p274.p274file.FileId,
                            EntityIdCode = segments[1],
                            EntityTypeQualifier = segments[2],
                            EntityLastName = segments[3],
                            LoopName=loopName,
                        };
                        if (segments.Length > 8) info.EntityIdQualifier = segments[8];
                        if (segments.Length > 9) info.EntityId = segments[9];
                        p274.p274informations.Add(info);
                    }
                    else if ("1X,1Y,5W,80,E9,LI,QA,ZZ,30,X5".Contains(segments[1]))
                    {
                        P274AffiliatedEntity aff = new P274AffiliatedEntity
                        {
                            FileId = p274.p274file.FileId,
                            DetailId = p274.p274details.Last().DetailId,
                            EntityQualifier = segments[1],
                            EntityTypeQualifier = segments[2],
                            EntityLastName = segments[3]
                        };
                        if (segments.Length > 8) aff.EntityIdQualifier = segments[8];
                        if (segments.Length > 9) aff.EntityId = segments[9];
                        p274.p274affiliatedentities.Add(aff);
                        if (loopName == "2000C") loopName = "2100CB";
                        else if (loopName == "2000D") loopName = "2100DB";
                        else if (loopName == "2000E") loopName = "2100EB";
                    }
                    else
                    {
                        P274Detail detail = new P274Detail
                        {
                            FileId = p274.p274file.FileId,
                            DetailId = "DE" + lineNumber.ToString().PadLeft(7, '0'),
                            ProviderQualifier = segments[1],
                            ProviderTypeQualifier = segments[2],
                        };
                        if (segments.Length > 3) detail.ProviderLastName = segments[3];
                        if (segments.Length > 4) detail.ProviderFirstName = segments[4];
                        if (segments.Length > 5) detail.ProviderMiddleName = segments[5];
                        if (segments.Length > 7) detail.ProviderSuffix = segments[7];
                        if (segments.Length > 8) detail.ProviderIdQualifier = segments[8];
                        if (segments.Length > 9) detail.ProviderId = segments[9];
                        if (segments[1] == "QV") 
                        { 
                            groupId = "QV" + lineNumber.ToString().PadLeft(7, '0');
                        }
                        if (segments[1] == "77") 
                        { 
                            siteId="77" + lineNumber.ToString().PadLeft(7, '0');
                        }
                        detail.GroupId = groupId;
                        detail.SiteId = siteId;
                        p274.p274details.Add(detail);
                        
                    }
                    break;
                case "N2":
                    if (loopName == "2100CB" || loopName == "2100DB" || loopName == "2100EB")
                    {
                        p274.p274affiliatedentities.Last().EntityAdditionalName1 = segments[1];
                        if (segments.Length > 2) p274.p274affiliatedentities.Last().EntityAdditionalName2 = segments[2];
                    }
                    else
                    {
                        p274.p274details.Last().ProviderAdditionalName1 = segments[1];
                        if (segments.Length > 2) p274.p274details.Last().ProviderAdditionalName2 = segments[2];
                    }
                    break;
                case "DMG":
                    if (loopName == "2000C")
                    {
                        p274.p274details.Last().ProviderGender = segments[3];
                    }
                    break;
                case "DEG":
                    if (loopName == "2000C")
                    {
                        if (string.IsNullOrEmpty(p274.p274details.Last().ProviderDegreeCode1))
                        {
                            p274.p274details.Last().ProviderDegreeCode1 = segments[1];
                            p274.p274details.Last().ProviderDegreeDescription1 = segments[4];
                        }
                        else if (string.IsNullOrEmpty(p274.p274details.Last().ProviderDegreeCode2))
                        {
                            p274.p274details.Last().ProviderDegreeCode2 = segments[1];
                            p274.p274details.Last().ProviderDegreeDescription2 = segments[4];
                        }
                        else if (string.IsNullOrEmpty(p274.p274details.Last().ProviderDegreeCode3))
                        {
                            p274.p274details.Last().ProviderDegreeCode3 = segments[1];
                            p274.p274details.Last().ProviderDegreeDescription3 = segments[4];
                        }
                        else if (string.IsNullOrEmpty(p274.p274details.Last().ProviderDegreeCode4))
                        {
                            p274.p274details.Last().ProviderDegreeCode4 = segments[1];
                            p274.p274details.Last().ProviderDegreeDescription4 = segments[4];
                        }
                        else if (string.IsNullOrEmpty(p274.p274details.Last().ProviderDegreeCode5))
                        {
                            p274.p274details.Last().ProviderDegreeCode5 = segments[1];
                            p274.p274details.Last().ProviderDegreeDescription5 = segments[4];
                        }
                        else if (string.IsNullOrEmpty(p274.p274details.Last().ProviderDegreeCode6))
                        {
                            p274.p274details.Last().ProviderDegreeCode6 = segments[1];
                            p274.p274details.Last().ProviderDegreeDescription6 = segments[4];
                        }
                        else if (string.IsNullOrEmpty(p274.p274details.Last().ProviderDegreeCode7))
                        {
                            p274.p274details.Last().ProviderDegreeCode7 = segments[1];
                            p274.p274details.Last().ProviderDegreeDescription7 = segments[4];
                        }
                        else if (string.IsNullOrEmpty(p274.p274details.Last().ProviderDegreeCode8))
                        {
                            p274.p274details.Last().ProviderDegreeCode8 = segments[1];
                            p274.p274details.Last().ProviderDegreeDescription8 = segments[4];
                        }
                        else if (string.IsNullOrEmpty(p274.p274details.Last().ProviderDegreeCode9))
                        {
                            p274.p274details.Last().ProviderDegreeCode9 = segments[1];
                            p274.p274details.Last().ProviderDegreeDescription9 = segments[4];
                        }
                    }
                    break;
                case "LUI":
                    if (loopName == "2000C" || loopName == "2000D")
                    {
                        if (string.IsNullOrEmpty(p274.p274details.Last().ProviderLanguage1Code))
                        {
                            p274.p274details.Last().ProviderLanguage1CodeQualifier = segments[1];
                            p274.p274details.Last().ProviderLanguage1Code = segments[2];
                            p274.p274details.Last().ProviderLanguage1ProficiencyInd = segments[5];
                        }
                        else if (string.IsNullOrEmpty(p274.p274details.Last().ProviderLanguage2Code))
                        {
                            p274.p274details.Last().ProviderLanguage2CodeQualifier = segments[1];
                            p274.p274details.Last().ProviderLanguage2Code = segments[2];
                            p274.p274details.Last().ProviderLanguage2ProficiencyInd = segments[5];
                        }
                        else if (string.IsNullOrEmpty(p274.p274details.Last().ProviderLanguage3Code))
                        {
                            p274.p274details.Last().ProviderLanguage3CodeQualifier = segments[1];
                            p274.p274details.Last().ProviderLanguage3Code = segments[2];
                            p274.p274details.Last().ProviderLanguage3ProficiencyInd = segments[5];
                        }
                        else if (string.IsNullOrEmpty(p274.p274details.Last().ProviderLanguage4Code))
                        {
                            p274.p274details.Last().ProviderLanguage4CodeQualifier = segments[1];
                            p274.p274details.Last().ProviderLanguage4Code = segments[2];
                            p274.p274details.Last().ProviderLanguage4ProficiencyInd = segments[5];
                        }
                        else if (string.IsNullOrEmpty(p274.p274details.Last().ProviderLanguage5Code))
                        {
                            p274.p274details.Last().ProviderLanguage5CodeQualifier = segments[1];
                            p274.p274details.Last().ProviderLanguage5Code = segments[2];
                            p274.p274details.Last().ProviderLanguage5ProficiencyInd = segments[5];
                        }
                        else if (string.IsNullOrEmpty(p274.p274details.Last().ProviderLanguage6Code))
                        {
                            p274.p274details.Last().ProviderLanguage6CodeQualifier = segments[1];
                            p274.p274details.Last().ProviderLanguage6Code = segments[2];
                            p274.p274details.Last().ProviderLanguage6ProficiencyInd = segments[5];
                        }
                        else if (string.IsNullOrEmpty(p274.p274details.Last().ProviderLanguage7Code))
                        {
                            p274.p274details.Last().ProviderLanguage7CodeQualifier = segments[1];
                            p274.p274details.Last().ProviderLanguage7Code = segments[2];
                            p274.p274details.Last().ProviderLanguage7ProficiencyInd = segments[5];
                        }
                        else if (string.IsNullOrEmpty(p274.p274details.Last().ProviderLanguage8Code))
                        {
                            p274.p274details.Last().ProviderLanguage8CodeQualifier = segments[1];
                            p274.p274details.Last().ProviderLanguage8Code = segments[2];
                            p274.p274details.Last().ProviderLanguage8ProficiencyInd = segments[5];
                        }
                        else if (string.IsNullOrEmpty(p274.p274details.Last().ProviderLanguage9Code))
                        {
                            p274.p274details.Last().ProviderLanguage9CodeQualifier = segments[1];
                            p274.p274details.Last().ProviderLanguage9Code = segments[2];
                            p274.p274details.Last().ProviderLanguage9ProficiencyInd = segments[5];
                        }
                    }
                    break;
                case "DTP":
                    if (segments[1] == "092") p274.p274details.Last().ProviderContractEffectiveDate = segments[3];
                    else if (segments[1] == "093") p274.p274details.Last().ProviderContractExpirationDate = segments[3];
                    break;
                case "LQ":
                    P274SpecializationArea area = new P274SpecializationArea
                    {
                        FileId = p274.p274file.FileId,
                        DetailId = p274.p274details.Last().DetailId,
                        LQQualifier = segments[1],
                        LQCode = segments[2]
                    };
                    p274.p274specializationareas.Add(area);
                    break;
                case "TPB":
                    if (string.IsNullOrEmpty(p274.p274specializationareas.Last().NetworkRoleCode1)) p274.p274specializationareas.Last().NetworkRoleCode1 = segments[1];
                    else if (string.IsNullOrEmpty(p274.p274specializationareas.Last().NetworkRoleCode2)) p274.p274specializationareas.Last().NetworkRoleCode2 = segments[1];
                    break;
                case "YNQ":
                    p274.p274specializationareas.Last().CertificationConditionInd = segments[1];
                    p274.p274specializationareas.Last().BoardCertificationInd = segments[2];
                    break;
                case "REF":
                    P274GroupIdNumber groupid = new P274GroupIdNumber
                    {
                        FileId = p274.p274file.FileId,
                        DetailId = p274.p274details.Last().DetailId,
                        ReferenceQualifier = segments[1],
                        ReferenceId = segments[2]
                    };
                    p274.p274groupidnumbers.Add(groupid);
                    break;
                case "HAD":
                    p274.p274affiliatedentities.Last().HospitalStatusCode = segments[1];
                    if (segments.Length > 5) p274.p274affiliatedentities.Last().HospitalIdQualifier = segments[5];
                    if (segments.Length > 6) p274.p274affiliatedentities.Last().HospitalIdCode = segments[6];
                    break;
                case "WS":
                    P274SiteWorkSchedule schedule = new P274SiteWorkSchedule
                    {
                        FileId = p274.p274file.FileId,
                        DetailId = p274.p274details.Last().DetailId,
                        ShiftCode = segments[1]
                    };
                    if (segments.Length > 2) schedule.StartTime = segments[2];
                    if (segments.Length > 3) schedule.EndTime = segments[3];
                    p274.p274siteworkschedules.Add(schedule);
                    break;
                case "CRC":
                    P274SiteCRC crc = new P274SiteCRC
                    {
                        FileId = p274.p274file.FileId,
                        DetailId = p274.p274details.Last().DetailId,
                        Category = segments[1],
                        Condition = segments[2],
                        Code1 = segments[3]
                    };
                    if (segments.Length > 4) crc.Code2 = segments[4];
                    if (segments.Length > 5) crc.Code3 = segments[5];
                    if (segments.Length > 6) crc.Code4 = segments[6];
                    if (segments.Length > 7) crc.Code5 = segments[7];
                    p274.p274sitecrcs.Add(crc);
                    break;
                case "PDI":
                    if (segments.Length > 1) p274.p274details.Last().SiteRestrictionGender = segments[1];
                    if (segments.Length > 2) p274.p274details.Last().SiteRestrictionMinAge = segments[2];
                    if (segments.Length > 3) p274.p274details.Last().SiteRestrictionMaxAge = segments[3];
                    break;
                case "NX1":
                    p274.p274details.Last().SiteLocationCode = segments[1];
                    break;
                case "N3":
                    p274.p274details.Last().SiteLocationAddress = segments[1];
                    if (segments.Length > 2) p274.p274details.Last().SiteLocationAddress2 = segments[2];
                    break;
                case "N4":
                    p274.p274details.Last().SiteLocationCity = segments[1];
                    if (segments.Length > 2) p274.p274details.Last().SiteLocationState = segments[2];
                    if (segments.Length > 3) p274.p274details.Last().SiteLocationZip = segments[3];
                    if (segments.Length > 4) p274.p274details.Last().SiteLocationCountry = segments[4];
                    break;
            }
        }
    }
}
