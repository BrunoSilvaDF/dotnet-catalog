using DotnetCatalog.Dtos;
using DotnetCatalog.Entitites;

namespace DotnetCatalog
{
  public static class Extensions
  {
    public static ItemDto AsDto(this Item item)
    {
      return new ItemDto
      {
        Id = item.Id,
        Name = item.Name,
        Price = item.Price,
        CreatedAt = item.CreatedAt
      };
    }
  }
}