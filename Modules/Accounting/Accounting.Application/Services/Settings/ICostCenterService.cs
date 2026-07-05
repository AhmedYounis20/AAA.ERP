using Accounting.Application.Commands.CostCenters;
using ERP.BuildingBlocks.Application.Services.BaseServices;
using Accounting.Domain.Models.Entities.CostCenters;

namespace Accounting.Application.Services;

/// <summary>
/// Application service contract for cost center.
/// </summary>
public interface ICostCenterService : IBaseTreeSettingService<CostCenter, CostCenterCreateCommand, CostCenterUpdateCommand>;
