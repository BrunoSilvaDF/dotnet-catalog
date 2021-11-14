using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.API.Dtos;
using Catalog.API.Entitites;
using Catalog.API.Exceptions;
using Catalog.API.Repositories;
using Catalog.API.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Catalog.UnitTests
{
  public class ItemsServiceTests
  {
    private readonly Mock<IItemsRepository> repositoryStub = new();
    private readonly Mock<ILogger<IItemsService>> loggerStub = new();
    private readonly Random rand = new();

    private ItemsService MakeSut(Mock<IItemsRepository> repository)
    {
      return new(repository.Object, loggerStub.Object);
    }

    // public void UnitOfWork_StateUnderTest_ExpectedBehavior()
    [Fact]
    public async void GetItemAsync_WithUnexistingItem_ThrowsItemNotFoundException()
    {
      // Arrange => setup mocks, variables, ...
      repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
        .ReturnsAsync((Item)null);

      var service = MakeSut(repositoryStub);

      // Act => execute the test
      Func<Task> act = () => service.GetItemAsync(Guid.NewGuid());

      // // Assert => verify
      // await Assert.ThrowsAsync<ItemNotFoundException>(() => service.GetItemAsync(Guid.NewGuid()));
      await act.Should().ThrowAsync<ItemNotFoundException>();
    }

    [Fact]
    public async Task GetItemAsync_WithExistingItem_ReturnsExpectedItem()
    {
      // Arrange
      var expectedItem = ItemHelpers.CreateRandomItem();

      repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
        .ReturnsAsync(expectedItem);

      var service = MakeSut(repositoryStub);

      // Act
      var item = await service.GetItemAsync(Guid.NewGuid());

      // FluentAssertions lib
      item.Should().BeEquivalentTo(expectedItem);
    }

    [Fact]
    public async Task GetItemsAsync_WithExistingItems_ReturnsAllItems()
    {
      // Arrange
      var expectedItems = new[] { ItemHelpers.CreateRandomItem(), ItemHelpers.CreateRandomItem(), ItemHelpers.CreateRandomItem(), };

      repositoryStub.Setup(repo => repo.GetItemsAsync()).ReturnsAsync(expectedItems);

      var service = MakeSut(repositoryStub);

      // Act
      var actualItems = await service.GetItemsAsync();

      // Assert
      actualItems.Should().BeEquivalentTo(expectedItems);
    }

    [Fact]
    public async Task GetItemsAsync_WithMatchingItems_ReturnsMatchingItems()
    {
      // Arrange
      var allItems = new[]
      {
        new Item(){ Name = "Potion" },
        new Item(){ Name = "Antidote" },
        new Item(){ Name = "Hi-Potion" }
      };

      var nameToMatch = "Potion";

      repositoryStub.Setup(repo => repo.GetItemsAsync()).ReturnsAsync(allItems);

      var service = MakeSut(repositoryStub);

      // Act
      var foundItems = await service.GetItemsAsync(nameToMatch);

      // Assert
      foundItems.Should().OnlyContain(
        item => item.Name == allItems[0].Name || item.Name == allItems[2].Name
      );
    }

    [Fact]
    public async Task CreateItemsAsync_WithItemToCreate_ReturnsCreatedItem()
    {
      // Arrange
      var itemToCreate = new CreateItemDto(
        Guid.NewGuid().ToString(),
        Guid.NewGuid().ToString(),
        rand.Next(1000)
      );

      var service = MakeSut(repositoryStub);

      // Act
      var createdItem = await service.CreateItemAsync(itemToCreate.Name, itemToCreate.Description, itemToCreate.Price);

      // Assert
      itemToCreate.Should()
        .BeEquivalentTo(
          createdItem,
          options => options.ComparingByMembers<ItemDto>().ExcludingMissingMembers()
        );
      createdItem.Id.Should().NotBeEmpty();
      createdItem.CreatedAt.Should().BeCloseTo(DateTimeOffset.UtcNow, TimeSpan.FromMilliseconds(1000));
    }

    [Fact]
    public async Task UpdateItemAsync_WithUnExistingItem_ThrowsItemNotFoundException()
    {
      // Arrange
      var existingItem = ItemHelpers.CreateRandomItem();

      repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
        .ReturnsAsync((Item)null);

      var itemId = existingItem.Id;
      var itemToUpdate = new Item
      {
        Id = itemId,
        Name = Guid.NewGuid().ToString(),
        Description = Guid.NewGuid().ToString(),
        Price = existingItem.Price + 3
      };

      var service = MakeSut(repositoryStub);

      // Act
      Func<Task> act = () => service.UpdateItemAsync(itemToUpdate.Id, itemToUpdate.Name, itemToUpdate.Description, itemToUpdate.Price);

      // // Assert => verify
      await act.Should().ThrowAsync<ItemNotFoundException>();
    }

    [Fact]
    public async Task UpdateItemAsync_WithExistingItem_ReturnsUpdatedItem()
    {
      // Arrange
      var existingItem = ItemHelpers.CreateRandomItem();

      repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
        .ReturnsAsync(existingItem);

      var itemId = existingItem.Id;
      var itemToUpdate = new Item
      {
        Id = itemId,
        Name = Guid.NewGuid().ToString(),
        Description = Guid.NewGuid().ToString(),
        Price = existingItem.Price + 3
      };

      var service = MakeSut(repositoryStub);

      // Act
      var updatedItem = await service.UpdateItemAsync(itemToUpdate.Id, itemToUpdate.Name, itemToUpdate.Description, itemToUpdate.Price);

      // Assert
      updatedItem.Should().BeEquivalentTo(itemToUpdate);
    }

    [Fact]
    public async Task DeleteItemAsync_WithUnExistingItem_ThrowsItemNotFoundException()
    {
      // Arrange
      var existingItem = ItemHelpers.CreateRandomItem();

      repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
        .ReturnsAsync((Item)null);

      var service = MakeSut(repositoryStub);

      // Act
      Func<Task> act = () => service.GetItemAsync(existingItem.Id);

      // // Assert => verify
      await act.Should().ThrowAsync<ItemNotFoundException>();
    }

    [Fact]
    public async Task DeleteItemAsync_WithExistingItem_NotThrows()
    {
      // Arrange
      var existingItem = ItemHelpers.CreateRandomItem();

      repositoryStub.Setup(repo => repo.GetItemAsync(It.IsAny<Guid>()))
        .ReturnsAsync(existingItem);

      var service = MakeSut(repositoryStub);

      // Act
      // await service.DeleteItemAsync(existingItem.Id);
      Func<Task> act = () => service.GetItemAsync(existingItem.Id);

      // // Assert => verify
      await act.Should().NotThrowAsync<ItemNotFoundException>();
    }
  }
}
