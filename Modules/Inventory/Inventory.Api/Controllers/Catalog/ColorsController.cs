using Inventory.Application.Commands.Colors;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Colors;
using ERP.BuildingBlocks.Api.Extensions;
using ERP.BuildingBlocks.Application.Constants;
using ERP.BuildingBlocks.Application.DTOs;

namespace Inventory.Api.Controllers.Catalog;

/// <summary>CRUD API for inventory color settings (hybrid CQRS pilot).</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ColorsController(ISender sender, IColorQueries queries) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ColorCreateCommand input)
        => this.ToResult(await sender.Send(input), HttpStatusCode.Created, SuccessMessages.CreatedSuccessfully);

    [HttpGet]
    public async Task<IActionResult> Get()
        => this.ToResult(await queries.GetAll());

    [HttpGet("paginated")]
    public async Task<IActionResult> GetPaginated([FromQuery] SettingFilterDto filter, CancellationToken cancellationToken)
        => this.ToResult(await queries.GetPaginated(filter, cancellationToken));

    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] string query)
        => this.ToResult(await queries.SearchByName(query));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
        => this.ToResult(await queries.GetById(id));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ColorUpdateCommand input)
    {
        input.Id = id;
        return this.ToResult(await sender.Send(input), successMessageKey: SuccessMessages.UpdatedSuccessfully);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
        => this.ToResult(await sender.Send(new ColorDeleteCommand(id)), successMessageKey: SuccessMessages.DeletedSuccessfully);

    [HttpGet("nextCode")]
    public async Task<IActionResult> GetNextCode()
        => this.ToResult(await queries.GetNextCode());
}
