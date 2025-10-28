using Account.Domain.Commands.AccountGuides;
using Account.Domain.Models.Entities.AccountGuides;
using Shared.Application.Validators.BaseCommandValidators.CreateCommandValidators;

namespace Account.Application.Validators.CommandValidators.AccountGuides;

public class AccountGuideCreateValidator : BaseSettingCreateValidator<AccountGuideCreateCommand, AccountGuide>
{ }