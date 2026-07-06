using Accounting.Application.Commands.Banks;
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

/// <summary>CRUD API for bank sub-ledger accounts (hybrid CQRS).</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class BanksController(
    ISender sender,
    IBankQueries queries,
    IBaseQueryService<Bank, SubLeadgerLookupDto> lookups)
    : BaseSubLedgerController<Bank, BankCreateCommand, BankUpdateCommand>(sender, queries)
{
    protected override IRequest<ApiResponse<Bank>> BuildDeleteCommand(Guid id)
        => new BankDeleteCommand(id);

    [HttpGet("lookups")]
    public Task<IActionResult> GetLookUps()
        => this.ToLookupsResult(() => lookups.GetLookUps(e => e.NodeType == NodeType.Domain));
}
