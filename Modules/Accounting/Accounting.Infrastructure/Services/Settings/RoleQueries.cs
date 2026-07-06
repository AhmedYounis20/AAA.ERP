using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.Roles;
using ERP.Infrastructure.Services.Queries;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Role read queries.
/// </summary>
public class RoleQueries(IRoleRepository repository)
    : BaseSettingRepositoryQueries<Role>(repository),
      IRoleQueries;
