using Domain.Account.Commands.Currencies;
using Domain.Account.Models.Entities.Currencies;
using Domain.Account.Services.BaseServices.interfaces;

namespace Domain.Account.Services.Interfaces;

public interface ICostCenterService: IBaseTreeSettingService<CostCenter,CostCenterCreateCommand,CostCenterUpdateCommand>{}