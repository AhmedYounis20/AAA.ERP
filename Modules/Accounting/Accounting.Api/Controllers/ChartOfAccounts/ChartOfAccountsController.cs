using Accounting.Application.Commands.ChartOfAccounts;
using Accounting.Application.Dtos.Filters;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using ERP.BuildingBlocks.Api.Extensions;
using ERP.BuildingBlocks.Application.Constants;

namespace Accounting.Api.Controllers.ChartOfAccounts;

/// <summary>CRUD API for chart-of-account tree and code generation (hybrid CQRS).</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ChartOfAccountsController(
    ISender sender,
    IChartOfAccountReadService reads,
    IBaseQueryService<ChartOfAccount, ChartOfAccountLookupDto> lookups) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] ChartOfAccountCreateCommand input)
        => this.ToResult(await sender.Send(input), HttpStatusCode.Created, SuccessMessages.CreatedSuccessfully);

    [HttpGet]
    public async Task<IActionResult> Get()
        => this.ToResult(await reads.GetAll());

    [HttpGet("paginated")]
    public async Task<IActionResult> GetPaginated([FromQuery] ChartOfAccountFilterDto filter, CancellationToken cancellationToken)
        => this.ToResult(await reads.GetPaginated(filter, cancellationToken));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
        => this.ToResult(await reads.GetById(id));

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] ChartOfAccountUpdateCommand input)
    {
        input.Id = id;
        return this.ToResult(await sender.Send(input), successMessageKey: SuccessMessages.UpdatedSuccessfully);
    }

    [HttpGet("lookups")]
    public async Task<IActionResult> GetLookUps()
    {
        try
        {
            return this.ToResult(new ApiResponse<IEnumerable<ChartOfAccountLookupDto>>
            {
                IsSuccess = true,
                Result = await lookups.GetLookUps()
            });
        }
        catch
        {
            return this.ToResult(new ApiResponse<IEnumerable<ChartOfAccountLookupDto>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest
            });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
        => this.ToResult(await sender.Send(new ChartOfAccountDeleteCommand(id)), successMessageKey: SuccessMessages.DeletedSuccessfully);

    [HttpGet("generateNewChildCode")]
    public async Task<IActionResult> GenerateNewChildCode([FromQuery] Guid? parentId)
        => Ok(await reads.GenerateNewChildCode(parentId));

    [HttpGet("NextAccountDefaultData")]
    public async Task<IActionResult> NextAccountDefaultData([FromQuery] Guid? parentId)
        => Ok(await reads.GetNextAccountDefaultData(parentId));
}
