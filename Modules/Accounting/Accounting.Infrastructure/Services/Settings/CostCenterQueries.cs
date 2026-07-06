using Accounting.Application.Repositories;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.CostCenters;
using ERP.Infrastructure.Services.Queries;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Cost center read queries.
/// </summary>
public class CostCenterQueries(ICostCenterRepository repository)
    : BaseTreeSettingRepositoryQueries<CostCenter>(repository),
      ICostCenterQueries;
