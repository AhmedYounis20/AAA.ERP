using Account.Domain.Commands.CostCenters;
using Account.Domain.Models.Entities.CostCenters;
using Shared.Application.BaseServices;

namespace Account.Application.Services;

public interface ICostCenterService: IBaseTreeSettingService<CostCenter,CostCenterCreateCommand,CostCenterUpdateCommand>{}