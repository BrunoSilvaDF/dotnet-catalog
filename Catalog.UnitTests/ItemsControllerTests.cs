using System;
using System.Collections.Generic;

using System.Threading.Tasks;
using Catalog.API.Controllers;
using Catalog.API.Dtos;
using Catalog.API.Entitites;
using Catalog.API.Exceptions;
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

    [Fact]
    public async Task CreateItemAsync_WithInvalidItemDto_ReturnsBadRequest()
    {
      var invalidItem = new
      {
        Price = 10
      };

      serviceStub.Setup(service => service.CreateItemAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>()));

      var controller = MakeSut(serviceStub);

      var result = await controller.CreateItemAsync(new("", "", 10));

      result.Result.Should().BeOfType<BadRequestResult>();
    }

    [Fact]
    public async Task CreateItemAsync_WithItemDto_ReturnsCreated()
    {
      var itemToCreate = ItemHelpers.CreateRandomItem();

      serviceStub.Setup(service => service.CreateItemAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>())).ReturnsAsync(itemToCreate);

      var controller = MakeSut(serviceStub);

      var result = await controller.CreateItemAsync(new CreateItemDto(itemToCreate.Name, itemToCreate.Description, itemToCreate.Price));

      result.Result.Should().BeOfType<CreatedAtActionResult>();
      (result.Result as CreatedAtActionResult).Value.Should().BeEquivalentTo(
        itemToCreate,
        options => options.ComparingByMembers<ItemDto>().ExcludingMissingMembers()
      );
    }

    [Fact]
    public async Task UpdateItemAsync_WithUnexistingItem_ReturnsNotFound()
    {
      var itemToUpdate = ItemHelpers.CreateRandomUpdateItemDto();

      serviceStub.Setup(service => service
        .UpdateItemAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>()))
        .ThrowsAsync(new ItemNotFoundException());
      var controller = MakeSut(serviceStub);

      var result = await controller.UpdateItemAsync(Guid.NewGuid(), itemToUpdate);

      result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task UpdateItemAsync_WithExistingItem_ReturnsNoContent()
    {
      var itemToUpdate = ItemHelpers.CreateRandomUpdateItemDto();

      serviceStub.Setup(service => service
        .UpdateItemAsync(It.IsAny<Guid>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>()));
      var controller = MakeSut(serviceStub);

      var result = await controller.UpdateItemAsync(Guid.NewGuid(), itemToUpdate);

      result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeleteItemAsync_WithUnexistingItem_ReturnsNotFound()
    {
      serviceStub.Setup(service => service.DeleteItemAsync(It.IsAny<Guid>())).ThrowsAsync(new ItemNotFoundException());
      var controller = MakeSut(serviceStub);

      var result = await controller.DeleteItemAsync(Guid.NewGuid());

      result.Should().BeOfType<NotFoundObjectResult>();
    }

    [Fact]
    public async Task DeleteItemAsync_WithExistingItem_ReturnsNoContent()
    {
      serviceStub.Setup(service => service.DeleteItemAsync(It.IsAny<Guid>()));
      var controller = MakeSut(serviceStub);

      var result = await controller.DeleteItemAsync(Guid.NewGuid());

      result.Should().BeOfType<NoContentResult>();
    }
  }
}
