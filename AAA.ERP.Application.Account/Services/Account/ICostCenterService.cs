using ERP.Application.Services.BaseServices;
using ERP.Domain.Commands.Account.CostCenters;
using ERP.Domain.Models.Entities.Account.CostCenters;

namespace ERP.Application.Services.Account;

public interface ICostCenterService: IBaseTreeSettingService<CostCenter,CostCenterCreateCommand,CostCenterUpdateCommand>{}