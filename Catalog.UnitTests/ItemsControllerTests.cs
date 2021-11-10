using System;
using System.Threading.Tasks;
using Catalog.API.Controllers;
using Catalog.API.Entitites;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Catalog.UnitTests
{
  public class ItemsControllerTests
  {
    // naming convention
    // public void UnitOfWork_StateUnderTest_ExpectedBehavior()
    [Fact]
    public async Task GetItemsAsync_WithUnexistingItem_ReturnsNotFound()
    {
      // Arrange => setup mocks, variables, ...
      var repositoryStub = new Mock<IItemsRepository>();
      repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
        .ReturnsAsync((Item)null);

      var loggerStub = new Mock<ILogger<ItemsController>>();

      var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

      // Act => execute the test
      var actionResult = await controller.GetItemAsync(Guid.NewGuid());

      // Assert => verify
      Assert.IsType<NotFoundResult>(actionResult.Result);
    }
  }
}
