using Accounting.Application.Commands.CostCenters;
using ERP.BuildingBlocks.Application.Services.BaseServices;
using Accounting.Domain.Models.Entities.CostCenters;

namespace Accounting.Application.Services;

public interface ICostCenterService : IBaseTreeSettingService<CostCenter, CostCenterCreateCommand, CostCenterUpdateCommand>;
