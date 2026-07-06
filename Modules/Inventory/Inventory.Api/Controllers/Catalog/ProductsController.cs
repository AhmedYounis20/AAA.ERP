using Inventory.Application.Commands.Products;
using Inventory.Application.Services;
using Inventory.Domain.Models.Entities.Products;
using ERP.BuildingBlocks.Api.Controllers;
using ERP.BuildingBlocks.Api.Extensions;
using ERP.BuildingBlocks.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Inventory.Api.Controllers.Catalog;

/// <summary>CRUD API for product catalog tree (hybrid CQRS).</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ProductsController(ISender sender, IProductQueries queries)
    : HybridTreeSettingController<Product, ProductCreateCommand, ProductUpdateCommand>(sender, queries)
{
    protected override IRequest<ApiResponse<Product>> BuildDeleteCommand(Guid id)
        => new ProductDeleteCommand(id);

    [HttpGet]
    public override async Task<IActionResult> Get()
        => this.ToResult(await queries.GetAll());

    [HttpGet("{id}")]
    public override async Task<IActionResult> Get(Guid id)
        => this.ToResult(await queries.GetById(id));

    [HttpGet("getNextCode")]
    public async Task<IActionResult> GetNextCode([FromQuery] Guid? parentId)
        => this.ToResult(await queries.GetNextCode(parentId));
}
