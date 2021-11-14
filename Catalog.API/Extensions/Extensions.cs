using Catalog.API.Dtos;
using Catalog.API.Entitites;

namespace Catalog.API.Extensions
{
  public static class Extensions
  {
    public static ItemDto AsDto(this Item item)
    {
      return new ItemDto(item.Id, item.Name, item.Description, item.Price, item.CreatedAt);
    }
  }
}