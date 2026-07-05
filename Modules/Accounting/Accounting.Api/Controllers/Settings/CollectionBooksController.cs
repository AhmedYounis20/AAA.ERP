using Accounting.Application.Commands.CollectionBooks;
using Accounting.Application.Dtos.Filters;
using Accounting.Application.Services;
using Accounting.Application.Services.Queries;
using Accounting.Domain.Models.Entities.CollectionBooks;
using ERP.BuildingBlocks.Api.Extensions;
using ERP.BuildingBlocks.Application.Constants;
using ERP.BuildingBlocks.Application.DTOs;

namespace Accounting.Api.Controllers.Settings;

/// <summary>CRUD API for collection book settings and lookups (hybrid CQRS).</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CollectionBooksController(
    ISender sender,
    ICollectionBookReadService reads,
    IBaseQueryService<CollectionBook, LookupDto> lookups) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CollectionBookCreateCommand input)
        => this.ToResult(await sender.Send(input), HttpStatusCode.Created, SuccessMessages.CreatedSuccessfully);

    [HttpGet]
    public async Task<IActionResult> Get() => this.ToResult(await reads.GetAll());

    [HttpGet("paginated")]
    public async Task<IActionResult> GetPaginated([FromQuery] SettingFilterDto filter, CancellationToken cancellationToken)
        => this.ToResult(await reads.GetPaginated(filter, cancellationToken));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id) => this.ToResult(await reads.GetById(id));

    [HttpGet("lookups")]
    public async Task<IActionResult> GetLookUps()
    {
        try
        {
            return this.ToResult(new ApiResponse<IEnumerable<LookupDto>> { IsSuccess = true, Result = await lookups.GetLookUps() });
        }
        catch
        {
            return this.ToResult(new ApiResponse<IEnumerable<LookupDto>> { IsSuccess = false, StatusCode = HttpStatusCode.BadRequest });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] CollectionBookUpdateCommand input)
    {
        input.Id = id;
        return this.ToResult(await sender.Send(input), successMessageKey: SuccessMessages.UpdatedSuccessfully);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
        => this.ToResult(await sender.Send(new CollectionBookDeleteCommand(id)), successMessageKey: SuccessMessages.DeletedSuccessfully);
}
