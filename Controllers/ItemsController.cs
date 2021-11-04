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

    [HttpPost]
    public ActionResult<ItemDto> CreateItem(CreateItemDto itemDto)
    {
      Item item = new()
      {
        Id = Guid.NewGuid(),
        Name = itemDto.Name,
        Price = itemDto.Price,
        CreatedAt = DateTimeOffset.UtcNow
      };

      repository.CreateItem(item);

      // Convenção
      //  => retornar Created (201)
      //  => retornar um header com a localização na api do retorno
      //                          host          rota get      id
      //      location: https://localhost:5001/Items/b5c8f35d-e715-40f3-9182-3ca0e227a7a5
      return CreatedAtAction(nameof(GetItem), new { id = item.Id }, item.AsDto());
    }

    [HttpPut("{id}")]
    public ActionResult UpdateItem(Guid id, UpdateItemDto itemDto)
    {
      var existingItem = repository.GetItem(id);

      if (existingItem is null)
      {
        return NotFound();
      }

      // record type with-expression
      // preserva mutabilidade
      Item updatedItem = existingItem with
      {
        Name = itemDto.Name,
        Price = itemDto.Price
      };

      repository.UpdateItem(updatedItem);

      // Convenção => retornar NoContent (204)
      return NoContent();
    }
  }
}