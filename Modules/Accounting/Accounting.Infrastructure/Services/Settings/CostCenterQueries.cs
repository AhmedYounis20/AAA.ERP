using Accounting.Application.Commands.CostCenters;
using Accounting.Application.Services;
using Accounting.Domain.Models.Entities.CostCenters;
using ERP.Infrastructure.Services.Queries;

namespace Accounting.Infrastructure.Services;

/// <summary>
/// Cost center read queries. Transitional: delegates to <see cref="ICostCenterService"/> until write methods are removed.
/// </summary>
public class CostCenterQueries(ICostCenterService costCenterService)
    : BaseTreeSettingQueries<CostCenter, CostCenterCreateCommand, CostCenterUpdateCommand>(costCenterService),
      ICostCenterQueries;
