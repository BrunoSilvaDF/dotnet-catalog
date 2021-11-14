using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.API.Entitites;
using Catalog.API.Exceptions;
using Catalog.API.Repositories;
using Microsoft.Extensions.Logging;

namespace Catalog.API.Services
{
  public class ItemsService : IItemsService
  {
    private readonly IItemsRepository repository;
    private readonly ILogger<IItemsService> logger;

    public ItemsService(IItemsRepository repository, ILogger<IItemsService> logger)
    {
      this.repository = repository;
      this.logger = logger;
    }

    public async Task<Item> CreateItemAsync(string name, string description, decimal price)
    {
      Item item = new()
      {
        Id = Guid.NewGuid(),
        Name = name,
        Description = description,
        Price = price,
        CreatedAt = DateTimeOffset.UtcNow
      };

      await repository.CreateItemAsync(item);

      return item;
    }

    public async Task DeleteItemAsync(Guid id)
    {
      var existingItem = await repository.GetItemAsync(id);

      if (existingItem is null)
      {
        throw new ItemNotFoundException();
      }

      await repository.DeleteItemAsync(id);
    }

    public async Task<Item> GetItemAsync(Guid id)
    {
      var item = await repository.GetItemAsync(id);

      if (item is null)
      {
        throw new ItemNotFoundException();
      }

      return item;
    }

    public async Task<IEnumerable<Item>> GetItemsAsync(string nameToMatch = null)
    {
      var items = await repository.GetItemsAsync();

      if (!string.IsNullOrWhiteSpace(nameToMatch))
      {
        items = items.Where(item => item.Name.Contains(nameToMatch, StringComparison.OrdinalIgnoreCase));
      }

      logger.LogInformation($"{DateTime.UtcNow.ToString("hh:mm:ss")}: Retrieved {items.Count()} items");

      return items;
    }

    public async Task<Item> UpdateItemAsync(Guid id, string name, string description, decimal price)
    {
      var existingItem = await repository.GetItemAsync(id);

      if (existingItem is null)
      {
        throw new ItemNotFoundException("item not found");
      }

      Item updatedItem = new()
      {
        Id = existingItem.Id,
        Name = name,
        Description = description,
        Price = price
      };

      await repository.UpdateItemAsync(updatedItem);

      return updatedItem;
    }
  }
}