namespace AzureFunctionsTech.Api.Services
{
    public interface IConfigurationService
    {
        public string GetConfiguration(string configurationName);
    }
}
