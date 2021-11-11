using System;
using System.ComponentModel.DataAnnotations;

namespace Catalog.API.Dtos
{
  public record ItemDto(Guid Id, string Name, string Description, decimal Price, DateTimeOffset CreatedAt);
  public record CreateItemDto([Required] string Name, string Description, [Range(1, 1000)] decimal Price);
  public record UpdateItemDto([Required] string Name, string Description, [Range(1, 1000)] decimal Price);
}