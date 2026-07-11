using Accounting.Api.Controllers.SubLedger.Base;
using Accounting.Application.Commands.CashInBoxes;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Api.Extensions;

namespace Accounting.Api.Controllers.SubLedger;

/// <summary>CRUD API for cash-in-box sub-ledger accounts (hybrid CQRS).</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CashInBoxesController(
    ISender sender,
    ICashInBoxQueries queries,
    IBaseQueryService<CashInBox, SubLeadgerLookupDto> lookups)
    : BaseSubLedgerController<CashInBox, CashInBoxCreateCommand, CashInBoxUpdateCommand>(sender, queries)
{
    protected override IRequest<ApiResponse<CashInBox>> BuildDeleteCommand(Guid id)
        => new CashInBoxDeleteCommand(id);

    [HttpGet("lookups")]
    public Task<IActionResult> GetLookUps()
        => this.ToLookupsResult(() => lookups.GetLookUps(e => e.NodeType == NodeType.Domain));
}
