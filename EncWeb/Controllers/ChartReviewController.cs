using EncWeb.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Fabric;
using System.Fabric.Query;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace EncWeb.Controllers
{
    public class ChartReviewController : Controller
    {
        private readonly HttpClient httpClient;
        private readonly FabricClient fabricClient;
        private readonly StatelessServiceContext serviceContext;
        public ChartReviewController(HttpClient _httpClient, FabricClient _fabricClient, StatelessServiceContext _serviceContext)
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
        public async Task<IActionResult> LoadChartReviewCsv()
        {
            ExportFilesModel model = new ExportFilesModel();
            Uri serviceName = new Uri($"{this.serviceContext.CodePackageActivationContext.ApplicationName}/LoadChartReviewCsv");
            Uri proxyAddress = this.GetProxyAddress(serviceName);
            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);
            foreach (Partition partition in partitions)
            {
                string proxyUrl = $"{proxyAddress}/LoadChartReviewCsv?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";

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
        public async Task<IActionResult> LoadChartReviewCsv(ExportFilesModel model)
        {
            model.newFiles = new List<string>();
            Uri serviceName = new Uri($"{this.serviceContext.CodePackageActivationContext.ApplicationName}/LoadChartReviewCsv");
            Uri proxyAddress = this.GetProxyAddress(serviceName);
            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);
            foreach (Partition partition in partitions)
            {
                string proxyUrl = $"{proxyAddress}/LoadChartReviewCsv/{model.ProdTestFlag}?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";

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
        public async Task<IActionResult> GenerateChartReview()
        {
            ExportFilesModel model = new ExportFilesModel();
            Uri serviceName = new Uri($"{this.serviceContext.CodePackageActivationContext.ApplicationName}/GenerateChartReview");
            Uri proxyAddress = this.GetProxyAddress(serviceName);
            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);
            foreach (Partition partition in partitions)
            {
                string proxyUrl = $"{proxyAddress}/GenerateChartReview?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";

                using (HttpResponseMessage response = await this.httpClient.GetAsync(proxyUrl))
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        continue;
                    }
                    model.newFiles = JsonConvert.DeserializeObject<List<string>>(await response.Content.ReadAsStringAsync());
                }
            }
            model.totalDatabaseRecords = model.newFiles[0];
            model.sourcePath = model.newFiles[1];
            model.newFiles.RemoveRange(0, 2);
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> GenerateChartReview(ExportFilesModel model)
        {
            model.newFiles = new List<string>();
            Uri serviceName = new Uri($"{this.serviceContext.CodePackageActivationContext.ApplicationName}/GenerateChartReview");
            Uri proxyAddress = this.GetProxyAddress(serviceName);
            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);
            foreach (Partition partition in partitions)
            {
                string proxyUrl = $"{proxyAddress}/GenerateChartReview/1?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";

                using (HttpResponseMessage response = await this.httpClient.GetAsync(proxyUrl))
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        continue;
                    }
                    List<string> result = JsonConvert.DeserializeObject<List<string>>(await response.Content.ReadAsStringAsync());
                    model.totalDatabaseRecords = result[0];
                    model.goodDatabaseRecords = result[1];
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
