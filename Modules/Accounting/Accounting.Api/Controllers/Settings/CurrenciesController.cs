using Accounting.Application.Commands.Currencies;
using Accounting.Application.Dtos.Lookups;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.Currencies;
using ERP.BuildingBlocks.Api.Controllers;
using ERP.BuildingBlocks.Api.Extensions;
using ERP.BuildingBlocks.Application.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Accounting.Api.Controllers.Settings;

/// <summary>CRUD API for currency settings and lookups (hybrid CQRS).</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class CurrenciesController(
    ISender sender,
    ICurrencyQueries queries,
    IBaseQueryService<Currency, CurrencyLookupDto> lookups)
    : BaseSettingController<Currency, CurrencyCreateCommand, CurrencyUpdateCommand>(sender, queries)
{
    protected override IRequest<ApiResponse<Currency>> BuildDeleteCommand(Guid id)
        => new CurrencyDeleteCommand(id);

    [HttpGet("lookups")]
    public Task<IActionResult> GetLookUps()
        => this.ToLookupsResult(() => lookups.GetLookUps());
}
