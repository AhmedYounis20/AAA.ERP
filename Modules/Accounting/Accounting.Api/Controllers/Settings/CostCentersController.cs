using Accounting.Application.Commands.CostCenters;
using Accounting.Application.Dtos.Lookups;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.CostCenters;
using ERP.BuildingBlocks.Api.Controllers;
using ERP.BuildingBlocks.Api.Extensions;
using ERP.BuildingBlocks.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Accounting.Api.Controllers.Settings;

/// <summary>CRUD API for cost center tree and domain lookups (hybrid CQRS).</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CostCentersController(
    ISender sender,
    ICostCenterQueries queries,
    IBaseQueryService<CostCenter, CostCenterLookupDto> lookups)
    : BaseTreeSettingController<CostCenter, CostCenterCreateCommand, CostCenterUpdateCommand>(sender, queries)
{
    protected override IRequest<ApiResponse<CostCenter>> BuildDeleteCommand(Guid id)
        => new CostCenterDeleteCommand(id);

    [HttpGet("lookups")]
    public Task<IActionResult> GetLookUps()
        => this.ToLookupsResult(() => lookups.GetLookUps(e => e.NodeType == NodeType.Domain));
}
