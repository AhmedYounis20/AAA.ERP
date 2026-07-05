using Inventory.Application.Commands.Colors;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Colors;
using ERP.BuildingBlocks.Api.Controllers;
using ERP.BuildingBlocks.Api.Extensions;
using ERP.BuildingBlocks.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers.Catalog;

/// <summary>CRUD API for inventory color settings (hybrid CQRS pilot).</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ColorsController(ISender sender, IColorQueries queries)
    : HybridSettingController<Color, ColorCreateCommand, ColorUpdateCommand>(sender, queries)
{
    protected override IRequest<ApiResponse<Color>> BuildDeleteCommand(Guid id)
        => new ColorDeleteCommand(id);

    [HttpGet("nextCode")]
    public async Task<IActionResult> GetNextCode()
        => this.ToResult(await queries.GetNextCode());
}
