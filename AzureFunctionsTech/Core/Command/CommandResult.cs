namespace AzureFunctionsTech.Api.Core.Command
{
    public class CommandResult<T>
    {
        public T Value { get; set; }
        public bool IsSuccess { get; set; }
        public CommandResultCode Code { get; set; }
        public string Description { get; set; }

    }
}
