using EncDataModel.ChartReview;
using LoadChartReviewCsv.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LoadChartReviewCsv.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoadChartReviewCsvController : ControllerBase
    {
        private readonly ILogger<LoadChartReviewCsvController> _logger;
        private readonly ChartReviewContext _context;
        public LoadChartReviewCsvController(ILogger<LoadChartReviewCsvController> logger, ChartReviewContext context)
        {
            _logger = logger;
            _context = context;
        }
        //LoadChartReviewCsv
        [HttpGet]
        public List<string> GetNewChartReviewFiles()
        {
            _logger.Log(LogLevel.Information, "inquiry unprocessed files");
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string sourcePath = configuration["Source_ChartReview"];
            string archivePath = configuration["Archive_ChartReview"];
            List<string> result = Directory.EnumerateFiles(sourcePath).Select(x => Path.GetFileName(x)).ToList();
            result.Insert(0, archivePath);
            result.Insert(0, sourcePath);
            return result;
        }
        //LoadChartReviewCsv/1
        [HttpGet("{id}")]
        public List<string> ProcessChartReviewFiles(long id)
        {
            List<string> result = new List<string>();
            _logger.Log(LogLevel.Information, "process new chart review files");
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string sourcePath = configuration["Source_ChartReview"];
            string archivePath = configuration["Archive_ChartReview"];
            DirectoryInfo di = new DirectoryInfo(sourcePath);
            FileInfo[] fis = di.GetFiles();
            int totalFiles = fis.Length;
            int goodFiles = 0;
            foreach (FileInfo fi in fis)
            {
                _logger.Log(LogLevel.Information, "Processing file " + fi.Name + " now...");
                string s = System.IO.File.ReadAllText(fi.FullName).Replace("\r", "");
                string[] chartLines = s.Split('\n');
                
                List<ChartReviewRecord> chartReviewRecords = new List<ChartReviewRecord>();
                for (int i=1;i<chartLines.Length;i++)
                {
                    string[] segments = chartLines[i].Split(',');
                    if (segments.Length < 7) continue;
                    ChartReviewRecord record = new ChartReviewRecord
                    {
                        ClaimType = segments[0],
                        ProviderNpi = segments[1],
                        MemberHicn = segments[2],
                        MemberDOB = segments[3],
                        DosFromDate = segments[4],
                        DosToDate = segments[5],
                        DiagnosisCode = segments[6],
                        DeleteIndicator = segments[7],
                        ProcedureCode = segments[8],
                        RevenueCode = segments[9]
                    };
                    chartReviewRecords.Add(record);
                }
                _context.Records.AddRange(chartReviewRecords);
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
    }
}
