using Inventory.Application.Commands.Sizes;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Sizes;
using ERP.BuildingBlocks.Api.Controllers;
using ERP.BuildingBlocks.Api.Extensions;
using ERP.BuildingBlocks.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers.Catalog;

/// <summary>CRUD API for inventory size settings (hybrid CQRS).</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SizesController(ISender sender, ISizeQueries queries)
    : HybridSettingController<Size, SizeCreateCommand, SizeUpdateCommand>(sender, queries)
{
    protected override IRequest<ApiResponse<Size>> BuildDeleteCommand(Guid id)
        => new SizeDeleteCommand(id);

    [HttpGet("nextCode")]
    public async Task<IActionResult> GetNextCode()
        => this.ToResult(await queries.GetNextCode());
}
