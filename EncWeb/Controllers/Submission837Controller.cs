using EncDataModel.RunStatus;
using EncWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Fabric.Query;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EncWeb.Controllers
{
    public class Submission837Controller : Controller
    {
        private readonly HttpClient httpClient;
        private readonly FabricClient fabricClient;
        private readonly StatelessServiceContext serviceContext;
        public Submission837Controller(HttpClient _httpClient, FabricClient _fabricClient, StatelessServiceContext _serviceContext)
        {
            httpClient = _httpClient;
            fabricClient = _fabricClient;
            serviceContext = _serviceContext;
            if (httpClient.Timeout != TimeSpan.FromMinutes(120))
            {
                httpClient.Timeout = TimeSpan.FromMinutes(120);
            }
            if (fabricClient.Settings.ConnectionIdleTimeout != TimeSpan.FromMinutes(120))
            {
                fabricClient.Settings.ConnectionIdleTimeout = TimeSpan.FromMinutes(120);
            }
        }
        public async Task<IActionResult> Load837File()
        {
            ExportFilesModel model = new ExportFilesModel();
            Uri serviceName = new Uri($"{this.serviceContext.CodePackageActivationContext.ApplicationName}/Load837File");
            Uri proxyAddress = this.GetProxyAddress(serviceName);
            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);
            foreach (Partition partition in partitions)
            {
                string proxyUrl = $"{proxyAddress}/Load837File?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";

                using (HttpResponseMessage response = await this.httpClient.GetAsync(proxyUrl))
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        continue;
                    }
                    model.newFiles = JsonConvert.DeserializeObject<List<string>>(await response.Content.ReadAsStringAsync());
                }
            }
            if (model.newFiles != null)
            {
                model.sourcePath = model.newFiles[0];
                model.archivePath = model.newFiles[1];
                model.newFiles.RemoveRange(0, 2);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Load837File(ExportFilesModel model)
        {
            model.newFiles = new List<string>();
            Uri serviceName = new Uri($"{this.serviceContext.CodePackageActivationContext.ApplicationName}/Load837File");
            Uri proxyAddress = this.GetProxyAddress(serviceName);
            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);
            foreach (Partition partition in partitions)
            {
                string proxyUrl = $"{proxyAddress}/Load837File/1/?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";

                using (HttpResponseMessage response = await this.httpClient.GetAsync(proxyUrl))
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        continue;
                    }
                    List<string> result = JsonConvert.DeserializeObject<List<string>>(await response.Content.ReadAsStringAsync());
                    model.totalFiles = result[0];
                    model.goodFiles = result[1];
                    break;
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Load837Meditrac()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string OperationPath = configuration["OperationFolder"];
            string OperationFile = Path.Combine(OperationPath, "Load837Meditrac", "Op" + DateTime.Today.ToString("yyyyMMdd") + ".json");
            RunStatus_Load837Database runStatus = JsonConvert.DeserializeObject<RunStatus_Load837Database>(System.IO.File.ReadAllText(OperationFile));
            Load837DatabaseModel model = new Load837DatabaseModel();
            model.lastRunStartDate = runStatus.CurrentRunStartDate;
            model.lastRunEndDate = runStatus.CurrentRunEndDate;
            model.lastRunDate = runStatus.CurrentRunDate;
            model.lastRunUser = runStatus.CurrentRunUser;
            model.lastRunStatus = runStatus.CurrentRunStatus;
            model.currentRunStartDate = DateTime.Today.AddDays(-7).ToShortDateString();
            model.currentRunEndDate = DateTime.Today.ToShortDateString();
            model.newFiles = new List<string>();
            if (runStatus.CurrentRunStatus == "1")
            {
                model.message = "data loading is already initialized by " + runStatus.CurrentRunUser;
                model.enableLoadButton = false;
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Load837Meditrac(Load837DatabaseModel model)
        {
            if (string.IsNullOrEmpty(model.currentRunStartDate) || string.IsNullOrEmpty(model.currentRunEndDate)) 
            {
                model.message = "Must provide start date and end date for data loading from Meditrac";
                return View(model);
            }
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string OperationPath = configuration["OperationFolder"];
            string OperationFile = Path.Combine(OperationPath, "Load837Meditrac", "Op" + DateTime.Today.ToString("yyyyMMdd") + ".json");
            RunStatus_Load837Database runStatus = new RunStatus_Load837Database 
            {
                LastRunStartDate=model.lastRunStartDate,
                LastRunEndDate=model.lastRunStartDate,
                LastRunDate=model.lastRunDate,
                LastRunUser=model.lastRunUser,
                LastRunStatus=model.lastRunStatus,
                CurrentRunStartDate=model.currentRunStartDate,
                CurrentRunEndDate=model.currentRunEndDate,
                CurrentRunDate=DateTime.Today.ToShortDateString(),
                CurrentRunUser=Environment.UserName,
                CurrentRunStatus="1"
            };
            System.IO.File.WriteAllText(OperationFile, JsonConvert.SerializeObject(runStatus, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            model.newFiles = new List<string>();
            model.newFiles.Add(model.currentRunStartDate);
            model.newFiles.Add(model.currentRunEndDate);
            model.newFiles.Add(model.LoadCMSP.ToString());
            model.newFiles.Add(model.LoadCMSI.ToString());
            model.newFiles.Add(model.LoadDualP.ToString());
            model.newFiles.Add(model.LoadDualI.ToString());
            model.newFiles.Add(model.LoadStateP.ToString());
            model.newFiles.Add(model.LoadStateI.ToString());
            Uri serviceName = new Uri($"{this.serviceContext.CodePackageActivationContext.ApplicationName}/Load837Meditrac");
            Uri proxyAddress = this.GetProxyAddress(serviceName);
            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);
            foreach (Partition partition in partitions)
            {
                string proxyUrl = $"{proxyAddress}/Load837Meditrac?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";
                HttpContent content = new StringContent(JsonConvert.SerializeObject(model.newFiles), Encoding.UTF8, "application/json");
                using (HttpResponseMessage response = await this.httpClient.PostAsync(proxyUrl, content))
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        continue;
                    }
                    model.newFiles = JsonConvert.DeserializeObject<List<string>>(await response.Content.ReadAsStringAsync());
                }
            }

            return View(model);
        }
        public async Task<IActionResult> Generate837()
        {
            Export837Model model = new Export837Model();
            Uri serviceName = new Uri($"{this.serviceContext.CodePackageActivationContext.ApplicationName}/Export837");
            Uri proxyAddress = this.GetProxyAddress(serviceName);
            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);
            foreach (Partition partition in partitions)
            {
                string proxyUrl = $"{proxyAddress}/Export837?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";

                using (HttpResponseMessage response = await this.httpClient.GetAsync(proxyUrl))
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        continue;
                    }
                    model.newFiles = JsonConvert.DeserializeObject<List<string>>(await response.Content.ReadAsStringAsync());
                }
            }
            if (model.newFiles != null)
            {
                model.sourcePath = model.newFiles[0];
                model.newFiles.RemoveRange(0, 1);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Generate837(Export837Model model)
        {
            model.newFiles = new List<string>();
            model.newFiles.Add(model.ProdTestFlag);
            model.newFiles.Add(model.ExportCMSP.ToString());
            model.newFiles.Add(model.ExportCMSI.ToString());
            model.newFiles.Add(model.ExportCMSE.ToString());
            model.newFiles.Add(model.ExportDualP.ToString());
            model.newFiles.Add(model.ExportDualI.ToString());
            model.newFiles.Add(model.ExportDualE.ToString());
            model.newFiles.Add(model.ExportStateP.ToString());
            model.newFiles.Add(model.ExportStateI.ToString());
            model.newFiles.Add(model.ExportStateE.ToString());
            Uri serviceName = new Uri($"{this.serviceContext.CodePackageActivationContext.ApplicationName}/Export837");
            Uri proxyAddress = this.GetProxyAddress(serviceName);
            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);
            foreach (Partition partition in partitions)
            {
                string proxyUrl = $"{proxyAddress}/Export837?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";
                HttpContent content = new StringContent(JsonConvert.SerializeObject(model.newFiles), Encoding.UTF8, "application/json");
                using (HttpResponseMessage response = await this.httpClient.PostAsync(proxyUrl, content))
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        continue;
                    }
                    model.newFiles = JsonConvert.DeserializeObject<List<string>>(await response.Content.ReadAsStringAsync());
                }
            }
            if (model.newFiles != null) 
            {
                model.totalDatabaseRecords = model.newFiles[0];
                model.goodDatabaseRecords = model.newFiles[1];
            }
            return View(model);
        }
        public async Task<IActionResult> Load837SubHistory()
        {
            ExportFilesModel model = new ExportFilesModel();
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string OperationPath = configuration["OperationFolder"];
            string OperationFile = Path.Combine(OperationPath, "Load837SubHistory", "Op" + DateTime.Today.ToString("yyyyMMdd") + ".json");
            model.enableSubmit = true;
            if (System.IO.File.Exists(OperationFile)) 
            {
                RunStatus_LoadFileModel runStatus = JsonConvert.DeserializeObject<RunStatus_LoadFileModel>(System.IO.File.ReadAllText(OperationFile));
                if (runStatus.CurrentRunStatus == "1")
                {
                    model.displayMessage = "This process has been initiated by " + runStatus.CurrentRunUser;
                    model.enableSubmit = false;
                }
            }

            Uri serviceName = new Uri($"{this.serviceContext.CodePackageActivationContext.ApplicationName}/Load837SubHistory");
            Uri proxyAddress = this.GetProxyAddress(serviceName);
            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);
            foreach (Partition partition in partitions)
            {
                string proxyUrl = $"{proxyAddress}/Load837SubHistory?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";

                using (HttpResponseMessage response = await this.httpClient.GetAsync(proxyUrl))
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        continue;
                    }
                    model.newFiles = JsonConvert.DeserializeObject<List<string>>(await response.Content.ReadAsStringAsync());
                }
            }
            if (model.newFiles != null)
            {
                model.sourcePath = model.newFiles[0];
                model.archivePath = model.newFiles[1];
                model.newFiles.RemoveRange(0, 2);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Load837SubHistory(ExportFilesModel model)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string OperationPath = configuration["OperationFolder"];
            string OperationFile = Path.Combine(OperationPath, "Load837SubHistory", "Op" + DateTime.Today.ToString("yyyyMMdd") + ".json");
            RunStatus_LoadFileModel runStatus;
            if (System.IO.File.Exists(OperationFile))
            {
                runStatus = JsonConvert.DeserializeObject<RunStatus_LoadFileModel>(System.IO.File.ReadAllText(OperationFile));
                runStatus.CurrentRunStatus = "1";
            }
            else 
            {
                runStatus = new RunStatus_LoadFileModel
                {
                    CurrentRunUser=Environment.UserName,
                    CurrentRunDate=DateTime.Today.ToShortDateString(),
                    CurrentRunStatus="1",
                    FileNames=new List<string>()
                };
            }
            System.IO.File.WriteAllText(OperationFile, JsonConvert.SerializeObject(runStatus, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            model.newFiles = new List<string>();
            Uri serviceName = new Uri($"{this.serviceContext.CodePackageActivationContext.ApplicationName}/Load837SubHistory");
            Uri proxyAddress = this.GetProxyAddress(serviceName);
            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);
            foreach (Partition partition in partitions)
            {
                string proxyUrl = $"{proxyAddress}/Load837SubHistory/1/?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";

                using (HttpResponseMessage response = await this.httpClient.GetAsync(proxyUrl))
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        continue;
                    }
                    List<string> result = JsonConvert.DeserializeObject<List<string>>(await response.Content.ReadAsStringAsync());
                    model.totalFiles = result[0];
                    model.goodFiles = result[1];
                    break;
                }
            }
            if (!string.IsNullOrEmpty(model.totalFiles))
            {
                model.displayMessage = $"Total files: {model.totalFiles }, processed files: {model.goodFiles}";
            }
            else 
            {
                model.displayMessage = "Loading is in progress...";
            }
            return View(model);
        }
        public async Task<IActionResult> LoadRecon()
        {
            LoadReconModel model = new LoadReconModel();
            Uri serviceName = new Uri($"{this.serviceContext.CodePackageActivationContext.ApplicationName}/Load837Recon");
            Uri proxyAddress = this.GetProxyAddress(serviceName);
            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);
            foreach (Partition partition in partitions)
            {
                string proxyUrl = $"{proxyAddress}/Load837Recon?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";

                using (HttpResponseMessage response = await this.httpClient.GetAsync(proxyUrl))
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        continue;
                    }
                    model.newFiles = JsonConvert.DeserializeObject<List<Tuple<string,string>>>(await response.Content.ReadAsStringAsync());
                }
            }
            if (model.newFiles == null) model.newFiles = new List<Tuple<string,string>>();
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string OperationPath = configuration["OperationFolder"];
            string OperationFile = Path.Combine(OperationPath, "Load837Recon", "Op" + DateTime.Today.ToString("yyyyMMdd") + ".json");
            model.enableSubmit = true;
            if (System.IO.File.Exists(OperationFile))
            {
                RunStatus_LoadFileModel runStatus = JsonConvert.DeserializeObject<RunStatus_LoadFileModel>(System.IO.File.ReadAllText(OperationFile));
                if (runStatus.CurrentRunStatus == "1")
                {
                    model.displayMessage = "This process has been initiated by " + runStatus.CurrentRunUser;
                    model.enableSubmit = false;
                }
            }
            if (model.newFiles.Count == 0) model.enableSubmit = false;
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> LoadRecon(LoadReconModel model)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string OperationPath = configuration["OperationFolder"];
            string OperationFile = Path.Combine(OperationPath, "Load837Recon", "Op" + DateTime.Today.ToString("yyyyMMdd") + ".json");
            RunStatus_LoadFileModel runStatus;
            if (System.IO.File.Exists(OperationFile))
            {
                runStatus = JsonConvert.DeserializeObject<RunStatus_LoadFileModel>(System.IO.File.ReadAllText(OperationFile));
                runStatus.CurrentRunStatus = "1";
            }
            else
            {
                runStatus = new RunStatus_LoadFileModel
                {
                    CurrentRunUser = Environment.UserName,
                    CurrentRunDate = DateTime.Today.ToShortDateString(),
                    CurrentRunStatus = "1",
                    FileNames = new List<string>()
                };
            }
            System.IO.File.WriteAllText(OperationFile, JsonConvert.SerializeObject(runStatus, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            model.newFiles = new List<Tuple<string,string>>();
            Uri serviceName = new Uri($"{this.serviceContext.CodePackageActivationContext.ApplicationName}/Load837Recon");
            Uri proxyAddress = this.GetProxyAddress(serviceName);
            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);
            foreach (Partition partition in partitions)
            {
                string proxyUrl = $"{proxyAddress}/Load837Recon/1/?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";

                using (HttpResponseMessage response = await this.httpClient.GetAsync(proxyUrl))
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        continue;
                    }
                    List<string> result = JsonConvert.DeserializeObject<List<string>>(await response.Content.ReadAsStringAsync());
                    model.totalFiles = result[0];
                    model.goodFiles = result[1];
                    break;
                }
            }
            if (!string.IsNullOrEmpty(model.totalFiles))
            {
                model.displayMessage = $"Total files: {model.totalFiles }, processed files: {model.goodFiles}";
            }
            else
            {
                model.displayMessage = "Loading is in progress...";
            }
            return View(model);
        }
        public async Task<IActionResult> Load837WPC()
        {
            ExportFilesModel model = new ExportFilesModel();
            Uri serviceName = new Uri($"{this.serviceContext.CodePackageActivationContext.ApplicationName}/Load837WPC");
            Uri proxyAddress = this.GetProxyAddress(serviceName);
            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);
            foreach (Partition partition in partitions)
            {
                string proxyUrl = $"{proxyAddress}/Load837WPC?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";

                using (HttpResponseMessage response = await this.httpClient.GetAsync(proxyUrl))
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        continue;
                    }
                    model.newFiles = JsonConvert.DeserializeObject<List<string>>(await response.Content.ReadAsStringAsync());
                }
            }
            if (model.newFiles != null)
            {
                model.sourcePath = model.newFiles[0];
                model.archivePath = model.newFiles[1];
                model.newFiles.RemoveRange(0, 2);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Load837WPC(ExportFilesModel model)
        {
            model.newFiles = new List<string>();
            Uri serviceName = new Uri($"{this.serviceContext.CodePackageActivationContext.ApplicationName}/Load837WPC");
            Uri proxyAddress = this.GetProxyAddress(serviceName);
            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);
            foreach (Partition partition in partitions)
            {
                string proxyUrl = $"{proxyAddress}/Load837WPC/1/?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";

                using (HttpResponseMessage response = await this.httpClient.GetAsync(proxyUrl))
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        continue;
                    }
                    List<string> result = JsonConvert.DeserializeObject<List<string>>(await response.Content.ReadAsStringAsync());
                    model.totalFiles = result[0];
                    model.goodFiles = result[1];
                    break;
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Load837Facets()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string OperationPath = configuration["OperationFolder"];
            string OperationFile = Path.Combine(OperationPath, "Load837Facets", "Op" + DateTime.Today.ToString("yyyyMMdd") + ".json");
            Load837DatabaseModel model = new Load837DatabaseModel();
            model.enableLoadButton = true;
            if (System.IO.File.Exists(OperationFile))
            {
                RunStatus_Load837Database runStatus = JsonConvert.DeserializeObject<RunStatus_Load837Database>(System.IO.File.ReadAllText(OperationFile));
                if (runStatus.CurrentRunStatus == "1")
                {
                    model.message = "This process has been initiated by " + runStatus.CurrentRunUser;
                    model.enableLoadButton = false;
                }
                model.lastRunStartDate = runStatus.CurrentRunStartDate;
                model.lastRunEndDate = runStatus.CurrentRunEndDate;
                model.lastRunDate = runStatus.CurrentRunDate;
                model.lastRunUser = runStatus.CurrentRunUser;
                model.lastRunStatus = runStatus.CurrentRunStatus;
            }
            model.currentRunStartDate = DateTime.Today.AddDays(-7).ToShortDateString();
            model.currentRunEndDate = DateTime.Today.ToShortDateString();
            model.newFiles = new List<string>();
            Uri serviceName = new Uri($"{this.serviceContext.CodePackageActivationContext.ApplicationName}/Load837Facets");
            Uri proxyAddress = this.GetProxyAddress(serviceName);
            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);
            foreach (Partition partition in partitions)
            {
                string proxyUrl = $"{proxyAddress}/Load837Facets?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";

                using (HttpResponseMessage response = await this.httpClient.GetAsync(proxyUrl))
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        continue;
                    }
                    model.newFiles = JsonConvert.DeserializeObject<List<string>>(await response.Content.ReadAsStringAsync());
                }
            }
            model.DateRange = true;
            model.Include = false;
            model.Exclude = false;
            if (model.newFiles != null)
            {
                model.sourcePath = model.newFiles[0];
                model.archivePath = model.newFiles[1];
                model.newFiles.RemoveRange(0, 2);
                foreach (string s in model.newFiles) 
                { 
                    if (s.ToUpper().Contains("INCLUDE")) model.Include = true;
                    if (s.ToUpper().Contains("EXCLUDE")) model.Exclude = true;
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Load837Facets(Load837DatabaseModel model)
        {
            if (string.IsNullOrEmpty(model.currentRunStartDate) || string.IsNullOrEmpty(model.currentRunEndDate))
            {
                model.message = "Must provide start date and end date for data loading from Facets";
                return View(model);
            }
            RunStatus_Load837Database runStatus = new RunStatus_Load837Database
            {
                LastRunStartDate = model.lastRunStartDate,
                LastRunEndDate = model.lastRunStartDate,
                LastRunDate = model.lastRunDate,
                LastRunUser = model.lastRunUser,
                LastRunStatus = model.lastRunStatus,
                CurrentRunStartDate = model.currentRunStartDate,
                CurrentRunEndDate = model.currentRunEndDate,
                CurrentRunDate = DateTime.Today.ToShortDateString(),
                CurrentRunUser = Environment.UserName,
                CurrentRunStatus = "1"
            };
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string OperationPath = configuration["OperationFolder"];
            string OperationFile = Path.Combine(OperationPath, "Load837Facets", "Op" + DateTime.Today.ToString("yyyyMMdd") + ".json");
            System.IO.File.WriteAllText(OperationFile, JsonConvert.SerializeObject(runStatus, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            model.newFiles = new List<string>();
            model.newFiles.Add(model.currentRunStartDate);
            model.newFiles.Add(model.currentRunEndDate);
            model.newFiles.Add(model.LoadCMSP.ToString());
            model.newFiles.Add(model.LoadCMSI.ToString());
            model.newFiles.Add(model.LoadDualP.ToString());
            model.newFiles.Add(model.LoadDualI.ToString());
            model.newFiles.Add(model.LoadStateP.ToString());
            model.newFiles.Add(model.LoadStateI.ToString());
            model.newFiles.Add(model.DateRange.ToString());
            model.newFiles.Add(model.Include.ToString());
            model.newFiles.Add(model.Exclude.ToString());
            Uri serviceName = new Uri($"{this.serviceContext.CodePackageActivationContext.ApplicationName}/Load837Facets");
            Uri proxyAddress = this.GetProxyAddress(serviceName);
            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);
            foreach (Partition partition in partitions)
            {
                string proxyUrl = $"{proxyAddress}/Load837Facets?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";
                HttpContent content = new StringContent(JsonConvert.SerializeObject(model.newFiles), Encoding.UTF8, "application/json");
                using (HttpResponseMessage response = await this.httpClient.PostAsync(proxyUrl, content))
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        continue;
                    }
                    model.newFiles = JsonConvert.DeserializeObject<List<string>>(await response.Content.ReadAsStringAsync());
                }
            }

            return View(model);
        }
        public async Task<IActionResult> LoadVoidsReplacements() 
        {
            ExportFilesModel model = new ExportFilesModel();
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string OperationPath = configuration["OperationFolder"];
            string OperationFile = Path.Combine(OperationPath, "LoadVoidsReplacement", "Op" + DateTime.Today.ToString("yyyyMMdd") + ".json");
            model.enableSubmit = true;
            if (System.IO.File.Exists(OperationFile))
            {
                RunStatus_LoadFileModel runStatus = JsonConvert.DeserializeObject<RunStatus_LoadFileModel>(System.IO.File.ReadAllText(OperationFile));
                if (runStatus.CurrentRunStatus == "1")
                {
                    model.displayMessage = "This process has been initiated by " + runStatus.CurrentRunUser;
                    model.enableSubmit = false;
                }
            }

            Uri serviceName = new Uri($"{this.serviceContext.CodePackageActivationContext.ApplicationName}/LoadVoidsReplacements");
            Uri proxyAddress = this.GetProxyAddress(serviceName);
            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);
            foreach (Partition partition in partitions)
            {
                string proxyUrl = $"{proxyAddress}/LoadVoidsReplacements?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";

                using (HttpResponseMessage response = await this.httpClient.GetAsync(proxyUrl))
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        continue;
                    }
                    model.newFiles = JsonConvert.DeserializeObject<List<string>>(await response.Content.ReadAsStringAsync());
                }
            }
            if (model.newFiles != null)
            {
                model.sourcePath = model.newFiles[0];
                model.archivePath = model.newFiles[1];
                model.newFiles.RemoveRange(0, 2);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> LoadVoidsReplacements(ExportFilesModel model)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string OperationPath = configuration["OperationFolder"];
            string OperationFile = Path.Combine(OperationPath, "LoadVoidsReplacements", "Op" + DateTime.Today.ToString("yyyyMMdd") + ".json");
            RunStatus_LoadFileModel runStatus;
            if (System.IO.File.Exists(OperationFile))
            {
                runStatus = JsonConvert.DeserializeObject<RunStatus_LoadFileModel>(System.IO.File.ReadAllText(OperationFile));
                runStatus.CurrentRunStatus = "1";
            }
            else
            {
                runStatus = new RunStatus_LoadFileModel
                {
                    CurrentRunUser = Environment.UserName,
                    CurrentRunDate = DateTime.Today.ToShortDateString(),
                    CurrentRunStatus = "1",
                    FileNames = new List<string>()
                };
            }
            System.IO.File.WriteAllText(OperationFile, JsonConvert.SerializeObject(runStatus, Formatting.Indented, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            model.newFiles = new List<string>();
            Uri serviceName = new Uri($"{this.serviceContext.CodePackageActivationContext.ApplicationName}/LoadVoidsReplacements");
            Uri proxyAddress = this.GetProxyAddress(serviceName);
            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);
            foreach (Partition partition in partitions)
            {
                string proxyUrl = $"{proxyAddress}/LoadVoidsReplacements/1/?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";

                using (HttpResponseMessage response = await this.httpClient.GetAsync(proxyUrl))
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        continue;
                    }
                    List<string> result = JsonConvert.DeserializeObject<List<string>>(await response.Content.ReadAsStringAsync());
                    model.totalFiles = result[0];
                    model.goodFiles = result[1];
                    break;
                }
            }
            if (!string.IsNullOrEmpty(model.totalFiles))
            {
                model.displayMessage = $"Total files: {model.totalFiles }, processed files: {model.goodFiles}";
            }
            else
            {
                model.displayMessage = "Loading is in progress...";
            }
            return View(model);
        }
        private Uri GetProxyAddress(Uri serviceName)
        {
            return new Uri($"http://localhost:19083{serviceName.AbsolutePath}");
        }
    }
}
