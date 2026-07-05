using Inventory.Application.Commands.Sizes;
using Inventory.Domain.Models.Entities.Sizes;
using ERP.BuildingBlocks.Api.Extensions;
using ERP.BuildingBlocks.Application.Constants;

namespace Inventory.Api.Controllers.Catalog;

/// <summary>CRUD API for inventory size settings (hybrid CQRS).</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SizesController(ISender sender, ISizeReadService reads) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] SizeCreateCommand input)
        => this.ToResult(await sender.Send(input), HttpStatusCode.Created, SuccessMessages.CreatedSuccessfully);

    [HttpGet]
    public async Task<IActionResult> Get()
        => this.ToResult(await reads.GetAll());

    [HttpGet("paginated")]
    public async Task<IActionResult> GetPaginated([FromQuery] SettingFilterDto filter, CancellationToken cancellationToken)
        => this.ToResult(await reads.GetPaginated(filter, cancellationToken));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
        => this.ToResult(await reads.GetById(id));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] SizeUpdateCommand input)
    {
        input.Id = id;
        return this.ToResult(await sender.Send(input), successMessageKey: SuccessMessages.UpdatedSuccessfully);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
        => this.ToResult(await sender.Send(new SizeDeleteCommand(id)), successMessageKey: SuccessMessages.DeletedSuccessfully);

    [HttpGet("nextCode")]
    public async Task<IActionResult> GetNextCode()
        => this.ToResult(await reads.GetNextCode());
}
