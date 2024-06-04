using Domain.Account.Models.Entities.ChartOfAccounts;
using Domain.Account.Validators.BussinessValidator.BaseBussinessValidators.Interfaces;

namespace Domain.Account.Validators.BussinessValidator.Interfaces;

public interface IChartOfAccountBussinessValidator : IBaseTreeSettingBussinessValidator<ChartOfAccount>
{ }