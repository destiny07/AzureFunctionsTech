using AzureFunctionsTech.Api.Core.Command;
using AzureFunctionsTech.Api.Services;
using AzureFunctionsTech.Api.Services.Exceptions;
using AzureFunctionsTech.Api.Services.Messages;
using MediatR;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace AzureFunctionsTech.Api.Core.TimeEntries
{
    public class CreateTimeEntriesCommand : IRequest<CommandResult<int>>
    {
        public CreateTimeEntriesCommand(DateTime startTime, DateTime endTime)
        {
            StartOn = startTime;
            EndOn = endTime;
        }

        public DateTime StartOn { get; }
        public DateTime EndOn { get; }

        public class CreateTimeEntriesCommandHandler : IRequestHandler<CreateTimeEntriesCommand, CommandResult<int>>
        {
            private readonly IDataverseService _dataverseService;

            public CreateTimeEntriesCommandHandler(IDataverseService dataverseService)
            {
                _dataverseService = dataverseService;
            }

            public async Task<CommandResult<int>> Handle(CreateTimeEntriesCommand request, CancellationToken cancellationToken)
            {
                var startOn = request.StartOn;
                var endOn = request.EndOn;

                if (startOn > endOn)
                {
                    return CommandResultFactory.GetErrorResult<int>(
                        CommandResultCode.InvalidArgument, $"{nameof(request.StartOn)} cannot be later than {nameof(request.EndOn)}");
                }

                var timeEntries = new List<TimeEntryServiceMessage>();
                while (startOn <= endOn)
                {
                    var isTimeEntryExists = await _dataverseService.IsTimeEntryExistsAsync(startOn);

                    if (!isTimeEntryExists)
                    {
                        timeEntries.Add(new TimeEntryServiceMessage
                        {
                            Start = startOn,
                            End = startOn
                        });
                    }

                    startOn = startOn.AddDays(1);
                }

                if (timeEntries.Count == 0)
                {
                    return CommandResultFactory.GetErrorResult<int>(
                        CommandResultCode.Duplicate, "All date entries already exists.");
                }

                try
                {
                    var result = await _dataverseService.AddTimeEntriesAsync(timeEntries);
                    return CommandResultFactory.GetResult(result);
                }
                catch (DataverserServiceException ex)
                {
                    return CommandResultFactory.GetErrorResult<int>(CommandResultCode.Error, ex.Message);
                }
                catch (Exception)
                {
                    return CommandResultFactory.GetErrorResult<int>(CommandResultCode.Error, "Something went wrong.");
                }
            }
        }
    }
}
