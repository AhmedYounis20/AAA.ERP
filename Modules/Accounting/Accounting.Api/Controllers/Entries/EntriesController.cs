using Accounting.Application.Commands.Entries;
using Accounting.Application.Dtos.Filters;
using Accounting.Application.Services.Entries;
using ERP.BuildingBlocks.Api.Extensions;
using ERP.BuildingBlocks.Application.Constants;

namespace Accounting.Api.Controllers.Entries;

/// <summary>CRUD API for complex accounting entries (hybrid CQRS).</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class EntriesController(ISender sender, IComplexEntryQueries queries) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ComplexEntryCreateCommand input)
        => this.ToResult(await sender.Send(input), HttpStatusCode.Created, SuccessMessages.CreatedSuccessfully);

    [HttpGet]
    public async Task<IActionResult> Get()
        => this.ToResult(await queries.GetAll());

    [HttpGet("paginated")]
    public async Task<IActionResult> GetPaginated([FromQuery] EntryFilterDto filter, CancellationToken cancellationToken)
        => this.ToResult(await queries.GetPaginated(filter, cancellationToken));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
        => this.ToResult(await queries.GetById(id));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, ComplexEntryUpdateCommand input)
    {
        input.Id = id;
        return this.ToResult(await sender.Send(input), successMessageKey: SuccessMessages.UpdatedSuccessfully);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
        => this.ToResult(await sender.Send(new ComplexEntryDeleteCommand(id)), successMessageKey: SuccessMessages.DeletedSuccessfully);

    [HttpGet("GetEntryNumber")]
    public async Task<IActionResult> GetEntryNumber([FromQuery] DateTime dateTime)
        => this.ToResult(await queries.GetEntryNumber(dateTime));
}
