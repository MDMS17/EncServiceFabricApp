using JsonLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ValidateMCPDIPJsonFile.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ValidateMCPDIPJsonFileController : ControllerBase
    {
        private readonly ILogger<ValidateMCPDIPJsonFileController> _logger;
        public ValidateMCPDIPJsonFileController(ILogger<ValidateMCPDIPJsonFileController> logger) 
        {
            _logger = logger;
        }
        //ValidateMCPDIPJsonFile
        [HttpGet]
        public List<string> GetMCPDIPJsonFiles()
        {
            _logger.Log(LogLevel.Information, "retrieve MCPDIP json files");
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string sourcePath = configuration["Output_MCPDIP"];
            List<string> result = Directory.EnumerateFiles(sourcePath).Select(x => Path.GetFileName(x)).ToList();
            result.Insert(0, sourcePath);
            return result;
        }
        //ValidateMCPDIPJsonFile
        [HttpPost]
        public List<string> ProcessMCPDIPResponseFiles([FromBody]List<string> selectedFiles)
        {
            List<string> result = new List<string>();
            _logger.Log(LogLevel.Information, "validate selected MCPDIP json files");
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string sourcePath = configuration["Output_MCPDIP"];
            foreach (string selectedFile in selectedFiles)
            {
                if (!System.IO.File.Exists(Path.Combine(sourcePath, selectedFile)))
                {
                    result.Add("");
                }
                else 
                {
                    if (selectedFile.ToUpper().Contains("MCPD"))
                    {
                        string mcpdFile = System.IO.File.ReadAllText(Path.Combine(sourcePath, selectedFile));
                        Tuple<bool, string> validationResult = JsonOperationsNew.ValidateMcpdFile(ref mcpdFile);
                        if (validationResult.Item1) result.Add("");
                        else result.Add(validationResult.Item2);
                    }
                    else if (selectedFile.ToUpper().Contains("PCPA"))
                    {
                        string pcpaFile = System.IO.File.ReadAllText(Path.Combine(sourcePath, selectedFile));
                        Tuple<bool, string> validationResult = JsonOperationsNew.ValidatePcpaFile(ref pcpaFile);
                        if (validationResult.Item1) result.Add("");
                        else result.Add(validationResult.Item2);
                    }
                }
            }
            result.Insert(0, sourcePath);
            return result;
        }

    }
}
