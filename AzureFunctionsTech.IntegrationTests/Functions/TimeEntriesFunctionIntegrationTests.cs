using AzureFunctionsTech.Api.Functions;
using AzureFunctionsTech.IntegrationTests.Fixtures;

namespace AzureFunctionsTech.IntegrationTests.Functions
{
    public class TimeEntriesFunctionIntegrationTests : IClassFixture<TimeEntriesFunctionFixture>
    {
        private readonly TimeEntriesFunctionFixture _timeEntriesFunctionFixture;

        public TimeEntriesFunctionIntegrationTests(TimeEntriesFunctionFixture timeEntriesFunctionFixture)
        {
            _timeEntriesFunctionFixture = timeEntriesFunctionFixture;
        }

        [Fact]
        public async void PostRequest_InvalidRequest_ShouldReturnBadRequest()
        {
            var request = _timeEntriesFunctionFixture.GetValidRequestBody();
            var response = await TimeEntriesFunction.Run(request, );
        }
    }
}