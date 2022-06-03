namespace AzureFunctionsTech.Api.Core.Command
{
    public static class CommandResultFactory
    {
        public static CommandResult<T> GetErrorResult<T>(CommandResultCode code, string description)
        {
            return new CommandResult<T>
            {
                Code = code,
                IsSuccess = false,
                Description = description
            };
        }

        public static CommandResult<T> GetResult<T>(T value)
        {
            return new CommandResult<T>
            {
                IsSuccess = true,
                Value = value
            };
        }
    }
}
