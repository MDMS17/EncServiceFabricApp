using EncDataModel.RunStatus;
using EncDataModel.Submission837;
using Load837WPC.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Load837WPC.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Load837WPCController : ControllerBase
    {
        private readonly ILogger<Load837WPCController> _logger;
        private readonly Sub837CacheContext _context;
        private readonly WPC837IContext _context837I;
        private readonly WPC837PContext _context837P;

        public Load837WPCController(ILogger<Load837WPCController> logger, Sub837CacheContext context, WPC837IContext context837I, WPC837PContext context837P)
        {
            _logger = logger;
            _context = context;
            _context837I = context837I;
            _context837P = context837P;
        }
        //Load837WPC
        [HttpGet]
        public List<string> GetSource837WPCClaimFiles()
        {
            _logger.Log(LogLevel.Information, "inquiry source files contain WPC claims");
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string sourcePath = configuration["Source_WPC"];
            string archivePath = configuration["Archive_WPC"];
            List<string> result = Directory.EnumerateFiles(sourcePath).Select(x => Path.GetFileName(x)).ToList();
            result.Insert(0, archivePath);
            result.Insert(0, sourcePath);
            return result;
        }
        //Load837WPC/1
        [HttpGet("{id}")]
        public List<string> Load837WPCClaimFiles(long id)
        {
            List<string> result = new List<string>();
            _logger.Log(LogLevel.Information, "Load files contain WPC claims to database");
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string sourcePath = configuration["Source_WPC"];
            string archivePath = configuration["Archive_WPC"];
            string operationPath = configuration["OperationFolder"];
            string OperationFile = Path.Combine(operationPath, "Load837WPC", "Op" + DateTime.Today.ToString("yyyyMMdd") + ".json");
            RunStatus_LoadFileModel runStatus = JsonConvert.DeserializeObject<RunStatus_LoadFileModel>(System.IO.File.ReadAllText(OperationFile));
            DirectoryInfo di = new DirectoryInfo(sourcePath);
            FileInfo[] fis = di.GetFiles();
            int totalFiles = fis.Length;
            int goodFiles = 0;
            foreach (FileInfo fi in fis)
            {
                _logger.Log(LogLevel.Information, "Processing file " + fi.Name + " now...");
                string wpcClaimFile = System.IO.File.ReadAllText(fi.FullName);
                wpcClaimFile = wpcClaimFile.Replace("\r", "");
                List<string> wpcClaims = wpcClaimFile.Split('\n').ToList();
                wpcClaimFile = null;
                wpcClaims.RemoveAll(x => string.IsNullOrEmpty(x));
                int encounterCount = wpcClaims.Count;
                if (encounterCount > 0)
                {
                    Claim claim;
                    List<Claim> claims = new List<Claim>();
                    if (fi.Name.ToUpper().Contains("837I"))
                    {
                        foreach (string claimId in wpcClaims)
                        {
                            string xmlDocument = _context837I.XmlDocuments.Join(_context837I.Loopss, x => x.transactionid, y => y.transactionid, (x, y) => new { transactionDocument = x.transactionDocument, id = y.id, loopName = y.loopName }).Where(x => x.loopName == "2300").Join(_context837I.Clms, x => x.id, y => y.parentid, (x, y) => new { ordinal = y.ordinal, clm01 = y.CLM01, transactionDocument = x.transactionDocument }).Where(x => x.clm01 == claimId).OrderByDescending(x => x.ordinal).Select(x => x.transactionDocument).FirstOrDefault()?.ToString();
                            if (xmlDocument == null) continue;
                            claim = new Claim();
                            string ClaimId = claimId;
                            Processes.ProcessHipaaXml.Process837I(ref ClaimId, ref claim, ref xmlDocument);
                            claims.Add(claim);
                            if (claims.Count >= 1000)
                            {
                                SaveClaims(ref claims);
                                claims.Clear();
                            }
                        }
                        SaveClaims(ref claims);
                    }
                    else if (fi.Name.ToUpper().Contains("837P"))
                    {
                        foreach (string claimId in wpcClaims)
                        {
                            string xmlDocument = _context837P.XmlDocuments.Join(_context837P.Loopss, x => x.transactionid, y => y.transactionid, (x, y) => new { transactionDocument = x.transactionDocument, id = y.id, loopName = y.loopName }).Where(x => x.loopName == "2300").Join(_context837P.Clms, x => x.id, y => y.parentid, (x, y) => new { ordinal = y.ordinal, clm01 = y.CLM01, transactionDocument = x.transactionDocument }).Where(x => x.clm01 == claimId).OrderByDescending(x => x.ordinal).Select(x => x.transactionDocument).FirstOrDefault()?.ToString();
                            if (xmlDocument == null) continue;
                            claim = new Claim();
                            string ClaimId = claimId;
                            Processes.ProcessHipaaXml.Process837P(ref ClaimId, ref claim, ref xmlDocument);
                            claims.Add(claim);
                            if (claims.Count >= 1000)
                            {
                                SaveClaims(ref claims);
                                claims.Clear();
                            }
                        }
                        SaveClaims(ref claims);
                    }
                    else 
                    {
                        foreach (string claimId in wpcClaims)
                        {
                            string xmlDocument = _context837I.XmlDocuments.Join(_context837I.Loopss, x => x.transactionid, y => y.transactionid, (x, y) => new { transactionDocument = x.transactionDocument, id = y.id, loopName = y.loopName }).Where(x => x.loopName == "2300").Join(_context837I.Clms, x => x.id, y => y.parentid, (x, y) => new { ordinal = y.ordinal, clm01 = y.CLM01, transactionDocument = x.transactionDocument }).Where(x => x.clm01 == claimId).OrderByDescending(x => x.ordinal).Select(x => x.transactionDocument).FirstOrDefault()?.ToString();
                            if (xmlDocument == null)
                            {
                                xmlDocument = _context837P.XmlDocuments.Join(_context837P.Loopss, x => x.transactionid, y => y.transactionid, (x, y) => new { transactionDocument = x.transactionDocument, id = y.id, loopName = y.loopName }).Where(x => x.loopName == "2300").Join(_context837P.Clms, x => x.id, y => y.parentid, (x, y) => new { ordinal = y.ordinal, clm01 = y.CLM01, transactionDocument = x.transactionDocument }).Where(x => x.clm01 == claimId).OrderByDescending(x => x.ordinal).Select(x => x.transactionDocument).FirstOrDefault()?.ToString();
                                if (xmlDocument == null) continue;
                                //837P
                                claim = new Claim();
                                string ClaimId = claimId;
                                Processes.ProcessHipaaXml.Process837P(ref ClaimId, ref claim, ref xmlDocument);
                                claims.Add(claim);
                                if (claims.Count >= 1000)
                                {
                                    SaveClaims(ref claims);
                                    claims.Clear();
                                }
                            }
                            else 
                            {
                                //837I
                                claim = new Claim();
                                string ClaimId = claimId;
                                Processes.ProcessHipaaXml.Process837I(ref ClaimId, ref claim, ref xmlDocument);
                                claims.Add(claim);
                                if (claims.Count >= 1000)
                                {
                                    SaveClaims(ref claims);
                                    claims.Clear();
                                }
                            }
                        }
                        SaveClaims(ref claims);
                    }
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

        private void SaveClaims(ref List<Claim> claims)
        {
            _context.ClaimHeaders.AddRange(claims.Select(x => x.Header).ToList());
            _context.ClaimCAS.AddRange(claims.SelectMany(x => x.Cases).ToList());
            _context.ClaimCRCs.AddRange(claims.SelectMany(x => x.CRCs).ToList());
            _context.ClaimHIs.AddRange(claims.SelectMany(x => x.His).ToList());
            _context.ClaimK3s.AddRange(claims.SelectMany(x => x.K3s).ToList());
            _context.ClaimLineFRMs.AddRange(claims.SelectMany(x => x.FRMs).ToList());
            _context.ClaimLineLQs.AddRange(claims.SelectMany(x => x.LQs).ToList());
            _context.ClaimLineMEAs.AddRange(claims.SelectMany(x => x.Meas).ToList());
            _context.ClaimLineSVDs.AddRange(claims.SelectMany(x => x.SVDs).ToList());
            _context.ClaimNtes.AddRange(claims.SelectMany(x => x.Notes).ToList());
            _context.ClaimPatients.AddRange(claims.SelectMany(x => x.Patients).ToList());
            _context.ClaimProviders.AddRange(claims.SelectMany(x => x.Providers).ToList());
            _context.ClaimPWKs.AddRange(claims.SelectMany(x => x.PWKs).ToList());
            _context.ClaimSBRs.AddRange(claims.SelectMany(x => x.Subscribers).ToList());
            _context.ClaimSecondaryIdentifications.AddRange(claims.SelectMany(x => x.SecondaryIdentifications).ToList());
            _context.ProviderContacts.AddRange(claims.SelectMany(x => x.ProviderContacts).ToList());
            _context.ServiceLines.AddRange(claims.SelectMany(x => x.Lines).ToList());
            _context.ToothStatus.AddRange(claims.SelectMany(x => x.ToothStatuses).ToList());
            _context.SaveChanges();
        }

        private void MoveFile(string archivePath, FileInfo fi)
        {
            if (System.IO.File.Exists(Path.Combine(archivePath, fi.Name))) System.IO.File.Delete(Path.Combine(archivePath, fi.Name));
            fi.MoveTo(Path.Combine(archivePath, fi.Name));
        }
    }
}
