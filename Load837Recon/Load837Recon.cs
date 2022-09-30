using System;
using System.Collections.Generic;
using System.Fabric;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Data;
using Microsoft.Extensions.Configuration;
using Load837Recon.Data;
using Microsoft.EntityFrameworkCore;

namespace Load837Recon
{
    /// <summary>
    /// The FabricRuntime creates an instance of this class for each service type instance. 
    /// </summary>
    internal sealed class Load837Recon : StatefulService
    {
        public Load837Recon(StatefulServiceContext context)
            : base(context)
        { }

        /// <summary>
        /// Optional override to create listeners (like tcp, http) for this service instance.
        /// </summary>
        /// <returns>The collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
            string cn = configuration.GetConnectionString("SubHistoryContext");
            string cn_CMS277CA = configuration.GetConnectionString("CMS277CAContext");
            string cn_CMSMAO002 = configuration.GetConnectionString("CMSMAO002Context");
            string cn_DHCS = configuration.GetConnectionString("DHCSContext");
            return new ServiceReplicaListener[]
            {
                new ServiceReplicaListener(serviceContext =>
                    new KestrelCommunicationListener(serviceContext, (url, listener) =>
                    {
                        ServiceEventSource.Current.ServiceMessage(serviceContext, $"Starting Kestrel on {url}");

                        return new WebHostBuilder()
                                    .UseKestrel()
                                    .ConfigureServices(
                                        services => services
                                            .AddSingleton<StatefulServiceContext>(serviceContext)
                                            .AddDbContext<SubHistoryContext>(options=>options.UseSqlServer(cn))
                                            .AddDbContext<ReconContext>(options=>options.UseSqlServer(cn))
                                            .AddDbContext<CMS277CAContext>(options=>options.UseSqlServer(cn_CMS277CA))
                                            .AddDbContext<CMSMAO002Context>(options=>options.UseSqlServer(cn_CMSMAO002))
                                            .AddDbContext<DHCSContext>(options=>options.UseSqlServer(cn_DHCS))
                                            .AddSingleton<IReliableStateManager>(this.StateManager))
                                    .UseContentRoot(Directory.GetCurrentDirectory())
                                    .UseStartup<Startup>()
                                    .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.UseUniqueServiceUrl)
                                    .UseUrls(url)
                                    .Build();
                    }))
            };
        }
    }
}
