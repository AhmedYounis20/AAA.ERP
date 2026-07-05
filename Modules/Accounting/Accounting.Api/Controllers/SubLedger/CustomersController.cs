using Accounting.Application.Commands.Customers;
using Accounting.Application.Dtos.Lookups;
using Accounting.Application.Services;
using Accounting.Application.Services.Queries;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Api.Controllers;
using ERP.BuildingBlocks.Api.Extensions;
using ERP.BuildingBlocks.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Accounting.Api.Controllers.SubLedger;

/// <summary>CRUD API for customer sub-ledger accounts (hybrid CQRS).</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CustomersController(
    ISender sender,
    ICustomerQueries queries,
    IBaseQueryService<Customer, SubLeadgerLookupDto> lookups)
    : HybridSubLedgerController<Customer, CustomerCreateCommand, CustomerUpdateCommand>(sender, queries)
{
    protected override IRequest<ApiResponse<Customer>> BuildDeleteCommand(Guid id)
        => new CustomerDeleteCommand(id);

    [HttpGet("lookups")]
    public Task<IActionResult> GetLookUps()
        => this.ToLookupsResult(() => lookups.GetLookUps(e => e.NodeType == NodeType.Domain));
}
