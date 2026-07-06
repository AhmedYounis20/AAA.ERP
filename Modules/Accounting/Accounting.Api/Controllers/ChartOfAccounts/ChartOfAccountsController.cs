using Accounting.Application.Commands.ChartOfAccounts;
using Accounting.Application.Dtos.Filters;
using Accounting.Application.Dtos.Lookups;
using Accounting.Application.Services;
using Accounting.Application.Services.Queries;
using Accounting.Domain.Models.Entities.ChartOfAccounts;
using ERP.BuildingBlocks.Api.Controllers;
using ERP.BuildingBlocks.Api.Extensions;
using ERP.BuildingBlocks.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Accounting.Api.Controllers.ChartOfAccounts;

/// <summary>CRUD API for chart-of-account tree and code generation (hybrid CQRS).</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ChartOfAccountsController(
    ISender sender,
    IChartOfAccountQueries queries,
    IBaseQueryService<ChartOfAccount, ChartOfAccountLookupDto> lookups)
    : BaseTreeSettingController<ChartOfAccount, ChartOfAccountCreateCommand, ChartOfAccountUpdateCommand>(sender, queries)
{
    protected override IRequest<ApiResponse<ChartOfAccount>> BuildDeleteCommand(Guid id)
        => new ChartOfAccountDeleteCommand(id);

    [HttpGet("paginated")]
    public async Task<IActionResult> GetPaginated(
        [FromQuery] ChartOfAccountFilterDto filter,
        CancellationToken cancellationToken)
        => this.ToResult(await queries.GetPaginated(filter, cancellationToken));

    [HttpGet("lookups")]
    public Task<IActionResult> GetLookUps()
        => this.ToLookupsResult(() => lookups.GetLookUps());

    [HttpGet("generateNewChildCode")]
    public async Task<IActionResult> GenerateNewChildCode([FromQuery] Guid? parentId)
        => Ok(await queries.GenerateNewChildCode(parentId));

    [HttpGet("NextAccountDefaultData")]
    public async Task<IActionResult> NextAccountDefaultData([FromQuery] Guid? parentId)
        => this.ToResult(await queries.GetNextAccountDefaultData(parentId));
}
