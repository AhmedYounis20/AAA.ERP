using Accounting.Api.Controllers.SubLedger.Base;
using Accounting.Application.Commands.Suppliers;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Api.Extensions;

namespace Accounting.Api.Controllers.SubLedger;

/// <summary>CRUD API for supplier sub-ledger accounts (hybrid CQRS).</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class SuppliersController(
    ISender sender,
    ISupplierQueries queries,
    IBaseQueryService<Supplier, SubLeadgerLookupDto> lookups)
    : BaseSubLedgerController<Supplier, SupplierCreateCommand, SupplierUpdateCommand>(sender, queries)
{
    protected override IRequest<ApiResponse<Supplier>> BuildDeleteCommand(Guid id)
        => new SupplierDeleteCommand(id);

    [HttpGet("lookups")]
    public Task<IActionResult> GetLookUps()
        => this.ToLookupsResult(() => lookups.GetLookUps(e => e.NodeType == NodeType.Domain));
}
