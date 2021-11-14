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
    private readonly ILogger<ItemsService> logger;

    public ItemsService(IItemsRepository repository, ILogger<ItemsService> logger)
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

    public Task<Item> GetItemAsync(Guid id)
    {
      var item = repository.GetItemAsync(id);

      if (item is null)
      {
        throw new ItemNotFoundException();
      }

      return item;
    }

    public async Task<IEnumerable<Item>> GetItemsAsync(string nameToMatch)
    {
      var items = await repository.GetItemsAsync();

      if (!string.IsNullOrWhiteSpace(nameToMatch))
      {
        items = items.Where(item => item.Name.Contains(nameToMatch, StringComparison.OrdinalIgnoreCase));
      }

      logger.LogInformation($"{DateTime.UtcNow.ToString("hh:mm:ss")}: Retrieved {items.Count()} items");

      return items;
    }

    public async Task UpdateItemAsync(Guid id, string name, string description, decimal price)
    {
      var existingItem = await repository.GetItemAsync(id);

      if (existingItem is null)
      {
        throw new ItemNotFoundException("item not found");
      }

      existingItem.Name = name;
      existingItem.Description = description;
      existingItem.Price = price;

      await repository.UpdateItemAsync(existingItem);
    }
  }
}