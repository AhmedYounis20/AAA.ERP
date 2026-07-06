using Accounting.Application.Commands.Branches;
using Accounting.Application.Dtos.Lookups;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.SubLeadgers;
using ERP.BuildingBlocks.Api.Controllers;
using ERP.BuildingBlocks.Api.Extensions;
using ERP.BuildingBlocks.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Accounting.Api.Controllers.SubLedger;

/// <summary>CRUD API for branch sub-ledger accounts (hybrid CQRS).</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BranchesController(
    ISender sender,
    IBranchQueries queries,
    IBaseQueryService<Branch, SubLeadgerLookupDto> lookups)
    : BaseSubLedgerController<Branch, BranchCreateCommand, BranchUpdateCommand>(sender, queries)
{
    protected override IRequest<ApiResponse<Branch>> BuildDeleteCommand(Guid id)
        => new BranchDeleteCommand(id);

    [HttpGet("lookups")]
    public Task<IActionResult> GetLookUps()
        => this.ToLookupsResult(() => lookups.GetLookUps(e => e.NodeType == NodeType.Domain));
}
