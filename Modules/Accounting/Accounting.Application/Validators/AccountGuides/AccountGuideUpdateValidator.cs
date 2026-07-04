using Accounting.Application.Commands.AccountGuides;
using ERP.Application.Validators.Account.ComandValidators.BaseCommandValidators.UpdateCommandValidators;
using ERP.Domain.Models.Entities.Account.AccountGuides;

namespace Accounting.Application.Validators.AccountGuides;

public class AccountGuideUpdateValidator : BaseSettingUpdateValidator<AccountGuideUpdateCommand, AccountGuide>;
