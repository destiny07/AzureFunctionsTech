using AzureFunctionsTech.Api.Core.Command;
using AzureFunctionsTech.Api.Services;
using AzureFunctionsTech.Api.Services.Exceptions;
using AzureFunctionsTech.Api.Services.Messages;
using AzureFunctionsTech.UnitTests.Fixtures;
using Moq;
using static AzureFunctionsTech.Api.Core.TimeEntries.CreateTimeEntriesCommand;

namespace AzureFunctionsTech.UnitTests.CommandHandlers
{
    public class CreateTimeEntriesCommandTests : IClassFixture<CreateTimeEntriesCommandFixture>
    {
        private readonly CreateTimeEntriesCommandFixture _createTimeEntriesCommandFixture;

        public CreateTimeEntriesCommandTests(CreateTimeEntriesCommandFixture createTimeEntriesCommandFixture)
        {
            _createTimeEntriesCommandFixture = createTimeEntriesCommandFixture;
        }

        [Fact]
        public async Task CreateTimeEntries_InvalidDateRange_ShouldReturnErrorResult()
        {
            var request = _createTimeEntriesCommandFixture.GetEndDateEarlierThanStartDateRequest();

            var createRequestCommandHandler = new CreateTimeEntriesCommandHandler(It.IsAny<IDataverseService>());
            var result = await createRequestCommandHandler.Handle(request, It.IsAny<CancellationToken>());

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task CreateTimeEntries_InvalidDateRange_ShouldReturnInvalidArgument()
        {
            var request = _createTimeEntriesCommandFixture.GetEndDateEarlierThanStartDateRequest();

            var createRequestCommandHandler = new CreateTimeEntriesCommandHandler(It.IsAny<IDataverseService>());
            var result = await createRequestCommandHandler.Handle(request, It.IsAny<CancellationToken>());

            Assert.Equal(CommandResultCode.InvalidArgument, result.Code);
        }

        [Fact]
        public async Task CreateTimeEntries_DuplicateEntries_ShouldReturnErrorResult()
        {
            var request = _createTimeEntriesCommandFixture.GetDefaultCreateTimeEntriesRequest();

            var dataverseService = new Mock<IDataverseService>();
            dataverseService.Setup(x => x.IsTimeEntryExistsAsync(It.IsAny<DateTime>())).ReturnsAsync(true);

            var createRequestCommandHandler = new CreateTimeEntriesCommandHandler(dataverseService.Object);
            var result = await createRequestCommandHandler.Handle(request, It.IsAny<CancellationToken>());

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task CreateTimeEntries_DuplicateEntries_ShouldReturnDuplicate()
        {
            var request = _createTimeEntriesCommandFixture.GetDefaultCreateTimeEntriesRequest();

            var dataverseService = new Mock<IDataverseService>();
            dataverseService.Setup(x => x.IsTimeEntryExistsAsync(It.IsAny<DateTime>())).ReturnsAsync(true);

            var createRequestCommandHandler = new CreateTimeEntriesCommandHandler(dataverseService.Object);
            var result = await createRequestCommandHandler.Handle(request, It.IsAny<CancellationToken>());

            Assert.Equal(CommandResultCode.Duplicate, result.Code);
        }

        [Fact]
        public async Task CreateTimeEntries_ValidRequest_ShouldReturnSuccess()
        {
            var request = _createTimeEntriesCommandFixture.GetRequestWithDateIntervalRequest(3);

            var dataverseService = new Mock<IDataverseService>();
            dataverseService.Setup(x => x.IsTimeEntryExistsAsync(It.IsAny<DateTime>())).ReturnsAsync(false);

            var createRequestCommandHandler = new CreateTimeEntriesCommandHandler(dataverseService.Object);
            var result = await createRequestCommandHandler.Handle(request, It.IsAny<CancellationToken>());

            Assert.True(result.IsSuccess);
        }

        [Fact]
        public async Task CreateTimeEntries_ValidRequest_ShouldCallApiService()
        {
            var request = _createTimeEntriesCommandFixture.GetRequestWithDateIntervalRequest(3);

            var dataverseService = new Mock<IDataverseService>();
            dataverseService.Setup(x => x.IsTimeEntryExistsAsync(It.IsAny<DateTime>())).ReturnsAsync(false);

            var createRequestCommandHandler = new CreateTimeEntriesCommandHandler(dataverseService.Object);
            var result = await createRequestCommandHandler.Handle(request, It.IsAny<CancellationToken>());

            dataverseService.Verify(mock => mock.AddTimeEntriesAsync(It.IsAny<IEnumerable<TimeEntryServiceMessage>>()), Times.Once());
        }

        [Fact]
        public async Task CreateTimeEntries_ApiServiceException_ShouldReturnErrorResult()
        {
            var request = _createTimeEntriesCommandFixture.GetRequestWithDateIntervalRequest(3);

            var dataverseService = new Mock<IDataverseService>();
            dataverseService
                .Setup(x => x.AddTimeEntriesAsync(It.IsAny<IEnumerable<TimeEntryServiceMessage>>()))
                .Throws<DataverserServiceException>();

            var createRequestCommandHandler = new CreateTimeEntriesCommandHandler(dataverseService.Object);
            var result = await createRequestCommandHandler.Handle(request, It.IsAny<CancellationToken>());

            Assert.False(result.IsSuccess);
        }

        [Fact]
        public async Task CreateTimeEntries_ApiServiceException_ShouldReturnError()
        {
            var request = _createTimeEntriesCommandFixture.GetRequestWithDateIntervalRequest(3);

            var dataverseService = new Mock<IDataverseService>();
            dataverseService
                .Setup(x => x.AddTimeEntriesAsync(It.IsAny<IEnumerable<TimeEntryServiceMessage>>()))
                .Throws<DataverserServiceException>();

            var createRequestCommandHandler = new CreateTimeEntriesCommandHandler(dataverseService.Object);
            var result = await createRequestCommandHandler.Handle(request, It.IsAny<CancellationToken>());

            Assert.Equal(CommandResultCode.Error, result.Code);
        }
    }
}
