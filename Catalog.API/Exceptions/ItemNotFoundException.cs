using System;

namespace Catalog.API.Exceptions
{
  public class ItemNotFoundException : Exception
  {
    private const string message = "item not found";
    public ItemNotFoundException() : base(message)
    {
    }

    public ItemNotFoundException(string message) : base(message)
    {
    }
  }
}