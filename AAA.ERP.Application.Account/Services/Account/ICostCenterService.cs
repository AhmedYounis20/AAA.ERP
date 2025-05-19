using Domain.Account.Commands.Currencies;
using Domain.Account.Models.Entities.CostCenters;
using ERP.Application.Services.BaseServices;

namespace ERP.Application.Services.Account;

public interface ICostCenterService: IBaseTreeSettingService<CostCenter,CostCenterCreateCommand,CostCenterUpdateCommand>{}