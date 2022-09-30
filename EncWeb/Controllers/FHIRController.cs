using EncWeb.Models;
using FHIR.DataModel.Administration;
using FHIR.DataModel.Financial;
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
    public class FHIRController : Controller
    {
        private readonly HttpClient httpClient;
        private readonly FabricClient fabricClient;
        private readonly StatelessServiceContext serviceContext;
        public FHIRController(HttpClient _httpClient, FabricClient _fabricClient, StatelessServiceContext _serviceContext)
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
        public async Task<IActionResult> MembersModule() 
        {
            FHIRMemberModel model = new FHIRMemberModel();
            model.ShowLogin = "display:block;";
            model.ShowVerification = "display:none;";
            return View(model);
        }
        public async Task<IActionResult> ProvidersModule() 
        {
            FHIRProviderModel model = new FHIRProviderModel();
            model.ShowLogin = "display:block;";
            model.ShowVerification = "display:none;";
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> MembersModule(FHIRMemberModel model)
        {
            if (string.IsNullOrEmpty(model.VerificationCode)) 
            {
                model.ShowLogin = "display:block;";
                model.ShowVerification = "display:block;";
                return View(model);
            }
            Uri serviceName = new Uri($"{this.serviceContext.CodePackageActivationContext.ApplicationName}/FHIRMembers");
            Uri proxyAddress = this.GetProxyAddress(serviceName);
            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);
            model.Parameters = new List<string>();
            model.Parameters.Add(model.UserId);
            foreach (Partition partition in partitions)
            {
                string proxyUrl = $"{proxyAddress}/FHIRMembers?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";
                HttpContent content = new StringContent(JsonConvert.SerializeObject(model.Parameters), Encoding.UTF8, "application/json");
                using (HttpResponseMessage response = await this.httpClient.PostAsync(proxyUrl, content))
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        continue;
                    }
                    model.patient = JsonConvert.DeserializeObject<Patient>(await response.Content.ReadAsStringAsync());
                    break;
                }
            }
            model.ShowLogin = "display:none;";
            if (model.patient == null)
            {
                model.Message = "No record found!";
                model.showMember = "display:none;";
            }
            else 
            {
                model.showMember = "display:block;";
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> FHIRClaims(FHIRMemberModel modelMember) 
        {
            FHIRClaimModel model = new FHIRClaimModel();
            Uri serviceName = new Uri($"{this.serviceContext.CodePackageActivationContext.ApplicationName}/FHIRClaims");
            Uri proxyAddress = this.GetProxyAddress(serviceName);
            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);
            model.Parameters = new List<string>();
            model.Parameters.Add("MemberId");
            model.Parameters.Add(modelMember.UserId);
            foreach (Partition partition in partitions)
            {
                string proxyUrl = $"{proxyAddress}/FHIRClaims?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";
                HttpContent content = new StringContent(JsonConvert.SerializeObject(model.Parameters), Encoding.UTF8, "application/json");
                using (HttpResponseMessage response = await this.httpClient.PostAsync(proxyUrl, content))
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        continue;
                    }
                    model.claim = JsonConvert.DeserializeObject<Claim>(await response.Content.ReadAsStringAsync());
                    break;
                }
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> ProvidersModule(FHIRProviderModel model)
        {
            if (string.IsNullOrEmpty(model.VerificationCode))
            {
                model.ShowLogin = "display:block;";
                model.ShowVerification = "display:block;";
                return View(model);
            }
            Uri serviceName = new Uri($"{this.serviceContext.CodePackageActivationContext.ApplicationName}/FHIRProviders");
            Uri proxyAddress = this.GetProxyAddress(serviceName);
            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);
            model.Parameters = new List<string>();
            model.Parameters.Add(model.UserId);
            foreach (Partition partition in partitions)
            {
                string proxyUrl = $"{proxyAddress}/FHIRProviders?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";
                HttpContent content = new StringContent(JsonConvert.SerializeObject(model.Parameters), Encoding.UTF8, "application/json");
                using (HttpResponseMessage response = await this.httpClient.PostAsync(proxyUrl, content))
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        continue;
                    }
                    model.practitioner = JsonConvert.DeserializeObject<Practitioner>(await response.Content.ReadAsStringAsync());
                    break;
                }
            }
            model.ShowLogin = "display:none;";
            if (model.practitioner == null)
            {
                model.Message = "No record found!";
                model.showProvider = "display:none;";
            }
            else
            {
                model.showProvider = "display:block;";
            }
            return View(model);
        }
        [HttpPost]
        public async Task<IActionResult> FHIRProviderClaims(FHIRProviderModel modelProvider) 
        {
            FHIRClaimModel model = new FHIRClaimModel();
            Uri serviceName = new Uri($"{this.serviceContext.CodePackageActivationContext.ApplicationName}/FHIRClaims");
            Uri proxyAddress = this.GetProxyAddress(serviceName);
            ServicePartitionList partitions = await this.fabricClient.QueryManager.GetPartitionListAsync(serviceName);
            model.Parameters = new List<string>();
            model.Parameters.Add("ProviderId");
            model.Parameters.Add(modelProvider.UserId);
            foreach (Partition partition in partitions)
            {
                string proxyUrl = $"{proxyAddress}/FHIRClaims?PartitionKey={((Int64RangePartitionInformation)partition.PartitionInformation).LowKey}&PartitionKind=Int64Range";
                HttpContent content = new StringContent(JsonConvert.SerializeObject(model.Parameters), Encoding.UTF8, "application/json");
                using (HttpResponseMessage response = await this.httpClient.PostAsync(proxyUrl, content))
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        continue;
                    }
                    model.claim = JsonConvert.DeserializeObject<Claim>(await response.Content.ReadAsStringAsync());
                    break;
                }
            }
            return View(model);
        }
        private Uri GetProxyAddress(Uri serviceName)
        {
            return new Uri($"http://localhost:19089{serviceName.AbsolutePath}");
        }
    }
}
