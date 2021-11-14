using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Catalog.API.Dtos;
using Catalog.API.Exceptions;
using Catalog.API.Extensions;
using Catalog.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class ItemsController : ControllerBase
  {
    private readonly IItemsService service;

    public ItemsController(IItemsService service)
    {
      this.service = service;
    }

    [HttpGet]
    public async Task<IEnumerable<ItemDto>> GetItemsAsync(string nameToMatch = null)
    {
      var items = (await service.GetItemsAsync(nameToMatch)).Select(item => item.AsDto());

      return items;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)
    {
      try
      {
        return (await service.GetItemAsync(id)).AsDto();
      }
      catch (System.Exception)
      {
        return NotFound();
      }
    }

    [HttpPost]
    public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto)
    {
      var item = await service.CreateItemAsync(itemDto.Name, itemDto.Description, itemDto.Price);

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
      try
      {
        await service.UpdateItemAsync(id, itemDto.Name, itemDto.Description, itemDto.Price);
        // Convenção => retornar NoContent (204)
        return NoContent();
      }
      catch (ItemNotFoundException ex)
      {
        return NotFound(ex.Message);
      }
    }

    [HttpDelete]
    public async Task<ActionResult> DeleteItemAsync(Guid id)
    {
      try
      {
        await service.DeleteItemAsync(id);
        // Convenção => retornar NoContent (204)
        return NoContent();
      }
      catch (ItemNotFoundException ex)
      {
        return NotFound(ex.Message);
      }
    }
  }
}