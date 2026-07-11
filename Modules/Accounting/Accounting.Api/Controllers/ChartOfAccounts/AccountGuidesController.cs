using Accounting.Application.Commands.AccountGuides;
using Accounting.Domain.Models.Entities.AccountGuides;
using ERP.BuildingBlocks.Api.Extensions;

namespace Accounting.Api.Controllers.ChartOfAccounts;

/// <summary>CRUD API for account guide settings and lookups (hybrid CQRS).</summary>
[Route("api/[controller]")]
[ApiController]
[Authorize]
public class AccountGuidesController(
    ISender sender,
    IAccountGuideQueries queries,
    IBaseQueryService<AccountGuide, LookupDto> lookups)
    : BaseSettingController<AccountGuide, AccountGuideCreateCommand, AccountGuideUpdateCommand>(sender, queries)
{
    protected override IRequest<ApiResponse<AccountGuide>> BuildDeleteCommand(Guid id)
        => new AccountGuideDeleteCommand(id);

    [HttpGet("lookups")]
    public Task<IActionResult> GetLookUps()
        => this.ToLookupsResult(() => lookups.GetLookUps());
}
