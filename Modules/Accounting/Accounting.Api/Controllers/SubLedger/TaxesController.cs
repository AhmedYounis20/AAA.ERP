using Accounting.Api.Controllers.SubLedger.Base;
using Accounting.Application.Commands.Taxes;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Api.Extensions;

namespace Accounting.Api.Controllers.SubLedger;

/// <summary>CRUD API for tax sub-ledger accounts (hybrid CQRS pilot).</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class TaxesController(
    ISender sender,
    ITaxQueries queries,
    IBaseQueryService<Tax, SubLeadgerLookupDto> lookups)
    : BaseSubLedgerController<Tax, TaxCreateCommand, TaxUpdateCommand>(sender, queries)
{
    protected override IRequest<ApiResponse<Tax>> BuildDeleteCommand(Guid id)
        => new TaxDeleteCommand(id);

    [HttpGet("lookups")]
    public Task<IActionResult> GetLookUps()
        => this.ToLookupsResult(() => lookups.GetLookUps(e => e.NodeType == NodeType.Domain));
}
