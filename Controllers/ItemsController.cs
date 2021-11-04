using System;
using System.Collections.Generic;
using DotnetCatalog.Entitites;
using DotnetCatalog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DotnetCatalog.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class ItemsController : ControllerBase
  {
    private readonly InMemItemsRepository repository;

    public ItemsController()
    {
      repository = new InMemItemsRepository();
    }

    [HttpGet]
    public IEnumerable<Item> GetItems()
    {
      return repository.GetItems();
    }

    [HttpGet("{id}")]
    public ActionResult<Item> GetItem(Guid id)
    {
      var item = repository.GetItem(id);

      if (item is null)
      {
        return NotFound();
      }

      return item;
    }
  }
}