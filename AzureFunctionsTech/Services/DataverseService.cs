using AzureFunctionsTech.Api.Services.Exceptions;
using AzureFunctionsTech.Api.Services.Messages;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureFunctionsTech.Api.Services
{
    public class DataverseService : IDataverseService
    {
        private readonly ServiceClient _serviceClient;

        public DataverseService(ServiceClient serviceClient)
        {
            _serviceClient = serviceClient;
        }

        public async Task<int> AddTimeEntriesAsync(IEnumerable<TimeEntryServiceMessage> timeEntries)
        {
            if (!_serviceClient.IsReady)
                throw new DataverserServiceException("Error connecting to dataverse");

            var entityCollection = ConvertToEntityCollection(timeEntries);

            var request = new ExecuteMultipleRequest()
            {
                Settings = new ExecuteMultipleSettings()
                {
                    ContinueOnError = false,
                    ReturnResponses = true
                },
                Requests = new OrganizationRequestCollection()
            };

            foreach (var entity in entityCollection.Entities)
            {
                var createRequest = new CreateRequest { Target = entity };
                request.Requests.Add(createRequest);
            }

            var response =
                (ExecuteMultipleResponse)await _serviceClient.ExecuteAsync(request);

            if (response.IsFaulted)
                throw new DataverserServiceException("Error creating time entries");

            return response.Responses.Count;
        }

        public async Task<bool> IsTimeEntryExistsAsync(DateTime datetime)
        {
            var request = new QueryByAttribute
            {
                EntityName = "msdyn_timeentry",
                ColumnSet = new ColumnSet("msdyn_start", "msdyn_end"),
                Attributes = { "msdyn_start", "msdyn_end" },
                Values = { datetime, datetime }
            };

            var response = await _serviceClient.RetrieveMultipleAsync(request);
            return response.Entities.Count > 0;
        }

        private EntityCollection ConvertToEntityCollection(IEnumerable<TimeEntryServiceMessage> timeEntries)
        {
            var entries = timeEntries.Select(x =>
            {
                var entity = new Entity("msdyn_timeentry");
                entity["msdyn_start"] = x.Start;
                entity["msdyn_end"] = x.End;

                return entity;
            }).ToList();

            return new EntityCollection(entries);
        }
    }
}
