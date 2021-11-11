using System;
using System.Threading.Tasks;
using Catalog.API.Controllers;
using Catalog.API.Dtos;
using Catalog.API.Entitites;
using Catalog.API.Repositories;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Catalog.UnitTests
{
  public class ItemsControllerTests
  {
    private readonly Mock<IItemsRepository> repositoryStub = new();
    private readonly Mock<ILogger<ItemsController>> loggerStub = new();
    private readonly Random rand = new();

    // naming convention
    // public void UnitOfWork_StateUnderTest_ExpectedBehavior()
    [Fact]
    public async Task GetItemAsync_WithUnexistingItem_ReturnsNotFound()
    {
      // Arrange => setup mocks, variables, ...
      repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
        .ReturnsAsync((Item)null);

      var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

      // Act => execute the test
      var result = await controller.GetItemAsync(Guid.NewGuid());

      // Assert => verify
      // Assert.IsType<NotFoundResult>(result.Result);
      result.Result.Should().BeOfType<NotFoundResult>();
    }

    [Fact]
    public async Task GetItemAsync_WithExistingItem_ReturnsExpectedItem()
    {
      // Arrange
      var expectedItem = CreateRandomItem();

      repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
        .ReturnsAsync(expectedItem);

      var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

      // Act
      var result = await controller.GetItemAsync(Guid.NewGuid());

      // Esse Assert aqui não é legal :(
      // Assert.IsType<ItemDto>(actionResult.Value);
      // Assert.Equal(expectedItem.Id, dto.Id);
      // Assert.Equal(expectedItem.Name, dto.Name);
      // Assert.Equal(expectedItem.Price, dto.Price);
      // Assert.Equal(expectedItem.CreatedAt, dto.CreatedAt);

      // FluentAssertions lib
      //  quando usamos record (ao invés de class), esse sobrescreve o método Equals
      //  por isso precisamos passar o 2ndo param, para focar nas propriedades e não no Obj
      result.Value.Should()
        .BeEquivalentTo(expectedItem, options => options.ComparingByMembers<Item>());
    }

    [Fact]
    public async Task GetItemsAsync_WithExistingItems_ReturnsAllItems()
    {
      // Arrange
      var expectedItems = new[] { CreateRandomItem(), CreateRandomItem(), CreateRandomItem(), };

      repositoryStub.Setup(repo => repo.GetItemsAsync()).ReturnsAsync(expectedItems);

      var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

      // Act
      var actualItems = await controller.GetItemsAsync();

      // Assert
      actualItems.Should()
        .BeEquivalentTo(expectedItems, options => options.ComparingByMembers<Item>());
    }

    [Fact]
    public async Task CreateItemsAsync_WithItemToCreate_ReturnsCreatedItem()
    {
      // Arrange
      var itemToCreate = new CreateItemDto()
      {
        Name = Guid.NewGuid().ToString(),
        Price = rand.Next(1000)
      };

      var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

      // Act
      var result = await controller.CreateItemAsync(itemToCreate);

      // Assert
      var createdItem = (result.Result as CreatedAtActionResult).Value as ItemDto;
      itemToCreate.Should()
        .BeEquivalentTo(
          createdItem,
          options => options.ComparingByMembers<ItemDto>().ExcludingMissingMembers()
        );
      createdItem.Id.Should().NotBeEmpty();
      createdItem.CreatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromMilliseconds(1000));
    }

    [Fact]
    public async Task UpdateItemAsync_WithExistingItem_ReturnsNoContent()
    {
      // Arrange
      var existingItem = CreateRandomItem();

      repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
        .ReturnsAsync(existingItem);

      var itemId = existingItem.Id;
      var itemToUpdate = new UpdateItemDto()
      {
        Name = Guid.NewGuid().ToString(),
        Price = existingItem.Price + 3
      };

      var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

      // Act
      var result = await controller.UpdateItemAsync(itemId, itemToUpdate);

      // Assert
      result.Should().BeOfType<NoContentResult>();
    }

    [Fact]
    public async Task DeleteItemAsync_WithExistingItem_ReturnsNoContent()
    {
      // Arrange
      var existingItem = CreateRandomItem();

      repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
        .ReturnsAsync(existingItem);

      var controller = new ItemsController(repositoryStub.Object, loggerStub.Object);

      // Act
      var result = await controller.DeleteItemAsync(existingItem.Id);

      // Assert
      result.Should().BeOfType<NoContentResult>();
    }

    private Item CreateRandomItem()
    {
      return new()
      {
        Id = Guid.NewGuid(),
        Name = Guid.NewGuid().ToString(),
        Price = rand.Next(1000),
        CreatedAt = DateTimeOffset.UtcNow
      };
    }
  }
}
