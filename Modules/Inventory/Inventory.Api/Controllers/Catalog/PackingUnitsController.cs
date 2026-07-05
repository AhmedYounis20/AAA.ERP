using Inventory.Application.Commands.PackingUnits;
using Inventory.Application.Dtos.Lookups;
using Inventory.Application.Services;
using Accounting.Application.Services.Queries;
using Inventory.Domain.Models.Entities.PackingUnits;
using ERP.BuildingBlocks.Api.Controllers;
using ERP.BuildingBlocks.Api.Extensions;
using ERP.BuildingBlocks.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers.Catalog;

/// <summary>CRUD API for packing unit settings and lookup queries (hybrid CQRS).</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class PackingUnitsController(
    ISender sender,
    IPackingUnitQueries queries,
    IBaseQueryService<PackingUnit, PackingUnitLookupDto> lookups)
    : HybridSettingController<PackingUnit, PackingUnitCreateCommand, PackingUnitUpdateCommand>(sender, queries)
{
    protected override IRequest<ApiResponse<PackingUnit>> BuildDeleteCommand(Guid id)
        => new PackingUnitDeleteCommand(id);

    [HttpGet("lookups")]
    public Task<IActionResult> GetLookUps()
        => this.ToLookupsResult(() => lookups.GetLookUps());
}
