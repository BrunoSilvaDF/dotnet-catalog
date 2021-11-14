using System;
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
  }
}