using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.AccountGuides;
using ERP.Infrastructure.Services.Queries;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Account guide read queries.
/// </summary>
public class AccountGuideQueries(IAccountGuideRepository repository)
    : BaseSettingRepositoryQueries<AccountGuide>(repository),
      IAccountGuideQueries;
