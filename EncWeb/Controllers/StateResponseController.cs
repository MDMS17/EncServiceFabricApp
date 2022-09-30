using EncWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Fabric.Query;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace EncWeb.Controllers
{
    public class StateResponseController : Controller
    {
        private readonly HttpClient httpClient;
        private readonly FabricClient fabricClient;
        private readonly StatelessServiceContext serviceContext;
        public StateResponseController(HttpClient _httpClient, FabricClient _fabricClient, StatelessServiceContext _serviceContext) 
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
        public async Task<IActionResult> DHCSEVR()
        {
            NewFilesModel model = new NewFilesModel();
            Uri serviceName = new Uri($"{this.serviceContext.CodePackageActivationContext.ApplicationName}/DHCSEVR");
            Uri proxyAddress = this.GetProxyAddress(serviceName);
            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);
            foreach (Partition partition in partitions)
            {
                string proxyUrl = $"{proxyAddress}/DHCSEVR?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";

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
        public async Task<IActionResult> DHCSEVR(NewFilesModel model)
        {
            model.newFiles = new List<string>();
            Uri serviceName = new Uri($"{this.serviceContext.CodePackageActivationContext.ApplicationName}/DHCSEVR");
            Uri proxyAddress = this.GetProxyAddress(serviceName);
            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);
            foreach (Partition partition in partitions)
            {
                string proxyUrl = $"{proxyAddress}/DHCSEVR/1?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";

                using (HttpResponseMessage response = await this.httpClient.GetAsync(proxyUrl))
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        continue;
                    }
                    Tuple<int, int> result = JsonConvert.DeserializeObject<Tuple<int, int>>(await response.Content.ReadAsStringAsync());
                    model.totalFiles = result.Item1;
                    model.goodFiles = result.Item2;
                    break;
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Premium820()
        {
            NewFilesModel model = new NewFilesModel();
            Uri serviceName = new Uri($"{this.serviceContext.CodePackageActivationContext.ApplicationName}/Premium820");
            Uri proxyAddress = this.GetProxyAddress(serviceName);
            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);
            foreach (Partition partition in partitions)
            {
                string proxyUrl = $"{proxyAddress}/Premium820?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";

                using (HttpResponseMessage response = await this.httpClient.GetAsync(proxyUrl))
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        continue;
                    }
                    model.newFiles = JsonConvert.DeserializeObject<List<string>>(await response.Content.ReadAsStringAsync());
                }
            }
            model.sourcePath = model.newFiles[0];
            model.archivePath = model.newFiles[1];
            model.newFiles.RemoveRange(0, 2);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Premium820(NewFilesModel model)
        {
            model.newFiles = new List<string>();
            Uri serviceName = new Uri($"{this.serviceContext.CodePackageActivationContext.ApplicationName}/Premium820");
            Uri proxyAddress = this.GetProxyAddress(serviceName);
            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);
            foreach (Partition partition in partitions)
            {
                string proxyUrl = $"{proxyAddress}/Premium820/1?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";

                using (HttpResponseMessage response = await this.httpClient.GetAsync(proxyUrl))
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        continue;
                    }
                    Tuple<int, int> result = JsonConvert.DeserializeObject<Tuple<int, int>>(await response.Content.ReadAsStringAsync());
                    model.totalFiles = result.Item1;
                    model.goodFiles = result.Item2;
                    break;
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Eligibility834()
        {
            NewFilesModel model = new NewFilesModel();
            Uri serviceName = new Uri($"{this.serviceContext.CodePackageActivationContext.ApplicationName}/Eligibility834");
            Uri proxyAddress = this.GetProxyAddress(serviceName);
            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);
            foreach (Partition partition in partitions)
            {
                string proxyUrl = $"{proxyAddress}/Eligibility834?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";

                using (HttpResponseMessage response = await this.httpClient.GetAsync(proxyUrl))
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        continue;
                    }
                    model.newFiles = JsonConvert.DeserializeObject<List<string>>(await response.Content.ReadAsStringAsync());
                    break;
                }
            }
            model.sourcePath = model.newFiles[0];
            model.archivePath = model.newFiles[1];
            model.newFiles.RemoveRange(0, 2);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Eligibility834(NewFilesModel model)
        {
            model.newFiles = new List<string>();
            Uri serviceName = new Uri($"{this.serviceContext.CodePackageActivationContext.ApplicationName}/Eligibility834");
            Uri proxyAddress = this.GetProxyAddress(serviceName);
            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);
            foreach (Partition partition in partitions)
            {
                string proxyUrl = $"{proxyAddress}/Eligibility834/1?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";

                using (HttpResponseMessage response = await this.httpClient.GetAsync(proxyUrl))
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        continue;
                    }
                    Tuple<int, int> result = JsonConvert.DeserializeObject<Tuple<int, int>>(await response.Content.ReadAsStringAsync());
                    model.totalFiles = result.Item1;
                    model.goodFiles = result.Item2;
                    break;
                }
            }
            return View(model);
        }
        public async Task<IActionResult> Remittance835()
        {
            NewFilesModel model = new NewFilesModel();
            Uri serviceName = new Uri($"{this.serviceContext.CodePackageActivationContext.ApplicationName}/Remittance835");
            Uri proxyAddress = this.GetProxyAddress(serviceName);
            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);
            foreach (Partition partition in partitions)
            {
                string proxyUrl = $"{proxyAddress}/Remittance835?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";

                using (HttpResponseMessage response = await this.httpClient.GetAsync(proxyUrl))
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        continue;
                    }
                    model.newFiles = JsonConvert.DeserializeObject<List<string>>(await response.Content.ReadAsStringAsync());
                }
            }
            model.sourcePath = model.newFiles[0];
            model.archivePath = model.newFiles[1];
            model.newFiles.RemoveRange(0, 2);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> Remittance835(NewFilesModel model)
        {
            model.newFiles = new List<string>();
            Uri serviceName = new Uri($"{this.serviceContext.CodePackageActivationContext.ApplicationName}/Remittance835");
            Uri proxyAddress = this.GetProxyAddress(serviceName);
            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);
            foreach (Partition partition in partitions)
            {
                string proxyUrl = $"{proxyAddress}/Remittance835/1?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";

                using (HttpResponseMessage response = await this.httpClient.GetAsync(proxyUrl))
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        continue;
                    }
                    Tuple<int, int> result = JsonConvert.DeserializeObject<Tuple<int, int>>(await response.Content.ReadAsStringAsync());
                    model.totalFiles = result.Item1;
                    model.goodFiles = result.Item2;
                    break;
                }
            }
            return View(model);
        }
        private Uri GetProxyAddress(Uri serviceName)
        {
            return new Uri($"http://localhost:19081{serviceName.AbsolutePath}");
        }
    }
}
