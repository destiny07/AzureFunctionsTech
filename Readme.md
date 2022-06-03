A test project for Azure Functions that communicates to Dynamics 365. 
The project uses delegation pattern using Mediatr and the business/core logic is found under AzureFunctionsTech.Api.Core.
Package used to communicate to Dynamics 365 is [official dataverse sdk](https://www.nuget.org/packages/Microsoft.PowerPlatform.Dataverse.Client/)
The featured function is found under the AzureFunctions.Tech.Api.Functions directory.

For unit testing, [xUnit](https://xunit.net/) library is used and [Moq](https://moq.github.io/moq4/) for interface mockups.