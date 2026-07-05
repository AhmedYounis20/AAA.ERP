using Accounting.Application.Commands.CostCenters;
using ERP.Application.Services.BaseServices;
using ERP.Domain.Models.Entities.Account.CostCenters;

namespace Accounting.Application.Services;

public interface ICostCenterService : IBaseTreeSettingService<CostCenter, CostCenterCreateCommand, CostCenterUpdateCommand>;
