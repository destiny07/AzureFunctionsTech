using System;
using System.Runtime.Serialization;

namespace AzureFunctionsTech.Api.Services.Messages
{
    public class TimeEntryServiceMessage
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }
}
