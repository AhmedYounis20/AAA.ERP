using Accounting.Api.Controllers.SubLedger.Base;
using Accounting.Application.Commands.Customers;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Api.Extensions;

namespace Accounting.Api.Controllers.SubLedger;

/// <summary>CRUD API for customer sub-ledger accounts (hybrid CQRS).</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CustomersController(
    ISender sender,
    ICustomerQueries queries,
    IBaseQueryService<Customer, SubLeadgerLookupDto> lookups)
    : BaseSubLedgerController<Customer, CustomerCreateCommand, CustomerUpdateCommand>(sender, queries)
{
    protected override IRequest<ApiResponse<Customer>> BuildDeleteCommand(Guid id)
        => new CustomerDeleteCommand(id);

    [HttpGet("lookups")]
    public Task<IActionResult> GetLookUps()
        => this.ToLookupsResult(() => lookups.GetLookUps(e => e.NodeType == NodeType.Domain));
}
