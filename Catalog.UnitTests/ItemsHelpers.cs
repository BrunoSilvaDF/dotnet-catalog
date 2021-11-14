using System;
using Catalog.API.Dtos;
using Catalog.API.Entitites;

namespace Catalog.UnitTests
{
  public static class ItemHelpers
  {
    public static Item CreateRandomItem()
    {
      Random rand = new();
      return new()
      {
        Id = Guid.NewGuid(),
        Name = Guid.NewGuid().ToString(),
        Price = rand.Next(1000),
        CreatedAt = DateTimeOffset.UtcNow
      };
    }

    public static Item[] CreateRandomItems()
    {
      return new[] { CreateRandomItem(), CreateRandomItem(), CreateRandomItem(), };
    }

    public static ItemDto CreateRandomItemDto()
    {
      Random rand = new();
      return new ItemDto(Guid.NewGuid(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), rand.Next(1000), DateTimeOffset.UtcNow);
    }

    public static CreateItemDto CreateRandomCreateItemDto()
    {
      Random rand = new();
      return new CreateItemDto(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), rand.Next(1000));
    }

    public static UpdateItemDto CreateRandomUpdateItemDto()
    {
      Random rand = new();
      return new UpdateItemDto(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), rand.Next(1000));
    }
  }
}