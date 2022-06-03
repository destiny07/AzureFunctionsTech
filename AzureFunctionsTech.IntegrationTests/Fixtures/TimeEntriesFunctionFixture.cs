using Newtonsoft.Json;

namespace AzureFunctionsTech.IntegrationTests.Fixtures
{
    public class TimeEntriesFunctionFixture
    {
        public string GetValidRequestBody()
        {
            var requestBody = new
            {
                StartOn = DateTime.Now,
                EndOn = DateTime.Now
            };
            return JsonConvert.SerializeObject(requestBody);
        }
    }
}
