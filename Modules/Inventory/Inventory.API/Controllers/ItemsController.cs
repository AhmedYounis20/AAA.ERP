using Inventory.Application.Services;
using Inventory.Domain.Commands.Items;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.Domain.Responses;

namespace Inventory.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private readonly IItemService _itemService;
    private readonly ISender _sender;

    public ItemsController(IItemService itemService, ISender sender)
    {
        _itemService = itemService;
        _sender = sender;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ItemCreateCommand input)
    {
        var result = await _sender.Send(input);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var result = await _itemService.ReadAll();
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var result = await _itemService.ReadById(id);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ItemUpdateCommand input)
    {
        input.Id = id;
        var result = await _sender.Send(input);
        return StatusCode((int)result.StatusCode, result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _itemService.Delete(id);
        return StatusCode((int)result.StatusCode, result);
    }
}
