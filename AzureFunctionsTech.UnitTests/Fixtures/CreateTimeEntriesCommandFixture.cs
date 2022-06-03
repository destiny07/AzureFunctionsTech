using AzureFunctionsTech.Api.Core.TimeEntries;

namespace AzureFunctionsTech.UnitTests.Fixtures
{
    public class CreateTimeEntriesCommandFixture
    {
        public CreateTimeEntriesCommand GetDefaultCreateTimeEntriesRequest()
        {
            return GetRequestWithDateIntervalRequest(0);
        }

        public CreateTimeEntriesCommand GetRequestWithDateIntervalRequest(int interval)
        {
            return new CreateTimeEntriesCommand(DateTime.Now, DateTime.Now.Add(TimeSpan.FromDays(interval)));
        }

        public CreateTimeEntriesCommand GetEndDateEarlierThanStartDateRequest()
        {
            return new CreateTimeEntriesCommand(DateTime.Now, DateTime.Now.Subtract(TimeSpan.FromDays(1)));
        }
    }
}
