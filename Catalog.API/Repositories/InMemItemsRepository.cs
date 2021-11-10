using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.API.Entitites;

namespace Catalog.API.Repositories
{
  public class InMemItemsRepository : IItemsRepository
  {
    // target-typed new item
    private readonly List<Item> items = new()
    {
      new Item { Id = Guid.NewGuid(), Name = "Potion", Price = 9, CreatedAt = DateTimeOffset.UtcNow },
      new Item { Id = Guid.NewGuid(), Name = "Iron Sword", Price = 20, CreatedAt = DateTimeOffset.UtcNow },
      new Item { Id = Guid.NewGuid(), Name = "Bronze Shield", Price = 18, CreatedAt = DateTimeOffset.UtcNow },
    };

    public async Task<IEnumerable<Item>> GetItemsAsync()
    {
      return await Task.FromResult(items);
    }

    public async Task<Item> GetItemAsync(Guid id)
    {
      return await Task.FromResult(items.Where(item => item.Id == id).SingleOrDefault());
    }

    public async Task CreateItemAsync(Item item)
    {
      items.Add(item);

      // adicionado somente para fins de compilação
      await Task.CompletedTask;
    }

    public async Task UpdateItemAsync(Item item)
    {
      var index = items.FindIndex(existingItem => existingItem.Id == item.Id);
      items[index] = item;
      await Task.CompletedTask;
    }

    public async Task DeleteItemAsync(Guid id)
    {
      var index = items.FindIndex(existingItem => existingItem.Id == id);
      items.RemoveAt(index);
      await Task.CompletedTask;
    }
  }
}