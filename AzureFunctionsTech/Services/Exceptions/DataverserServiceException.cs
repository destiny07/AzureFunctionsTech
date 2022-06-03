using System;

namespace AzureFunctionsTech.Api.Services.Exceptions
{
    public class DataverserServiceException : Exception
    {
        public DataverserServiceException()
        {
        }

        public DataverserServiceException(string message) : base(message)
        {
        }
    }
}
