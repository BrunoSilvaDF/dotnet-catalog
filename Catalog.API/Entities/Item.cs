using System;

namespace Catalog.API.Entitites
{
  // Record Types
  //  * use for immutable objects
  //  * with-expressions support
  //      Item potion2 = potion1 with { Price = 12 }
  //  * value-based equality support
  //      Item potion1 = new() { Name = "Potion", Price = 9 }
  //      Item potion2 = new() { Name = "Potion", Price = 9 }
  //      bool areEqual = potion1 == potion2; // = true

  public record Item
  {
    // init => init-only properties => can't modify after init
    // private set => can define only using a constructor
    public Guid Id { get; init; }
    public string Name { get; init; }
    public decimal Price { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
  }
}