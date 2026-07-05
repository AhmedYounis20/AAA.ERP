using Accounting.Application.Commands.AccountGuides;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.AccountGuides;
using ERP.Infrastructure.Services.Queries;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Account guide read queries. Transitional: delegates to <see cref="IAccountGuideService"/> until write methods are removed.
/// </summary>
public class AccountGuideQueries(IAccountGuideService accountGuideService)
    : BaseSettingQueries<AccountGuide, AccountGuideCreateCommand, AccountGuideUpdateCommand>(accountGuideService),
      IAccountGuideQueries;
