using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.API.Dtos;
using Catalog.API.Entitites;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Catalog.API.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class ItemsController : ControllerBase
  {
    private readonly IItemsRepository repository;
    private readonly ILogger<ItemsController> logger;

    public ItemsController(IItemsRepository repository, ILogger<ItemsController> logger)
    {
      this.repository = repository;
      this.logger = logger;
    }

    [HttpGet]
    public async Task<IEnumerable<ItemDto>> GetItemsAsync()
    {
      // before extension method
      // return repository.GetItems().Select(item => new ItemDto
      // {
      //   Id = item.Id,
      //   Name = item.Name,
      //   Price = item.Price,
      //   CreatedAt = item.CreatedAt
      // });
      var items = (await repository.GetItemsAsync()).Select(item => item.AsDto());

      logger.LogInformation($"{DateTime.UtcNow.ToString("hh:mm:ss")}: Retrieved {items.Count()} items");

      return items;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)
    {
      var item = await repository.GetItemAsync(id);

      if (item is null)
      {
        return NotFound();
      }

      return item.AsDto();
    }

    [HttpPost]
    public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto)
    {
      Item item = new()
      {
        Id = Guid.NewGuid(),
        Name = itemDto.Name,
        Price = itemDto.Price,
        CreatedAt = DateTimeOffset.UtcNow
      };

      await repository.CreateItemAsync(item);

      // Convenção
      //  => retornar Created (201)
      //  => retornar um header com a localização na api do retorno
      //                          host          rota get      id
      //      location: https://localhost:5001/Items/b5c8f35d-e715-40f3-9182-3ca0e227a7a5
      return CreatedAtAction(nameof(GetItemAsync), new { id = item.Id }, item.AsDto());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateItemAsync(Guid id, UpdateItemDto itemDto)
    {
      var existingItem = await repository.GetItemAsync(id);

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

      await repository.UpdateItemAsync(updatedItem);

      // Convenção => retornar NoContent (204)
      return NoContent();
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteItemAsync(Guid id)
    {
      var existingItem = repository.GetItemAsync(id);

      if (existingItem is null)
      {
        return NotFound();
      }

      await repository.DeleteItemAsync(id);

      // Convenção => retornar NoContent (204)
      return NoContent();
    }
  }
}