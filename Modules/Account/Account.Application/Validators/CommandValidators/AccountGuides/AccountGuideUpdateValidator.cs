using Account.Domain.Commands.AccountGuides;
using Account.Domain.Models.Entities.AccountGuides;
using Shared.Application.Validators.BaseCommandValidators.UpdateCommandValidators;

namespace Account.Application.Validators.CommandValidators.AccountGuides;

public class AccountGuideUpdateValidator : BaseSettingUpdateValidator<AccountGuideUpdateCommand, AccountGuide>
{ }