using Accounting.Application.Commands.CollectionBooks;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.CollectionBooks;
using ERP.BuildingBlocks.Api.Controllers;
using ERP.BuildingBlocks.Api.Extensions;
using ERP.BuildingBlocks.Application.DTOs;
using ERP.BuildingBlocks.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Accounting.Api.Controllers.Settings;

/// <summary>CRUD API for collection book settings and lookups (hybrid CQRS).</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CollectionBooksController(
    ISender sender,
    ICollectionBookQueries queries,
    IBaseQueryService<CollectionBook, LookupDto> lookups)
    : BaseSettingController<CollectionBook, CollectionBookCreateCommand, CollectionBookUpdateCommand>(sender, queries)
{
    protected override IRequest<ApiResponse<CollectionBook>> BuildDeleteCommand(Guid id)
        => new CollectionBookDeleteCommand(id);

    [HttpGet("lookups")]
    public Task<IActionResult> GetLookUps()
        => this.ToLookupsResult(() => lookups.GetLookUps());
}
