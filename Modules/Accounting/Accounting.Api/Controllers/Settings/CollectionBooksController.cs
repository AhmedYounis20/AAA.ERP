using Accounting.Application.Commands.CollectionBooks;
using Accounting.Domain.Models.Entities.CollectionBooks;
using ERP.BuildingBlocks.Api.Extensions;

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
