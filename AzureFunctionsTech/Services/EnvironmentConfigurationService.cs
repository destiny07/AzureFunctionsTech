using System;

namespace AzureFunctionsTech.Api.Services
{
    public class EnvironmentConfigurationService : IConfigurationService
    {
        public string GetConfiguration(string configurationName)
        {
            return Environment.GetEnvironmentVariable(
                configurationName, EnvironmentVariableTarget.Process);
        }
    }
}
