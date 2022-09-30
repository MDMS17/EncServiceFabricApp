using EncDataModel.Premium820;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Premium820.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Premium820.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Premium820Controller : ControllerBase
    {
        private readonly ILogger<Premium820Controller> _logger;
        private readonly Premium820Context _context;
        public Premium820Controller(ILogger<Premium820Controller> logger, Premium820Context context)
        {
            _logger = logger;
            _context = context;
        }
        //Premium820
        [HttpGet]
        public List<string> GetNewPremium820Files()
        {
            _logger.Log(LogLevel.Information, "inquiry unprocessed files");
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string sourcePath = configuration["Source_Premium820"];
            string archivePath = configuration["Archive_Premium820"];
            List<string> result = Directory.EnumerateFiles(sourcePath).Select(x => Path.GetFileName(x)).ToList();
            result.Insert(0, archivePath);
            result.Insert(0, sourcePath);
            return result;
        }
        //Premium820/1
        [HttpGet("{id}")]
        public Tuple<int, int> ProcessPremium820Files(long id)
        {
            _logger.Log(LogLevel.Information, "process new premium 820 files");
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string sourcePath = configuration["Source_Premium820"];
            string archivePath = configuration["Archive_Premium820"];
            DirectoryInfo di = new DirectoryInfo(sourcePath);
            FileInfo[] fis = di.GetFiles();
            int totalFiles = fis.Length;
            int goodFiles = 0;
            foreach (FileInfo fi in fis)
            {
                _logger.Log(LogLevel.Information, "Processing file " + fi.Name + " now...");
                File820 processingFile = _context.Files.FirstOrDefault(x => x.FileName == fi.Name);
                if (processingFile != null)
                {
                    _logger.Log(LogLevel.Information, fi.Name + " has been already processed");
                    MoveFile(archivePath, fi);
                    continue;
                }
                List<Member820> premiums = new List<Member820>();
                string s820 = System.IO.File.ReadAllText(fi.FullName).Replace("\r", "").Replace("\n", "");
                string[] s820Lines = s820.Split('~');
                s820 = null;
                processingFile = new File820();
                processingFile.FileName = fi.Name;
                string[] tempSegs = s820Lines[0].Split('*');
                processingFile.SenderId = tempSegs[6];
                processingFile.ReceiverId = tempSegs[8];
                processingFile.InterchangeControlNumber = tempSegs[13];
                tempSegs = s820Lines[1].Split('*');
                processingFile.TransactionDate = tempSegs[4];
                processingFile.TransactionTime = tempSegs[5];
                tempSegs = s820Lines[3].Split('*');
                processingFile.TotalPremiumAmount = tempSegs[2];
                processingFile.PaymentMethodQualifier = tempSegs[3];
                processingFile.PaymentMethod = tempSegs[4];
                processingFile.TransactionNumber = tempSegs[10];
                processingFile.CheckIssueDate = tempSegs[16];
                tempSegs = s820Lines[4].Split('*');
                processingFile.TraceTypeCode = tempSegs[1];
                processingFile.TraceNumber = tempSegs[2];
                _context.Files.Add(processingFile);
                _context.SaveChanges();
                string loopName = "";
                bool firstRMR = true;
                foreach (string line in s820Lines)
                {
                    Parse820Line(line, ref loopName, ref processingFile, ref premiums, ref firstRMR);
                }
                _context.Members.AddRange(premiums);
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
        private void Parse820Line(string line, ref string loopName, ref File820 processingFile, ref List<Member820> premiums, ref bool firstRMR)
        {
            string[] segments = line.Split('*');
            switch (segments[0])
            {
                case "ST":
                    loopName = "1000A";
                    break;
                case "ENT":
                    loopName = "2000B";
                    Member820 premium = new Member820();
                    premium.FileId = processingFile.FileId;
                    premium.EntityIdQualifier = segments[3];
                    premium.EntityId = segments[4];
                    premiums.Add(premium);
                    firstRMR = true;
                    break;
                case "NM1":
                    if (loopName == "2000B")
                    {
                        premiums.Last().MemberLastName = segments[3];
                        premiums.Last().MemberFirstName = segments[4];
                        premiums.Last().MemberMiddleName = segments[5];
                        premiums.Last().MemberIdQualifier = segments[8];
                        premiums.Last().MemberId = segments[9];
                    }
                    break;
                case "N1":
                    if (loopName == "1000A" && segments[1] == "PR")
                    {
                        loopName = "1000B";
                        processingFile.PayerName = segments[2];
                    }
                    else if (loopName == "1000A" && segments[1] == "PE")
                    {
                        processingFile.PayeeLastName = segments[2];
                    }
                    break;
                case "N3":
                    if (loopName == "1000A")
                    {
                        processingFile.PayeeAddress = segments[1];
                    }
                    else if (loopName == "1000B")
                    {
                        processingFile.PayerAddress = segments[1];
                    }
                    break;
                case "N4":
                    if (loopName == "1000A")
                    {
                        processingFile.PayeeCity = segments[1];
                        processingFile.PayeeState = segments[2];
                        processingFile.PayeeZip = segments[3];
                    }
                    else if (loopName == "1000B")
                    {
                        processingFile.PayerCity = segments[1];
                        processingFile.PayerState = segments[2];
                        processingFile.PayerZip = segments[3];
                    }
                    break;
                case "RMR":
                    loopName = "2300B";
                    if (firstRMR)
                    {
                        premiums.Last().InsuranceRemittanceReferenceNumber = segments[2];
                        premiums.Last().DetailPremiumPaymentAmount = segments[4];
                        if (segments.Length > 5) premiums.Last().BilledPremiumAmount = segments[5];
                        firstRMR = false;
                    }
                    else
                    {
                        Member820 premium2 = new Member820();
                        premium2.FileId = processingFile.FileId;
                        premium2.EntityIdQualifier = premiums.Last().EntityIdQualifier;
                        premium2.EntityId = premiums.Last().EntityId;
                        premium2.MemberLastName = premiums.Last().MemberLastName;
                        premium2.MemberFirstName = premiums.Last().MemberFirstName;
                        premium2.MemberMiddleName = premiums.Last().MemberMiddleName;
                        premium2.MemberIdQualifier = premiums.Last().MemberIdQualifier;
                        premium2.MemberId = premiums.Last().MemberId;
                        premium2.InsuranceRemittanceReferenceNumber = segments[2];
                        premium2.DetailPremiumPaymentAmount = segments[4];
                        if (segments.Length > 5) premium2.BilledPremiumAmount = segments[5];
                        premiums.Add(premium2);
                    }
                    break;
                case "REF":
                    if (loopName == "1000A")
                    {
                        processingFile.PayeeIdQualifier = segments[1];
                        processingFile.PayeeId = segments[2];
                    }
                    else if (loopName == "2300B")
                    {
                        if (segments[1] == "18") premiums.Last().CountyCode = segments[2];
                        else if (segments[1] == "ZZ" && segments[2].Length == 2) premiums.Last().OrganizationalReferenceId = segments[2];
                        else if (segments[1] == "ZZ" && segments[2].Length > 2) premiums.Last().OrganizationalDescription = segments[2];
                    }
                    break;
                case "DTM":
                    if (loopName == "2300B")
                    {
                        premiums.Last().CapitationFromDate = segments[6].Split('-')[0];
                        premiums.Last().CapitationThroughDate = segments[6].Split('-')[1];
                    }
                    else if (loopName == "1000A")
                    {
                        processingFile.CoverageFirstDate = segments[2].Split('-')[0];
                        processingFile.CoverageLastDate = segments[2].Split('-')[1];
                    }
                    break;
                case "ADX":
                    if (loopName == "2300B")
                    {
                        premiums.Last().AdjustmentAmount = segments[1];
                        premiums.Last().AdjustmentReasonCode = segments[2];
                    }
                    break;
            }
        }
    }
}
