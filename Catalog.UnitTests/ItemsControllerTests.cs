using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.API.Controllers;
using Catalog.API.Entitites;
using Catalog.API.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Catalog.UnitTests
{
  public class ItemsControllerTests
  {
    private readonly Mock<IItemsService> serviceStub = new();

    private ItemsController MakeSut(Mock<IItemsService> service)
    {
      return new(service.Object);
    }

    [Fact]
    public async Task GetItemsAsync_WithUnExistingItems_ReturnsNoContent()
    {
      serviceStub.Setup(service => service.GetItemsAsync(It.IsAny<string>())).ReturnsAsync(new List<Item>());

      var controller = MakeSut(serviceStub);

      var result = await controller.GetItemsAsync();

      result.Result.Should().BeOfType<NoContentResult>();

    }

    [Fact]
    public async Task GetItemsAsync_WithExistingItems_ReturnsOk()
    {
      var expectedItems = ItemHelpers.CreateRandomItems();

      serviceStub.Setup(service => service.GetItemsAsync(It.IsAny<string>())).ReturnsAsync(expectedItems);

      var controller = MakeSut(serviceStub);

      var result = await controller.GetItemsAsync();

      result.Result.Should().BeOfType<OkObjectResult>();
      (result.Result as OkObjectResult).Value.Should().BeEquivalentTo(expectedItems);
    }

    [Fact]
    public async Task GetItemAsync_WithUnExistingItem_ReturnsNotFound()
    {
      serviceStub.Setup(service => service.GetItemAsync(It.IsAny<Guid>())).ReturnsAsync((Item)null);

      var controller = MakeSut(serviceStub);

      var result = await controller.GetItemAsync(Guid.NewGuid());

      result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetItemAsync_WithExistingItem_ReturnsOk()
    {
      var expectedItem = ItemHelpers.CreateRandomItem();

      serviceStub.Setup(service => service.GetItemAsync(It.IsAny<Guid>())).ReturnsAsync(expectedItem);

      var controller = MakeSut(serviceStub);

      var result = await controller.GetItemAsync(expectedItem.Id);

      result.Result.Should().BeOfType<OkObjectResult>();
      (result.Result as OkObjectResult).Value.Should().BeEquivalentTo(expectedItem);
    }

    // TODO testes
    [Fact(Skip = "")]
    public void CreateItemAsync_WithInvalidItemDto_ReturnsBadRequest() { }

    [Fact(Skip = "")]
    public void CreateItemAsync_WithItemDto_ReturnsCreated() { }

    [Fact(Skip = "")]
    public void UpdateItemAsync_WithUnexistingItem_ReturnsNotFound() { }

    [Fact(Skip = "")]
    public void UpdateItemAsync_WithExistingItem_ReturnsNoContent() { }

    [Fact(Skip = "")]
    public void DeleteItemAsync_WithUnexistingItem_ReturnsNotFound() { }

    [Fact(Skip = "")]
    public void DeleteItemAsync_WithExistingItem_ReturnsNoContent() { }
  }
}
