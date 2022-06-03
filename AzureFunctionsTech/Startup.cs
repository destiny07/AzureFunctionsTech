using AzureFunctionsTech.Api.Services;
using MediatR;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.PowerPlatform.Dataverse.Client;
using System;
using static AzureFunctionsTech.Api.Core.TimeEntries.CreateTimeEntriesCommand;

[assembly: FunctionsStartup(typeof(AzureFunctionsTech.Api.Startup))]
namespace AzureFunctionsTech.Api
{

    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddMediatR(typeof(CreateTimeEntriesCommandHandler));
            builder.Services.AddScoped<IConfigurationService, EnvironmentConfigurationService>();

            var dataverseConnectionString = Environment.GetEnvironmentVariable("Connection");
            builder.Services.AddScoped<IDataverseService>(x => new DataverseService(
                new ServiceClient(dataverseConnectionString)));
        }
    }
}
