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
  // 
  // public string Nome { get; init; }
  // init => init-only properties => can't modify after init
  // private set => can define only using a constructor

  public class Item
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal Price { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
  }
}