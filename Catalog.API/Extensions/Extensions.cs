using System.Collections.Generic;
using Catalog.API.Dtos;
using Catalog.API.Entitites;
using System.Linq;

namespace Catalog.API.Extensions
{
  public static class Extensions
  {
    public static ItemDto AsDto(this Item item)
    {
      return new ItemDto(item.Id, item.Name, item.Description, item.Price, item.CreatedAt);
    }

    public static bool IsEmpty<T>(this IEnumerable<T> data)
    {
      return !data.Any();
    }
  }
}