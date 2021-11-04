using System;

namespace DotnetCatalog.Dtos
{
  // DTO => Data Transfer Object
  //  the "contract" that is exposed on API, consumed by the client
  public record ItemDto
  {
    public Guid Id { get; init; }
    public string Name { get; init; }
    public decimal Price { get; init; }
    public DateTimeOffset CreatedAt { get; init; }
  }
}