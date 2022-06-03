using AzureFunctionsTech.Api.Services.Messages;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureFunctionsTech.Api.Services
{
    public interface IDataverseService
    {
        Task<int> AddTimeEntriesAsync(IEnumerable<TimeEntryServiceMessage> timeEntries);
        Task<bool> IsTimeEntryExistsAsync(DateTime datetime);
    }
}
