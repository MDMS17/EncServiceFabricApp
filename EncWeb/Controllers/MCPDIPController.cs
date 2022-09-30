using EncWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Fabric.Query;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EncWeb.Controllers
{
    public class MCPDIPController : Controller
    {
        private readonly HttpClient httpClient;
        private readonly FabricClient fabricClient;
        private readonly StatelessServiceContext serviceContext;
        public MCPDIPController(HttpClient _httpClient, FabricClient _fabricClient, StatelessServiceContext _serviceContext)
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
        public async Task<IActionResult> LoadMCPDIPFile()
        {
            TwoTiersFilesModel model = new TwoTiersFilesModel();
            model.twoTiersFiles = new List<Tuple<string, string, string, string, string>>();
            Uri serviceName = new Uri($"{this.serviceContext.CodePackageActivationContext.ApplicationName}/LoadMCPDIPExcel");
            Uri proxyAddress = this.GetProxyAddress(serviceName);
            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);
            List<String> items = new List<string>();
            foreach (Partition partition in partitions)
            {
                string proxyUrl = $"{proxyAddress}/LoadMCPDIPExcel?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";

                using (HttpResponseMessage response = await this.httpClient.GetAsync(proxyUrl))
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        continue;
                    }
                    items = JsonConvert.DeserializeObject<List<string>>(await response.Content.ReadAsStringAsync());
                }
            }
            if (items.Count>0) 
            {
                model.sourcePath = items[0];
                model.archivePath = items[1];
                List<Tuple<string, string>> files = new List<Tuple<string, string>>();
                for (int i = 2; i < items.Count; i++) 
                {
                    string[] item = items[i].Split('~');
                    files.Add(Tuple.Create(item[0], item[1]));
                }
                List<Tuple<string,int>> fileCounts = files.GroupBy(x => x.Item1).Select(x => Tuple.Create(x.Key,x.Count())).ToList();
                for (int i= 0; i<files.Count;i++) 
                {
                    model.twoTiersFiles.Add(Tuple.Create(files[i].Item1, files[i].Item2, "Header" + i.ToString(), "Item" + i.ToString(), fileCounts.FirstOrDefault(x => x.Item1 == files[i].Item1).Item2.ToString()));
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> LoadMCPDIPFile(TwoTiersFilesModel model)
        {
            model.twoTiersFiles = new List<Tuple<string,string,string,string,string>>();
            Uri serviceName = new Uri($"{this.serviceContext.CodePackageActivationContext.ApplicationName}/LoadMCPDIPExcel");
            Uri proxyAddress = this.GetProxyAddress(serviceName);
            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);
            foreach (Partition partition in partitions)
            {
                string proxyUrl = $"{proxyAddress}/LoadMCPDIPExcel/1?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";

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
        public async Task<IActionResult> LoadMCPDIPResponse()
        {
            ExportFilesModel model = new ExportFilesModel();
            Uri serviceName = new Uri($"{this.serviceContext.CodePackageActivationContext.ApplicationName}/LoadMCPDIPResponse");
            Uri proxyAddress = this.GetProxyAddress(serviceName);
            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);
            foreach (Partition partition in partitions)
            {
                string proxyUrl = $"{proxyAddress}/LoadMCPDIPResponse?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";

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
        public async Task<IActionResult> LoadMCPDIPResponse(ExportFilesModel model)
        {
            model.newFiles = new List<string>();
            Uri serviceName = new Uri($"{this.serviceContext.CodePackageActivationContext.ApplicationName}/LoadMCPDIPResponse");
            Uri proxyAddress = this.GetProxyAddress(serviceName);
            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);
            foreach (Partition partition in partitions)
            {
                string proxyUrl = $"{proxyAddress}/LoadMCPDIPResponse/1?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";

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
        public async Task<IActionResult> GenerateMCPDJson()
        {
            ExportFilesModel model = new ExportFilesModel();
            Uri serviceName = new Uri($"{this.serviceContext.CodePackageActivationContext.ApplicationName}/GenerateMCPDJson");
            Uri proxyAddress = this.GetProxyAddress(serviceName);
            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);
            foreach (Partition partition in partitions)
            {
                string proxyUrl = $"{proxyAddress}/GenerateMCPDJson?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";

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
                model.totalDatabaseRecords = model.newFiles[0];
                model.sourcePath = model.newFiles[1];
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> GenerateMCPDJson(ExportFilesModel model)
        {
            model.newFiles = new List<string>();
            List<String> result = null;
            Uri serviceName = new Uri($"{this.serviceContext.CodePackageActivationContext.ApplicationName}/GenerateMCPDJson");
            Uri proxyAddress = this.GetProxyAddress(serviceName);
            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);
            foreach (Partition partition in partitions)
            {
                string proxyUrl = $"{proxyAddress}/GenerateMCPDJson/{model.ProdTestFlag}?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";

                using (HttpResponseMessage response = await this.httpClient.GetAsync(proxyUrl))
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        continue;
                    }
                    result = JsonConvert.DeserializeObject<List<string>>(await response.Content.ReadAsStringAsync());
                    break;
                }
            }
            model.totalDatabaseRecords = model.goodDatabaseRecords;
            if (result == null) 
            { 
                model.archivePath = "Process failed!"; 
            }
            else
            {
                model.archivePath = "Process succeeded!";
            }
            return View(model);
        }
        public async Task<IActionResult> GeneratePCPAJson()
        {
            ExportFilesModel model = new ExportFilesModel();
            Uri serviceName = new Uri($"{this.serviceContext.CodePackageActivationContext.ApplicationName}/GeneratePCPAJson");
            Uri proxyAddress = this.GetProxyAddress(serviceName);
            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);
            foreach (Partition partition in partitions)
            {
                string proxyUrl = $"{proxyAddress}/GeneratePCPAJson?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";

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
                model.totalDatabaseRecords = model.newFiles[0];
                model.sourcePath = model.newFiles[1];
                model.newFiles.RemoveRange(0, 2);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> GeneratePCPAJson(ExportFilesModel model)
        {
            model.newFiles = new List<string>();
            List<String> result = null;
            Uri serviceName = new Uri($"{this.serviceContext.CodePackageActivationContext.ApplicationName}/GeneratePCPAJson");
            Uri proxyAddress = this.GetProxyAddress(serviceName);
            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);
            foreach (Partition partition in partitions)
            {
                string proxyUrl = $"{proxyAddress}/GeneratePCPAJson/{model.ProdTestFlag}?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";

                using (HttpResponseMessage response = await this.httpClient.GetAsync(proxyUrl))
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        continue;
                    }
                    result = JsonConvert.DeserializeObject<List<string>>(await response.Content.ReadAsStringAsync());
                    break;
                }
            }
            if (result == null) model.archivePath = "Process failed!";
            else model.archivePath = "Process succeeded!";
            return View(model);
        }
        public async Task<IActionResult> ValidateMCPDIPJsonFile()
        {
            MCPDIPJsonFileValidationModel model = new MCPDIPJsonFileValidationModel();
            Uri serviceName = new Uri($"{this.serviceContext.CodePackageActivationContext.ApplicationName}/ValidateMCPDIPJsonFile");
            Uri proxyAddress = this.GetProxyAddress(serviceName);
            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);
            foreach (Partition partition in partitions)
            {
                string proxyUrl = $"{proxyAddress}/ValidateMCPDIPJsonFile?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";
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
                model.SelectedFiles = new List<Tuple<bool, string, string, string, string>>();
                foreach (var item in model.newFiles) 
                {
                    model.SelectedFiles.Add(Tuple.Create(false, item, "", "", ""));
                }
                model.AllFiles = string.Join('~', model.newFiles);
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ValidateMCPDIPJsonFile(MCPDIPJsonFileValidationModel model)
        {
            model.newFiles = new List<string>();
            model.SelectedFiles = new List<Tuple<bool, string, string, string, string>>();
            string[] allFiles = model.AllFiles.Split('~');
            string[] sequences = model.SelectedSequences.Split(',');
            for (int i = 0; i < allFiles.Length; i++) 
            {
                if (sequences[i] == "1") model.newFiles.Add(allFiles[i]);
            }
            Uri serviceName = new Uri($"{this.serviceContext.CodePackageActivationContext.ApplicationName}/ValidateMCPDIPJsonFile");
            Uri proxyAddress = this.GetProxyAddress(serviceName);
            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);
            foreach (Partition partition in partitions)
            {
                string proxyUrl = $"{proxyAddress}/ValidateMCPDIPJsonFile?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";
                HttpContent content = new StringContent(JsonConvert.SerializeObject(model.newFiles), Encoding.UTF8, "application/json");
                using (HttpResponseMessage response = await this.httpClient.PostAsync(proxyUrl,content))
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        continue;
                    }
                    List<string> result = JsonConvert.DeserializeObject<List<string>>(await response.Content.ReadAsStringAsync());
                    model.sourcePath = result[0];
                    result.RemoveRange(0, 1);
                    for (int i = 0; i < allFiles.Length; i++) 
                    {
                        if (sequences[i] == "0")
                        {
                            model.SelectedFiles.Add(Tuple.Create(false, allFiles[i], "", "", ""));
                        }
                        else 
                        {
                            if (string.IsNullOrEmpty(result[i]))
                            {
                                model.SelectedFiles.Add(Tuple.Create(true, allFiles[i], "Passed", "", ""));
                            }
                            else 
                            {
                                model.SelectedFiles.Add(Tuple.Create(true, allFiles[i], "Failed", "item" + i.ToString(), result[i]));
                            }
                        }
                    }
                    break;
                }
            }
            return View(model);
        }
        private Uri GetProxyAddress(Uri serviceName)
        {
            return new Uri($"http://localhost:19085{serviceName.AbsolutePath}");
        }
    }
}
