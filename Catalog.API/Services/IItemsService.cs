using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Catalog.API.Entitites;

namespace Catalog.API.Services
{
  public interface IItemsService
  {
    Task<Item> GetItemAsync(Guid id);
    Task<IEnumerable<Item>> GetItemsAsync(string nameToMatch);
    Task<Item> CreateItemAsync(string name, string description, decimal price);
    Task<Item> UpdateItemAsync(Guid id, string name, string description, decimal price);
    Task DeleteItemAsync(Guid id);
  }
}