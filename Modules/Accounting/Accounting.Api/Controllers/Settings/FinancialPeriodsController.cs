using Accounting.Application.Commands.FinancialPeriods;
using Accounting.Application.Dtos.Filters;
using Accounting.Application.Dtos.Lookups;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.FinancialPeriods;
using ERP.BuildingBlocks.Api.Extensions;
using ERP.BuildingBlocks.Application.Constants;

namespace Accounting.Api.Controllers.Settings;

/// <summary>CRUD API for financial periods and current-period queries (hybrid CQRS).</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class FinancialPeriodsController(
    ISender sender,
    IFinancialPeriodReadService reads,
    IBaseQueryService<FinancialPeriod, FinancialPeriodLookupDto> lookups) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] FinancialPeriodCreateCommand input)
        => this.ToResult(await sender.Send(input), HttpStatusCode.Created, SuccessMessages.CreatedSuccessfully);

    [HttpGet]
    public async Task<IActionResult> Get()
        => this.ToResult(await reads.GetAll());

    [HttpGet("paginated")]
    public async Task<IActionResult> GetPaginated([FromQuery] FinancialPeriodFilterDto filter, CancellationToken cancellationToken)
        => this.ToResult(await reads.GetPaginated(filter, cancellationToken));

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
        => this.ToResult(await reads.GetById(id));

    [HttpGet("nextDefaultdata")]
    public async Task<IActionResult> GetNextDefaultdata()
        => this.ToResult(await reads.GetNextDefaultdata());

    [HttpGet("GetCurrentFinancialPeriod")]
    public async Task<IActionResult> GetCurrentFinancialPeriod()
        => this.ToResult(await reads.GetCurrentFinancialPeriod());

    [HttpGet("lookups")]
    public async Task<IActionResult> GetLookUps()
    {
        try
        {
            return this.ToResult(new ApiResponse<IEnumerable<FinancialPeriodLookupDto>>
            {
                IsSuccess = true,
                Result = await lookups.GetLookUps()
            });
        }
        catch
        {
            return this.ToResult(new ApiResponse<IEnumerable<FinancialPeriodLookupDto>>
            {
                IsSuccess = false,
                StatusCode = HttpStatusCode.BadRequest
            });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] FinancialPeriodUpdateCommand input)
    {
        input.Id = id;
        return this.ToResult(await sender.Send(input), successMessageKey: SuccessMessages.UpdatedSuccessfully);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
        => this.ToResult(await sender.Send(new FinancialPeriodDeleteCommand(id)), successMessageKey: SuccessMessages.DeletedSuccessfully);
}
