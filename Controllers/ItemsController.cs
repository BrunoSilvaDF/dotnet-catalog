using System;
using System.Collections.Generic;
using System.Linq;
using DotnetCatalog.Dtos;
using DotnetCatalog.Entitites;
using DotnetCatalog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace DotnetCatalog.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class ItemsController : ControllerBase
  {
    private readonly IItemsRepository repository;

    public ItemsController(IItemsRepository repository)
    {
      this.repository = repository;
    }

    [HttpGet]
    public IEnumerable<ItemDto> GetItems()
    {
      // before extension method
      // return repository.GetItems().Select(item => new ItemDto
      // {
      //   Id = item.Id,
      //   Name = item.Name,
      //   Price = item.Price,
      //   CreatedAt = item.CreatedAt
      // });
      return repository.GetItems().Select(item => item.AsDto());
    }

    [HttpGet("{id}")]
    public ActionResult<ItemDto> GetItem(Guid id)
    {
      var item = repository.GetItem(id);

      if (item is null)
      {
        return NotFound();
      }

      return item.AsDto();
    }
  }
}