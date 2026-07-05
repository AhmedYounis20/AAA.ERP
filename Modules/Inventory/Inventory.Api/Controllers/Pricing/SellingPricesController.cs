using Inventory.Application.Commands.SellingPrices;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.SellingPrices;
using Inventory.Application.Dtos.Lookups;
using Accounting.Application.Services.Queries;
using ERP.BuildingBlocks.Api.Controllers;
using ERP.BuildingBlocks.Api.Extensions;
using ERP.BuildingBlocks.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers.Pricing;

/// <summary>CRUD API for selling price settings and lookup queries (hybrid CQRS).</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SellingPricesController(
    ISender sender,
    ISellingPriceQueries queries,
    IBaseQueryService<SellingPrice, SellingPriceLookupDto> lookups)
    : HybridSettingController<SellingPrice, SellingPriceCreateCommand, SellingPriceUpdateCommand>(sender, queries)
{
    protected override IRequest<ApiResponse<SellingPrice>> BuildDeleteCommand(Guid id)
        => new SellingPriceDeleteCommand(id);

    [HttpGet]
    public override async Task<IActionResult> Get()
        => this.ToResult(await queries.GetDtos());

    [HttpGet("lookups")]
    public Task<IActionResult> GetLookUps()
        => this.ToLookupsResult(() => lookups.GetLookUps());
}
